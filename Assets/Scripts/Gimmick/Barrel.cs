using System;
using Actor;
using UniRx;
using UnityEngine;

namespace Gimmick
{
    /// <summary>
    ///     樽のギミック
    /// </summary>
    public class Barrel : ActorBase, IGimmick
    {
        [SerializeField] private float explosionRange = 1f;
        [SerializeField] private float damage = 2f;
        [SerializeField] private float explosionPower = 2f;
        [SerializeField] private ContactFilter2D contactFilter;

        private readonly Subject<IActorEvent> _onActorEvent = new();

        public override int Level => 1;

        private void Start()
        {
            _onActorEvent
                .Where(e => e is DamageEvent)
                .Subscribe(Explosion)
                .AddTo(this);
        }

        public override void PublishActorEvent(IActorEvent ev)
        {
            _onActorEvent.OnNext(ev);
        }

        private void Explosion(IActorEvent _)
        {
            var result = new RaycastHit2D[10];
            var cnt = Physics2D.CircleCast(transform.position, explosionRange, Vector2.zero, contactFilter, result);
            if (cnt == 0) return;

            foreach (var hit in result)
            {
                if (!hit.transform.TryGetComponent(out ActorBase actor)) continue;
                actor.PublishActorEvent(new DamageEvent
                {
                    Damage = damage,
                    KnockBackDir = (hit.rigidbody.position - (Vector2)transform.position).normalized * explosionPower
                });
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.DrawWireSphere(transform.position, explosionRange);
        }
    }
}