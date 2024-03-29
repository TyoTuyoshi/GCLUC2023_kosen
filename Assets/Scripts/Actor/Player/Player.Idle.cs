using IceMilkTea.Core;

namespace Actor.Player
{
    public partial class Player
    {
        private class IdleState : ImtStateMachine<Player, PlayerState>.State
        {
            protected override void Enter()
            {
                Context._animator.SetFloat(AnimIdSpeed, 0);
                Context._animator.ResetTrigger(AnimIdAttackTrigger);
            }

            protected override void Update()
            {
                if (Context.Input.Move.IsPressed() || Context.Input.Jump.IsPressed())
                    StateMachine.SendEvent(PlayerState.Move);
            }
        }
    }
}