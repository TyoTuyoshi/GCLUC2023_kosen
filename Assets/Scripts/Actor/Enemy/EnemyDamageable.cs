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
                .Where(ev => ev is DamageEvent)
                .Select(ev => ev as DamageEvent)
                .Subscribe(OnDamage)
                .AddTo(this);
            _enemy.OnActorEvent
                .Where(ev => ev is DeathEvent)
                .Select(ev => ev as DeathEvent)
                .Subscribe(Death)
                .AddTo(this);

            MaxHp = maxHp.GetValue(_enemy.Level);
            CurrentHp = MaxHp;
        }

        public float MaxHp { get; private set; }
        public float CurrentHp { get; private set; }

        private void OnDamage(DamageEvent ev)
        {
            CurrentHp = Mathf.Clamp(CurrentHp - ev.Damage, 0, maxHp.GetValue(_enemy.Level));

            // ノックバック
            _rigid.AddForce(ev.KnockBackDir, ForceMode2D.Impulse);
            // 一定時間後にリセット
            DOVirtual.DelayedCall(1f, () => _rigid.velocity = Vector2.zero).SetLink(gameObject);

            if (CurrentHp <= 0) _enemy.PublishActorEvent(new DeathEvent());
        }

        private void Death(DeathEvent ev)
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