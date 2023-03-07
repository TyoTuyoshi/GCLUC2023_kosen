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
            if (CurrentHp <= 0) _enemy.PublishActorEvent(new DeathEvent());
        }

        private void Death(DeathEvent ev)
        {
            throw new NotImplementedException();
        }

        private void Start()
        {
            TryGetComponent(out _enemy);
            _enemy.OnActorEvent
                .Where(ev => ev is DamageEvent)
                .Select(ev => ev as DamageEvent)
                .Subscribe(TakeDamage)
                .AddTo(this);
            _enemy.OnActorEvent
                .Where(ev => ev is DeathEvent)
                .Select(ev => ev as DeathEvent)
                .Subscribe(Death)
                .AddTo(this);
            
            MaxHp = maxHp.GetValue(_enemy.Level);
            CurrentHp = MaxHp;
        }
    }
}