using System;
using UniRx;
using UnityEngine;

namespace Actor.Enemy
{
    public class EnemyDamageable : MonoBehaviour, IDamageableActor
    {
        [SerializeField] private GrowValue maxHp;
        private Enemy _enemy;

        public float MaxHp { get; private set; }
        public float CurrentHp { get; private set; }
        
        private void TakeDamage(DamageEvent ev)
        {
            CurrentHp = Mathf.Clamp(CurrentHp - ev.Damage, 0, maxHp.GetValue(_enemy.Level));
            if (CurrentHp <= 0) Death();
        }

        private void Death()
        {
            throw new NotImplementedException();
        }

        private void Start()
        {
            TryGetComponent(out _enemy);
            _enemy.OnActorEvent
                .Where(ev => ev.GetType() == typeof(DamageEvent))
                .Subscribe(ev => TakeDamage(ev as DamageEvent))
                .AddTo(this);
            
            MaxHp = maxHp.GetValue(_enemy.Level);
        }
    }
}