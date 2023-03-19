using System;
using AutoGenerate;
using IceMilkTea.Core;
using Particle;
using UniRx;
using UnityEngine;

namespace Actor.Enemy
{
    public partial class Enemy
    {
        private static readonly int AnimIdDead = Animator.StringToHash("Dead");

        private class DeathState : ImtStateMachine<Enemy, EnemyState>.State
        {
            private IDisposable _disposable;

            protected override void Enter()
            {
                Context._animator.SetBool(AnimIdDead, true);
                
                _disposable = Context.OnAnimEvent
                    .Where(s => s == "OnDead")
                    .Subscribe(OnDeath);
            }

            protected override void Exit()
            {
                _disposable.Dispose();
                _disposable = null;
            }

            private void OnDeath(string _)
            {
                Context.TryGetComponent(out EnemyLootTable lootTable);
                lootTable.OnDeath();

                Destroy(Context.gameObject);

                var pos = Context.transform.position;
                ParticleManager.Instance.PlayVfx(VfxEnum.Death, 1.5f, pos + new Vector3(0, 0.8f));
            }
        }
    }
}