using UnityEngine;

namespace Actor.Enemy
{
    /// <summary>
    ///     プレイヤーが近くにいれば攻撃を行う
    /// </summary>
    [RequireComponent(typeof(Animator))]
    public class EnemyAttacker : MonoBehaviour
    {
        private static readonly int AnimIdAttackRange = Animator.StringToHash("AttackRange");
        private static readonly int AnimIdAttackTrigger = Animator.StringToHash("AttackTrigger");
        [SerializeField] private GrowValue attackPower;
        [SerializeField] private float attackIntervalBase;
        private Animator _animator;
        private Enemy _enemy;
        private ActorBase _playerActor;
        private float _lastAttackTime;

        private void Start()
        {
            TryGetComponent(out _animator);
            TryGetComponent(out _enemy);

            GameObject.FindWithTag("Player").TryGetComponent(out _playerActor);
        }

        private void Update()
        {
            // 索敵して攻撃
            var time = Time.time - _lastAttackTime;
            if (time > attackIntervalBase && IsInRangePlayer())
            {
                _lastAttackTime = Time.time - Random.Range(0.3f, 0f);
                Attack();
            }
        }


        private void OnGUI()
        {
            if (GUILayout.Button("Attack")) Attack();
        }

        /// <summary>
        ///     攻撃
        /// </summary>
        private void Attack()
        {
            _animator.SetTrigger(AnimIdAttackTrigger);
        }

        /// <summary>
        ///     アニメーションイベントハンドラー
        /// </summary>
        private void HitAttack()
        {
            // 距離が行った攻撃に設定されている距離内なら
            if (IsInRangePlayer())
                _playerActor.PublishActorEvent(new DamageEvent
                {
                    Damage = attackPower.GetValue(_enemy.Level)
                });
        }

        /// <summary>
        /// プレイヤーとの距離が攻撃可能な距離か
        /// </summary>
        /// <returns></returns>
        private bool IsInRangePlayer()
        {
            var range = _animator.GetFloat(AnimIdAttackRange);
            var playerRangeSqr = (_playerActor.transform.position - transform.position).sqrMagnitude;
            
            return Mathf.Pow(range, 2) >= playerRangeSqr;
        }
    }
}