using UnityEngine;

namespace Animation
{
    public class ResetTriggerSmb : StateMachineBehaviour
    {
        [SerializeField] private string trigger;
        private int _animIdTrigger;
        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo,
            int layerIndex)
        {
            if (_animIdTrigger == 0) _animIdTrigger = Animator.StringToHash(trigger);

            animator.ResetTrigger(_animIdTrigger);
        }
    }
}