using IceMilkTea.Core;
using UnityEngine;

namespace Actor.Player
{
    public partial class Player
    {
        private static readonly int AnimIdSpeed = Animator.StringToHash("Speed");

        [SerializeField] [Header("Move")] [Space]
        private float moveSpeed = 4f;

        [SerializeField] private float airMoveSpeed = 2f;
        [SerializeField] private float jumpPower = 4f;
        [SerializeField] private float gravity = 6f;

        private class MoveState : ImtStateMachine<Player, PlayerState>.State
        {
            private bool _isJumping;
            private float _jumpBeginY;
            private float _jumpDelta;

            protected override void Update()
            {
                var pos = Context._rigid.position;
                pos = Move(pos);
                pos = Jump(pos);

                // 動きがないならIdleに遷移
                if (pos == Context._rigid.position)
                    StateMachine.SendEvent(PlayerState.Idle);
                else
                    Context._rigid.MovePosition(pos);
            }

            protected override void Exit()
            {
                Context._animator.SetFloat(AnimIdSpeed, 0);
            }

            private Vector2 Move(Vector2 current)
            {
                var value = Context.Input.Move.ReadValue<Vector2>();
                Context._animator.SetFloat(AnimIdSpeed, value.sqrMagnitude > 0.1f ? 1 : 0);
                if (value.sqrMagnitude < 0.1f) return current;

                // ジャンプしているなら横移動のみに制限
                if (_isJumping) value.y = 0;
                var speed = _isJumping ? Context.airMoveSpeed : Context.moveSpeed;

                // 動いた方向に回転
                var euler = Context.transform.rotation.eulerAngles;
                euler.y = value.x > 0.1f ? 180 : value.x < -0.1f ? 0 : euler.y;
                Context.transform.rotation = Quaternion.Euler(euler);

                return current + value.normalized * (speed * Time.fixedDeltaTime);
            }

            private Vector2 Jump(Vector2 current)
            {
                if (!_isJumping && Context.Input.Jump.ReadValue<float>() > 0.1f)
                {
                    _jumpDelta = 0;
                    _isJumping = true;
                    _jumpBeginY = current.y;
                }

                if (!_isJumping) return current;

                var jumpY = Context.jumpPower * _jumpDelta + 0.5f * Context.gravity * Mathf.Pow(_jumpDelta, 2);
                var jumpPos = new Vector2(current.x, _jumpBeginY + jumpY);

                _jumpDelta += Time.fixedDeltaTime;
                _isJumping = jumpPos.y >= _jumpBeginY;

                return jumpPos;
            }
        }
    }
}