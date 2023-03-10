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

        private class AttackState : ImtStateMachine<Enemy, EnemyState>.State
        {
            private static readonly int AnimIdAttackRange = Animator.StringToHash("AttackRange");
            private static readonly int AnimIdAttackTrigger = Animator.StringToHash("AttackTrigger");
            private CompositeDisposable _disposable;
            
            private float _lastAttackTime;

            protected override void Enter()
            {
                Context.OnActorEvent
                    .Where(ev => ev is AnimationEvent { EventName: "HitAttack" })
                    .Select(ev => ev as AnimationEvent)
                    .Subscribe(HitAttack)
                    .AddTo(_disposable);
            }

            private void HitAttack(AnimationEvent ev)
            {
                var dis =
                    (Context._playerActor.transform.position - Context.transform.position).sqrMagnitude;
                if (!IsInRangePlayer(dis)) return;

                Context._playerActor.PublishActorEvent(new DamageEvent
                {
                    Damage = Context.attackPower.GetValue(Context.Level)
                });
            }

            protected override void Exit()
            {
                _disposable.Dispose();
            }

            private void LookAtPlayer()
            {
                var euler = Context.transform.rotation.eulerAngles;
                if (Context._rigid.position.x < Context._playerActor.transform.position.x)
                    euler.y = -180;
                else
                    euler.y = 0;

                Context.transform.rotation = Quaternion.Euler(euler);
            }

            protected override void Update()
            {
                var time = Time.time - _lastAttackTime;
                var dis =
                    (Context._playerActor.transform.position - Context.transform.position).sqrMagnitude;
                LookAtPlayer();
                if (IsInRangePlayer(dis)) // ?????????????????????
                {
                    if (time < Context.attackIntervalBase) return;

                    _lastAttackTime = Time.time - Random.Range(0.3f, 0f);
                    Attack();
                    return;
                }

                if (dis < Mathf.Pow(Context.playerSearchRange, 2)) // ?????????????????????
                {
                    var pos = Context._rigid.position;
                    var pPos = (Vector2)Context._playerActor.transform.position;

                    // ????????????????????????????????????????????????????????????????????????????????????
                    if ((Context._initPos - pPos).sqrMagnitude < Mathf.Pow(Context.moveRange, 2))
                    {
                        Context._rigid.MovePosition(pos + (pPos - pos) * (Context.moveSpeed * Time.deltaTime));
                        return;
                    }
                }

                StateMachine.SendEvent(EnemyState.Idle);
            }

            private void Attack()
            {
                Context._animator.SetTrigger(AnimIdAttackTrigger);
            }

            /// <summary>
            ///     ??????????????????????????????????????????????????????
            /// </summary>
            /// <returns></returns>
            private bool IsInRangePlayer(float distance)
            {
                var range = Context._animator.GetFloat(AnimIdAttackRange);
                return Mathf.Pow(range, 2) >= distance;
            }
        }
    }
}