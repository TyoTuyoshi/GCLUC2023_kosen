using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Actor.Bullet
{
    /// <summary>
    ///     通常の弾丸
    /// </summary>
    [RequireComponent(typeof(Rigidbody2D))]
    public class NormalBullet : BulletBase
    {
        private readonly CancellationTokenSource _source = new();
        private (Vector3, Vector3) _event;
        private Rigidbody2D _rigid;

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
    }
}