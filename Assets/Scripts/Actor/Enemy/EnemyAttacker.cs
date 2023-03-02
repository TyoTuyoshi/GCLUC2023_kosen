using System.Linq;
using DG.Tweening;
using UniRx;
using UnityEngine;

namespace Actor.Enemy
{
    [RequireComponent(typeof(Animator))]
    public class EnemyAttacker : MonoBehaviour
    {
        [SerializeField] private string[] comboAnimStates;
        [SerializeField] private GrowValue attackPower;
        [SerializeField] private float allowableComboTime, comboTransTime;
        private Animator _animator;
        private int[] _animStates;
        private int _comboCount;
        private Enemy _enemy;
        private bool _isAllowAttack;
        private static readonly int AnimIdAttackRange = Animator.StringToHash("AttackRange");

        private void Start()
        {
            TryGetComponent(out _animator);
            TryGetComponent(out _enemy);

            _enemy.OnActorEvent
                .Where(ev => ev.GetType() == typeof(DamageEvent))
                .Subscribe(_ => ResetCombo())
                .AddTo(this);

            _animStates = comboAnimStates.Select(Animator.StringToHash).ToArray();
        }

        private void ResetCombo()
        {
            _comboCount = 0;
            _isAllowAttack = false;
        }

        /// <summary>
        ///     攻撃
        /// </summary>
        public void Attack()
        {
            if (!_isAllowAttack) return;
            
            _comboCount++;
            _animator.CrossFade(_animStates[_comboCount], comboTransTime);
        }

        /// <summary>
        ///     次のコンボに移れることを示すアニメーションイベントハンドラー
        /// </summary>
        private void BeginComboTrans()
        {
            _isAllowAttack = true;
            DOVirtual
                .DelayedCall(allowableComboTime, ResetCombo)
                .SetLink(gameObject);
        }

        /// <summary>
        /// 攻撃がヒットする瞬間のアニメーションイベントハンドラー
        /// </summary>
        private void HitAttack()
        {
            var attackRange = _animator.GetFloat(AnimIdAttackRange);
            
            // TODO: 攻撃の処理
        }
    }
}