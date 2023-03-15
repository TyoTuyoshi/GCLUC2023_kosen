using IceMilkTea.Core;
using UnityEngine;

namespace Actor.Player
{
    public partial class Player
    {
        private static readonly int AnimIdDamageTrigger = Animator.StringToHash("DamageTrigger");

        private class DamageState : ImtStateMachine<Player, PlayerState>.State
        {
            protected override void Enter()
            {
                Context._animator.SetTrigger(AnimIdDamageTrigger);
            }

            protected override void Update()
            {
                if (Context._animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
                    StateMachine.SendEvent(PlayerState.Idle);
            }

            protected override void Exit()
            {
                Context._animator.ResetTrigger(AnimIdDamageTrigger);
            }
        }
    }
}