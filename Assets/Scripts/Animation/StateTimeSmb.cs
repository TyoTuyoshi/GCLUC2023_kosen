using UnityEngine;

namespace Animation
{
    public class StateTimeSmb : StateMachineBehaviour
    {
        private static readonly int AnimIdStateTime = Animator.StringToHash("StateTime");

        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo,
            int layerIndex)
        {
            animator.SetFloat(AnimIdStateTime, stateInfo.normalizedTime);
        }
    }
}