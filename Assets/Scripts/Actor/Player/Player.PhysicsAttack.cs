using IceMilkTea.Core;
using UnityEngine;

namespace Actor.Player
{
    public partial class Player
    {
        [Header("PhysicsAttack"), Space, SerializeField]
        private float physicsAttackBaseDamage = 2f;

        private class PhysicsAttackState: ImtStateMachine<Player, PlayerAttackState>.State
        {
            protected override void Update()
            {
                
            }

            private void Attack()
            {
                
            }
        }
    }
}