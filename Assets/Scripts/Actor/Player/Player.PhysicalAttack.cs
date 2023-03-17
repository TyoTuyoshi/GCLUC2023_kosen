using System;
using AutoGenerate;
using Cinemachine;
using Cysharp.Threading.Tasks;
using Event;
using IceMilkTea.Core;
using Particle;
using UniRx;
using UnityEngine;

namespace Actor.Player
{
    public partial class Player
    {
        private static readonly int AnimIdAttackTrigger = Animator.StringToHash("AttackTrigger");
        private static readonly int AnimIdAttackRange = Animator.StringToHash("AttackRange");

        [Header("PhysicalAttack")] [Space] [SerializeField]
        private float physicalBasePower = 2f;

        [SerializeField] private float physicalKnockBack = 0.2f;
        [SerializeField] private Transform physicalAttackVfxPos, physicalAttackOrigin;

        /// <summary>
        ///     物理攻撃を行うステート、パンチ等
        /// </summary>
        private class PhysicalAttackState : ImtStateMachine<Player, PlayerState>.State
        {
            private IDisposable _disposable;

            protected override void Enter()
            {
                _disposable = Context.OnAnimEvent
                    .Where(e => e == "HitAttack")
                    .Subscribe(OnHit);
                Context._animator.SetTrigger(AnimIdAttackTrigger);
            }

            private void OnHit(string _)
            {
                var transform = Context.transform;

                EventPublisher.Instance.PublishEvent(new AttackEvent
                {
                    Amount = Context.physicalBasePower,
                    AttackRange = Context._animator.GetFloat(AnimIdAttackRange),
                    KnockBackPower = Context.physicalKnockBack,
                    SourcePos = Context.physicalAttackOrigin.position,
                    Source = transform
                });
                StateMachine.SendEvent(PlayerState.Idle);

                var forward = transform.forward;
                ParticleManager.Instance.PlayVfx(VfxEnum.Punch1, 1, Context.physicalAttackVfxPos.position,
                    Quaternion.Euler(0, forward.x < 0 ? 0 : 180, 0));
                
                Context._impulseSource.GenerateImpulse();                
            }

            protected override void Exit()
            {
                _disposable.Dispose();
            }

            protected override void Update()
            {
                if (!Context._animator.IsInTransition(0) &&
                    Context._animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
                    StateMachine.SendEvent(PlayerState.Idle);
            }
        }
    }
}