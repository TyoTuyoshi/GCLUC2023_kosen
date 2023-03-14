using System;
using IceMilkTea.Core;
using UnityEngine;

namespace Actor.Player
{
    public partial class Player
    {
        private static readonly int AnimIdGunAttack = Animator.StringToHash("GunAttack");

        private class GunAttackState : ImtStateMachine<Player, PlayerState>.State
        {
            protected override void Enter()
            {
                Context._animator.SetTrigger(AnimIdGunAttack);
            }

            protected override void Exit()
            {
                Context._animator.ResetTrigger(AnimIdGunAttack);
            }
        }
    }
}