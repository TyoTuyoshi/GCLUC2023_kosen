using DG.Tweening;
using UniRx;
using UnityEditor;
using UnityEngine;

namespace Actor.Enemy
{
    public class EnemyDamageable : MonoBehaviour, IDamageableActor
    {
        [SerializeField] private GrowValue maxHp;
        private Enemy _enemy;
        private Rigidbody2D _rigid;

        private void Start()
        {
            TryGetComponent(out _enemy);
            TryGetComponent(out _rigid);

            _enemy.OnActorEvent
                .Where(e => e is DamageEvent)
                .Select(e => e as DamageEvent)
                .Subscribe(OnDamage)
                .AddTo(this);
            _enemy.OnActorEvent
                .Where(e => e is DeathEvent)
                .Select(e => e as DeathEvent)
                .Subscribe(Death)
                .AddTo(this);
            _enemy.OnActorEvent
                .Where(e => e is HealEvent)
                .Select(e => e as HealEvent)
                .Subscribe(OnHeal)
                .AddTo(this);

            MaxHp = maxHp.GetValue(_enemy.Level);
            CurrentHp = MaxHp;
        }

        public float MaxHp { get; private set; }
        public float CurrentHp { get; private set; }

        private void OnHeal(HealEvent e)
        {
            CurrentHp = Mathf.Clamp(CurrentHp + e.Amount, 0, maxHp.GetValue(_enemy.Level));
        }

        private void OnDamage(DamageEvent e)
        {
            CurrentHp = Mathf.Clamp(CurrentHp - e.Damage, 0, maxHp.GetValue(_enemy.Level));

            // ノックバック
            _rigid.AddForce(e.KnockBackDir, ForceMode2D.Impulse);
            // 一定時間後にリセット
            DOVirtual.DelayedCall(1f, () => _rigid.velocity = Vector2.zero).SetLink(gameObject);

            if (CurrentHp <= 0) _enemy.PublishActorEvent(new DeathEvent());
        }

        private void Death(DeathEvent e)
        {
            Debug.Log("Death", gameObject);
        }
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(EnemyDamageable))]
    public class EnemyDamageableEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (!Application.isPlaying) return;

            var damageable = target as EnemyDamageable;
            GUILayout.Label($"Current HP: {damageable!.CurrentHp}");
        }
    }
#endif
}