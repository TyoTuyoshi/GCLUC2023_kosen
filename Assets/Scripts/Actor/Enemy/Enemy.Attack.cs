using System;
using AutoGenerate;
using Event;
using IceMilkTea.Core;
using Particle;
using UniRx;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Actor.Enemy
{
    public partial class Enemy
    {
        private static readonly int AnimIdSpeed = Animator.StringToHash("Speed");

        private static readonly int AnimIdAttackRange = Animator.StringToHash("AttackRange");
        private static readonly int AnimIdAttackTrigger = Animator.StringToHash("AttackTrigger");

        [Space] [Header("Attack")] [SerializeField]
        private GrowValue attackPower;

        [SerializeField] private Transform attackVfxPos, attackOrigin;

        [SerializeField] private float attackIntervalBase;

#if UNITY_EDITOR
        private void AttackStateGizmos()
        {
            if (!Application.isPlaying) return;

            Gizmos.color = Color.red;

            var range = _animator.GetFloat(AnimIdAttackRange);
            var trans = transform;
            Gizmos.DrawWireSphere(attackOrigin.position, range);
        }
#endif

        private class AttackState : ImtStateMachine<Enemy, EnemyState>.State
        {
            private IDisposable _disposable;

            private float _lastAttackTime;
            private float AttackRange => Context._animator.GetFloat(AnimIdAttackRange);

            protected override void Enter()
            {
                _disposable = Context.OnAnimEvent
                    .Where(e => e == "HitAttack")
                    .Subscribe(HitAttack);
            }

            private void HitAttack(string _)
            {
                var transform = Context.transform;
                var dis =
                    (Context._playerActor.transform.position - transform.position).sqrMagnitude;
                if (!IsInRangePlayer(dis)) return;

                var forward = transform.forward;
                EventPublisher.Instance.PublishEvent(new AttackEvent
                {
                    Amount = Context.attackPower.GetValue(Context.Level),
                    AttackRange = AttackRange,
                    KnockBackPower = 0.5f,
                    SourcePos = Context.attackOrigin.position,
                    Source = transform
                });
                ParticleManager.Instance.PlayVfx(VfxEnum.Punch1, 1, Context.attackVfxPos.position,
                    Quaternion.Euler(0, forward.x < 0 ? 0 : 180, 0));
            }

            protected override void Exit()
            {
                _disposable.Dispose();
                _disposable = null;
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
                if (IsInRangePlayer(dis)) // 攻撃範囲内なら
                {
                    if (time < Context.attackIntervalBase) return;

                    _lastAttackTime = Time.time - Random.Range(0.3f, 0f);
                    Attack();
                    // return;
                }

                if (dis < Mathf.Pow(Context.playerSearchRange, 2)) // 索敵範囲内なら
                {
                    var pos = Context._rigid.position;
                    var pPos = (Vector2)Context._playerActor.transform.position;

                    // プレイヤーの位置が動ける範囲内ならプレイヤーの方向に動く
                    if ((Context._initPos - pPos).sqrMagnitude < Mathf.Pow(Context.moveRange, 2))
                    {
                        Context._rigid.MovePosition(pos + (pPos - pos) * (Context.moveSpeed * Time.deltaTime));
                        Context._animator.SetFloat(AnimIdSpeed, 1);
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
            ///     プレイヤーとの距離が攻撃可能な距離か
            /// </summary>
            /// <returns></returns>
            private bool IsInRangePlayer(float distance)
            {
                return Mathf.Pow(AttackRange, 2) >= distance;
            }
        }
    }
}