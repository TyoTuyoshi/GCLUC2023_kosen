using DG.Tweening;
using Event;
using UniRx;
using UnityEngine;

namespace Actor.Enemy
{
    public partial class Enemy
    {
        [SerializeField] private GrowValue maxHp;

        public float MaxHp { get; private set; }
        public float CurrentHp { get; private set; }

        // private partial void Start()
        // {
        //     // _enemy.OnActorEvent
        //     //     .Where(e => e is HealEvent)
        //     //     .Select(e => e as HealEvent)
        //     //     .Subscribe(OnHeal)
        //     //     .AddTo(this);
        // }

        private void InitDamageable()
        {
            MaxHp = maxHp.GetValue(Level);
            CurrentHp = MaxHp;

            AttackEvent.RegisterListenerInRange(transform).Subscribe(OnDamage);
        }

        private void OnHeal(HealEvent e)
        {
            CurrentHp = Mathf.Clamp(CurrentHp + e.Amount, 0, maxHp.GetValue(Level));
        }

        private void OnDamage(AttackEvent e)
        {
            CurrentHp = Mathf.Clamp(CurrentHp - e.Amount, 0, maxHp.GetValue(Level));

            // ノックバック
            var dir = (_rigid.position - (Vector2)e.SourcePos).normalized * e.KnockBackPower;
            _rigid.AddForce(dir, ForceMode2D.Impulse);
            // 一定時間後にリセット
            DOVirtual.DelayedCall(1f, () => _rigid.velocity = Vector2.zero).SetLink(gameObject);

            if (CurrentHp <= 0) ChangeState(EnemyState.Death);
        }
    }
}