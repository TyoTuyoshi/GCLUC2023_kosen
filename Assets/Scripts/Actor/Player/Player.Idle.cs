using IceMilkTea.Core;

namespace Actor.Player
{
    public partial class Player
    {
        private class IdleState : ImtStateMachine<Player, PlayerState>.State
        {
            protected override void Update()
            {
                if (Context._input.Move.IsPressed() || Context._input.Jump.IsPressed())
                {
                    Context.ChangeState(PlayerState.Move);
                }
            }
        }
    }
}