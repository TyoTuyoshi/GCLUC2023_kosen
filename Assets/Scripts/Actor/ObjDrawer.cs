using System;
using System.Collections.Generic;
using System.Linq;
using Event;
using UniRx;
using UnityEngine;

namespace Actor
{
    /// <summary>
    ///     Actorの描画順序を管理
    /// </summary>
    public class ObjDrawer : MonoBehaviour
    {
        private readonly HashSet<SortedRenderer> _objs = new();
        private Player.Player _player;

        private void Start()
        {
            EventPublisher.Instance
                .RegisterListener<SpawnDestroyEvent>()
                .Subscribe(e =>
                {
                    if (e.IsSpawn)
                        _objs.Add(new SortedRenderer(e));
                    else
                        _objs.Remove(new SortedRenderer(e));
                })
                .AddTo(this);
        }

        private void Update()
        {
            var order = 0;

            foreach (var sortedRenderer in from obj in _objs orderby obj select  obj)
            {
                sortedRenderer.Order = order;
                order = sortedRenderer.Order;
                order--;
            }
        }

        private class SortedRenderer : IComparable<SortedRenderer>, IEquatable<SortedRenderer>
        {
            private readonly SpriteRendererComposite _composite;

            private readonly bool _isNullComposite;
            private readonly SpriteRenderer _spriteRenderer;

            public readonly Transform Transform;

            public SortedRenderer(SpriteRendererComposite composite, SpriteRenderer spriteRenderer, Transform transform)
            {
                Transform = transform;
                _composite = composite;
                _spriteRenderer = spriteRenderer;

                if (_composite == null) _isNullComposite = true;
            }

            public SortedRenderer(SpawnDestroyEvent e)
            {
                Transform = e.Transform;
                _composite = e.Composite;
                _spriteRenderer = e.SpriteRenderer;

                if (_composite == null) _isNullComposite = true;
            }

            public int Order
            {
                get => _isNullComposite ? _spriteRenderer.sortingOrder : _composite.SortingOrder;
                set
                {
                    if (_isNullComposite) _spriteRenderer.sortingOrder = value;
                    else _composite.SortingOrder = value;
                }
            }

            public int CompareTo(SortedRenderer other)
            {
                if (ReferenceEquals(this, other)) return 0;
                if (ReferenceEquals(null, other)) return 1;
                return Transform.position.y.CompareTo(other.Transform.position.y);
            }

            public bool Equals(SortedRenderer other)
            {
                if (ReferenceEquals(null, other)) return false;
                if (ReferenceEquals(this, other)) return true;
                return Equals(Transform, other.Transform);
            }

            public override bool Equals(object obj)
            {
                if (ReferenceEquals(null, obj)) return false;
                if (ReferenceEquals(this, obj)) return true;
                if (obj.GetType() != GetType()) return false;
                return Equals((SortedRenderer)obj);
            }

            public override int GetHashCode()
            {
                return Transform != null ? Transform.GetHashCode() : 0;
            }
        }
    }
}