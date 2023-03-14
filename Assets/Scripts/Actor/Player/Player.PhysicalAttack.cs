using System;
using AutoGenerate;
using Event;
using IceMilkTea.Core;
using Particle;
using UniRx;
using UnityEngine;
using UnityEngine.Serialization;

namespace Actor.Player
{
    public partial class Player
    {
        private static readonly int AnimIdAttackTrigger = Animator.StringToHash("AttackTrigger");
        private static readonly int AnimIdAttackRange = Animator.StringToHash("AttackRange");

        [Header("PhysicalAttack")] [Space] [SerializeField]
        private float physicalBasePower = 2f;

        [SerializeField] private float physicalKnockBack = 0.2f;
        [FormerlySerializedAs("physicalAttackOffset")] [SerializeField] private Vector3 physicalAttackVfxOffset;

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

            private async void OnHit(string _)
            {
                var range = Context._animator.GetFloat(AnimIdAttackRange);
                var transform = Context.transform;

                var pos = transform.position;
                var forward = transform.forward;
                EventPublisher.Instance.PublishEvent(new AttackEvent
                {
                    Amount = Context.physicalBasePower,
                    AttackRange = range,
                    KnockBackPower = Context.physicalKnockBack,
                    SourcePos = pos + forward.normalized * (range / 2),
                    Source = transform
                });
                StateMachine.SendEvent(PlayerState.Idle);

                var offset = Context.physicalAttackVfxOffset;
                var vfxPos = pos + new Vector3(offset.x * (forward.x < 0 ? -1 : 1), offset.y);
                await ParticleManager.Instance.PlayVfx(VfxEnum.Punch1, 1, vfxPos, Quaternion.Euler(0, forward.x < 0 ? 0 : 180, 0));
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