using IceMilkTea.Core;
using UnityEngine;

namespace Actor.Enemy
{
    public partial class Enemy
    {
        [SerializeField] [Header("Move")] [Space]
        private float moveSpeed = 5;

        [SerializeField] [Tooltip("移動することができる範囲")]
        private float moveRange = 5f;

        [SerializeField] private float reachedRange = 1f;
        [SerializeField] private float playerSearchRange = 2f;
        [SerializeField] private LayerMask obstacleLayer;

        private class MoveState : ImtStateMachine<Enemy, EnemyState>.State
        {
            private Vector2 _destination;

            protected override void Enter()
            {
                _destination = SetDestination();
                Context._animator.SetFloat(AnimIdSpeed, 1);
            }

            protected override void Exit()
            {
                Context._animator.SetFloat(AnimIdSpeed, 0);
            }

            protected override void Update()
            {
                TransitionAttack();

                var pos = Context._rigid.position;
                // 到着したならアイドルに移行
                if ((_destination - pos).sqrMagnitude < Mathf.Pow(Context.reachedRange, 2))
                    StateMachine.SendEvent(EnemyState.Idle);

                var dir = _destination - pos;
                Context._rigid.MovePosition(pos + dir.normalized * (Context.moveSpeed * Time.deltaTime));

                // 移動方向を向く
                var euler = Context.transform.rotation.eulerAngles;
                euler.y = Context._rigid.position.x < dir.x ? -180 : 0;
                Context.transform.rotation = Quaternion.Euler(euler);
            }

            /// <summary>
            ///     攻撃状態に移行するか判断
            /// </summary>
            private void TransitionAttack()
            {
                var dis = (Context.transform.position - Context._playerActor.transform.position).sqrMagnitude;
                if (dis < Mathf.Pow(Context.playerSearchRange, 2)) StateMachine.SendEvent(EnemyState.Attack);
            }

            private bool CanReach(in Vector2 target)
            {
                var pos = Context.transform.position;
                return Physics2D.LinecastNonAlloc(pos, target, new RaycastHit2D[1], Context.obstacleLayer) == 0;
            }

            private Vector2 SetDestination()
            {
                var range = Context.moveRange;
                var randomPos = Context._initPos + new Vector2(Random.Range(-range, range), Random.Range(-range, range));
                if (!CanReach(randomPos)) randomPos = Context._initPos;

                return randomPos;
            }
        }
    }
}