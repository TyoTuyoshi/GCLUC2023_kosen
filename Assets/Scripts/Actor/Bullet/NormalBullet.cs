using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Event;
using UnityEngine;

namespace Actor.Bullet
{
    /// <summary>
    ///     通常の弾丸
    /// </summary>
    [RequireComponent(typeof(Rigidbody2D))]
    public class NormalBullet : BulletBase
    {
        private (Vector3, Vector3) _event;
        private Rigidbody2D _rigid;
        private CancellationTokenSource _source = new();

        private void OnDestroy()
        {
            _source.Cancel();
        }

        public override async void Shot(Vector3 pos, Vector3 dir)
        {
            if (_rigid == null) TryGetComponent(out _rigid);
            await UniTask.DelayFrame(4, cancellationToken: _source.Token);

            var trans = transform;

            trans.position = pos;
            _rigid.AddForce(dir, ForceMode2D.Impulse);
            Debug.Log($"Velocity: {_rigid.velocity} dir: {dir}");

            await UniTask.Delay(TimeSpan.FromSeconds(2), cancellationToken: _source.Token);
            gameObject.SetActive(false);
        }

        protected override void OnHitBullet(Collider2D col)
        {
            _source.Cancel();
            _source = new CancellationTokenSource();
            
            gameObject.SetActive(false);
            EventPublisher.Instance.PublishEvent(new AttackEvent
            {
                Amount = bulletData.Damage,
                AttackRange = 1,
                KnockBackPower = 0.5f,
                Source = transform,
                SourcePos = col.transform.position
            });
        }
    }
}