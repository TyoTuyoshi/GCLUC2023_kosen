using System;
using Event;
using IceMilkTea.Core;
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
        [SerializeField] private Vector3 physicalAttackOffset;

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
                var range = Context._animator.GetFloat(AnimIdAttackRange);
                var transform = Context.transform;
                EventPublisher.Instance.PublishEvent(new AttackEvent
                {
                    Amount = Context.physicalBasePower,
                    AttackRange = range,
                    KnockBackPower = Context.physicalKnockBack,
                    SourcePos = transform.position + Context.physicalAttackOffset + transform.forward.normalized * (range / 2),
                    Source = transform
                });
                StateMachine.SendEvent(PlayerState.Idle);
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