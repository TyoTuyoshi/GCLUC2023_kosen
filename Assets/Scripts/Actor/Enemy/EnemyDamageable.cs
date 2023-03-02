using System;
using UnityEngine;

namespace Actor.Enemy
{
    public class EnemyDamageable : MonoBehaviour, IDamageableActor
    {
        [SerializeField] private GrowValue maxHp;

        public float MaxHp { get; private set; }
        public float CurrentHp { get; private set; }
        
        public void TakeDamage(float damage)
        {
            CurrentHp = Mathf.Clamp(CurrentHp - damage, 0, maxHp.GetValue(1));
            if (CurrentHp <= 0) Death();
        }

        private void Death()
        {
            throw new NotImplementedException();
        }
    }
}