using System;
using IceMilkTea.Core;
using UniRx;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Actor.Enemy
{
    public partial class Enemy
    {
        [Space] [Header("Attack")] [SerializeField]
        private GrowValue attackPower;

        [SerializeField] private float attackIntervalBase;

        private class AttackState : ImtStateMachine<Enemy, int>.State
        {
            private static readonly int AnimIdAttackRange = Animator.StringToHash("AttackRange");
            private static readonly int AnimIdAttackTrigger = Animator.StringToHash("AttackTrigger");
            private IDisposable _animEvent;
            private float _lastAttackTime;

            protected override void Enter()
            {
                _animEvent = Context.OnActorEvent
                    .Where(ev => ev is AnimationEvent { EventName: "HitAttack" })
                    .Select(ev => ev as AnimationEvent)
                    .Subscribe(HitAttack);
            }

            private void HitAttack(AnimationEvent ev)
            {
                if (!IsInRangePlayer()) return;

                Context._playerActor.PublishActorEvent(new DamageEvent
                {
                    Damage = Context.attackPower.GetValue(Context.Level)
                });
            }

            protected override void Exit()
            {
                _animEvent.Dispose();
            }

            protected override void Update()
            {
                var time = Time.time - _lastAttackTime;
                if (time > Context.attackIntervalBase && IsInRangePlayer())
                {
                    _lastAttackTime = Time.time - Random.Range(0.3f, 0f);
                    Attack();
                }
            }

            private void Attack()
            {
                Context._animator.SetTrigger(AnimIdAttackRange);
            }

            /// <summary>
            ///     プレイヤーとの距離が攻撃可能な距離か
            /// </summary>
            /// <returns></returns>
            private bool IsInRangePlayer()
            {
                var range = Context._animator.GetFloat(AnimIdAttackRange);
                var playerRangeSqr =
                    (Context._playerActor.transform.position - Context.transform.position).sqrMagnitude;

                return Mathf.Pow(range, 2) >= playerRangeSqr;
            }
        }
    }
}