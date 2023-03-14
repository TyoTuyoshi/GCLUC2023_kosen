using DG.Tweening;
using Event;
using UniRx;
using UnityEngine;

namespace Actor.Player
{
    public partial class Player
    {
        [SerializeField] private GrowValue maxHp;

        public float MaxHp => maxHp.GetValue(Level);
        public float CurrentHp { get; private set; }

        private void InitDamageable()
        {
            CurrentHp = MaxHp;
            AttackEvent
                .RegisterListenerInRange(transform)
                .Subscribe(OnDamage)
                .AddTo(this);
        }

        private void OnDamage(AttackEvent e)
        {
            CurrentHp = Mathf.Clamp(CurrentHp - e.Amount, 0, MaxHp);

            // ノックバック
            var dir = (_rigid.position - (Vector2)e.SourcePos).normalized * e.KnockBackPower;
            _rigid.AddForce(dir, ForceMode2D.Impulse);
            // 一定時間後にリセット
            DOVirtual.DelayedCall(1f, () => _rigid.velocity = Vector2.zero).SetLink(gameObject);

            _stateMachine.SendEvent(CurrentHp <= 0 ? PlayerState.Death : PlayerState.Damage);
        }
    }
}