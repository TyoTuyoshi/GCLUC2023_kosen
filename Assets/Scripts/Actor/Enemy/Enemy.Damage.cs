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
            }

            protected override void Update()
            {
                if (Context._animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
                    StateMachine.SendEvent(EnemyState.Idle);
            }

            protected override void Exit()
            {
                Context._animator.ResetTrigger(AnimIdDamageTrigger);
            }
        }
    }
}