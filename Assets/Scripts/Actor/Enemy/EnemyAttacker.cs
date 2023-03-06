using UnityEngine;

namespace Actor.Enemy
{
    /// <summary>
    ///     プレイヤーが近くにいれば攻撃を行う
    /// </summary>
    [RequireComponent(typeof(Animator))]
    public class EnemyAttacker : MonoBehaviour
    {
        private Transform _player;
        
        private static readonly int AnimIdAttackRange = Animator.StringToHash("AttackRange");
        private static readonly int AnimIdAttackTrigger = Animator.StringToHash("AttackTrigger");
        [SerializeField] private GrowValue attackPower;
        private Animator _animator;
        private Enemy _enemy;

        private void Start()
        {
            TryGetComponent(out _animator);
            TryGetComponent(out _enemy);
        }

        private void OnGUI()
        {
            if (GUILayout.Button("Attack")) Attack();
            GUILayout.Label("");
        }

        /// <summary>
        ///     攻撃
        /// </summary>
        private void Attack()
        {
            _animator.SetTrigger(AnimIdAttackTrigger);
        }

        private void HitAttack()
        {
            _player ??= GameObject.FindWithTag("Player").transform;
            
            var range = _animator.GetFloat(AnimIdAttackRange);
            var playerRangeSqr = (_player.position - transform.position).sqrMagnitude;
            if (Mathf.Pow(range, 2) <= playerRangeSqr)
            {
                
            }
        }
    }
}