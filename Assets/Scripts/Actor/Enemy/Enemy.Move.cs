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
            }

            protected override void Update()
            {
                var pos = Context._rigid.position;
                // 到着したならアイドルに移行
                if ((_destination - pos).sqrMagnitude < Mathf.Pow(Context.reachedRange, 2))
                    StateMachine.SendEvent(EnemyState.Idle);

                var dir = _destination - pos;
                Context._rigid.MovePosition(pos + dir.normalized * (Context.moveSpeed * Time.deltaTime));
                
                TransitionAttack();
            }

            /// <summary>
            /// 攻撃状態に移行するか判断
            /// </summary>
            private void TransitionAttack()
            {
                var dis = (Context.transform.position - Context._playerActor.transform.position).sqrMagnitude;
                if (dis < Mathf.Pow(Context.playerSearchRange, 2))
                {
                    StateMachine.SendEvent(EnemyState.Attack);
                }   
            }

            private bool IsReach(in Vector2 target)
            {
                var pos = Context.transform.position;
                return Physics2D.LinecastNonAlloc(pos, target, new RaycastHit2D[1], Context.obstacleLayer) == 0;
            }

            private Vector2 SetDestination()
            {
                var range = Context.moveRange;
                var randomPos = Vector2.zero;
                while (randomPos.sqrMagnitude == 0 || !IsReach(randomPos))
                    randomPos = Context._initPos + new Vector2(Random.Range(-range, range), Random.Range(-range, range));

                return randomPos;
            }
        }
    }
}