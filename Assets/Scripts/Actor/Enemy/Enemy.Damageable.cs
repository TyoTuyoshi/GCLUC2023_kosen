using DG.Tweening;
using Event;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Actor.Enemy
{
    public partial class Enemy
    {
        private static readonly int AnimIdDamageTrigger = Animator.StringToHash("DamageTrigger");

        [SerializeField] [Header("Damageable")] [Space]
        private GrowValue maxHp;

        [SerializeField] private Slider hpBar;

        public float MaxHp => maxHp.GetValue(Level);
        public float CurrentHp { get; private set; }

        private void InitDamageable()
        {
            CurrentHp = MaxHp;

            AttackEvent
                .RegisterListenerInRange(origin)
                .Subscribe(OnDamage)
                .AddTo(this);
        }

        private void OnHeal(HealEvent e)
        {
            CurrentHp = Mathf.Clamp(CurrentHp + e.Amount, 0, MaxHp);
            hpBar.value = CurrentHp / MaxHp;
        }

        private void OnDamage(AttackEvent e)
        {
            CurrentHp = Mathf.Clamp(CurrentHp - e.Amount, 0, MaxHp);
            hpBar.value = CurrentHp / MaxHp;

            // ノックバック
            // var dir = ((Vector2)origin.position - (Vector2)e.SourcePos).normalized * e.KnockBackPower;
            var dir = Vector2.zero;
            dir.x = _rigid.position.x < e.SourcePos.x ? e.KnockBackPower : -e.KnockBackPower;

            _rigid.AddForce(dir, ForceMode2D.Impulse);
            // 一定時間後にリセット
            DOVirtual.DelayedCall(1f, () => _rigid.velocity = Vector2.zero).SetLink(gameObject);

            _stateMachine.SendEvent(CurrentHp <= 0 ? EnemyState.Death : EnemyState.Damage);
        }
    }
}