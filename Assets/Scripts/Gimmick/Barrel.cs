using Actor;
using AutoGenerate;
using Event;
using Particle;
using Sounds;
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

        public override int Level => 1;

        private void Start()
        {
            AttackEvent
                .RegisterListenerInRange(transform)
                .Subscribe(Explosion)
                .AddTo(this);
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.DrawWireSphere(transform.position, explosionRange);
        }

        private void Explosion(IEvent _)
        {
            var trans = transform;
            var pos = trans.position;
            EventPublisher.Instance.PublishEvent(new AttackEvent
            {
                SourcePos = pos,
                Amount = damage,
                AttackRange = explosionRange,
                KnockBackPower = explosionPower,
                Source = trans
            });
            ParticleManager.Instance.PlayVfx(VfxEnum.Explosion, 1, pos);
            Destroy(gameObject);
        }
    }
}