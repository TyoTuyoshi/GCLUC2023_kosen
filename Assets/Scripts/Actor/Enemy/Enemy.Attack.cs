using Event;
using IceMilkTea.Core;
using UniRx;
using UnityEngine;

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
            private readonly CompositeDisposable _disposable = new();

            private float _lastAttackTime;
            private float _attackRange => Context._animator.GetFloat(AnimIdAttackRange);

            protected override void Enter()
            {
                Context.OnAnimEvent
                    .Where(e => e == "HitAttack")
                    .Subscribe(HitAttack)
                    .AddTo(_disposable);
            }

            private void HitAttack(string _)
            {
                var dis =
                    (Context._playerActor.transform.position - Context.transform.position).sqrMagnitude;
                if (!IsInRangePlayer(dis)) return;

                EventPublisher.Instance.PublishEvent(new AttackEvent
                {
                    Amount = Context.attackPower.GetValue(Context.Level),
                    AttackRange = _attackRange,
                    KnockBackPower = 0.5f,
                    SourcePos = Context.transform.position
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
                if (IsInRangePlayer(dis)) // 攻撃範囲内なら
                {
                    if (time < Context.attackIntervalBase) return;

                    _lastAttackTime = Time.time - Random.Range(0.3f, 0f);
                    Attack();
                    return;
                }

                if (dis < Mathf.Pow(Context.playerSearchRange, 2)) // 索敵範囲内なら
                {
                    var pos = Context._rigid.position;
                    var pPos = (Vector2)Context._playerActor.transform.position;

                    // プレイヤーの位置が動ける範囲内ならプレイヤーの方向に動く
                    if ((Context._initPos - pPos).sqrMagnitude < Mathf.Pow(Context.moveRange, 2))
                    {
                        Context._rigid.MovePosition(pos + (pPos - pos) * (Context.moveSpeed * Time.deltaTime));
                        return;
                    }
                }

                Context.ChangeState(EnemyState.Idle);
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
                return Mathf.Pow(_attackRange, 2) >= distance;
            }
        }
    }
}