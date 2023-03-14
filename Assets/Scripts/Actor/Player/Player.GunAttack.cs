    using System;
    using IceMilkTea.Core;
    using UnityEngine;

    namespace Actor.Player
    {
        public partial class Player
        {
            [SerializeField, Header("GunAttack"), Space]
            private GameObject gunWeapon;
            
            private static readonly int AnimIdGunAttack = Animator.StringToHash("GunAttack");

            private class GunAttackState : ImtStateMachine<Player, PlayerState>.State
            {
                protected override void Enter()
                {
                    // TODO: 銃の登場演出
                    Context.gunWeapon.SetActive(true);
                    
                    Context._animator.SetTrigger(AnimIdGunAttack);
                }

                protected override void Exit()
                {
                    // TODO: 銃の登場演出
                    Context.gunWeapon.SetActive(false);
                    
                    Context._animator.ResetTrigger(AnimIdGunAttack);
                }
            }
        }
    }