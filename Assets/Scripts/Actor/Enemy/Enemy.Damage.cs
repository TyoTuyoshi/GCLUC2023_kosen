using IceMilkTea.Core;

namespace Actor.Enemy
{
    public partial class Enemy
    {
        private class DamageState : ImtStateMachine<Enemy, EnemyState>.State
        {
            protected override void Enter()
            {
                Context._animator.SetTrigger(AnimIdDamageTrigger);
                StateMachine.SendEvent(EnemyState.Idle);
            }
        }
    }
}