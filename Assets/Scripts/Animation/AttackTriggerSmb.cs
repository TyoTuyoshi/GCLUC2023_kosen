using UnityEngine;

namespace Animation
{
    public class AttackTriggerSmb : StateMachineBehaviour
    {
        private static readonly int AnimIdAttackTrigger = Animator.StringToHash("AttackTrigger");

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo,
            int layerIndex)
        {
            animator.ResetTrigger(AnimIdAttackTrigger);
        }
    }
}