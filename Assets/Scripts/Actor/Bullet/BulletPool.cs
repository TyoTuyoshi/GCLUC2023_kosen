using System.Collections.Generic;
using Event;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace Actor.Bullet
{
    /// <summary>
    ///     弾丸のインスタンスの使いまわしと発射
    /// </summary>
    public class BulletPool : MonoBehaviour
    {
        private readonly Dictionary<int, Stack<BulletBase>> _pool = new();

        private void Start()
        {
            EventPublisher.Instance
                .RegisterListener<ShotEvent>()
                .Subscribe(OnShot)
                .AddTo(this);
        }

        private void OnShot(ShotEvent e)
        {
            var bullet = GetBullet(e);
            bullet.Shot(e.Pos, e.Dir);
        }

        private BulletBase GetBullet(ShotEvent e)
        {
            var id = e.Prefab.GetInstanceID();
            if (_pool.ContainsKey(id))
            {
                if (!_pool[id].TryPop(out var bullet)) return CreateInstance(e);

                bullet.gameObject.SetActive(true);
                bullet.transform.position = e.Pos;
                return bullet;
            }

            _pool[id] = new Stack<BulletBase>();
            return CreateInstance(e);
        }

        private BulletBase CreateInstance(ShotEvent e)
        {
            var prefab = e.Prefab;
            var instance = Instantiate(prefab);
            var id = prefab.GetInstanceID();

            instance
                .OnDisableAsObservable()
                .Subscribe(_ => _pool[id].Push(instance))
                .AddTo(instance);

            instance.transform.position = e.Pos;
            return instance;
        }
    }
}