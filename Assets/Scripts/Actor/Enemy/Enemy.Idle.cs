using DG.Tweening;
using IceMilkTea.Core;

namespace Actor.Enemy
{
    public partial class Enemy
    {
        private class IdleState : ImtStateMachine<Enemy, EnemyState>.State
        {
            private Tween _tween;

            protected override void Enter()
            {
                _tween = DOVirtual.DelayedCall(0.5f, () => StateMachine.SendEvent(EnemyState.Move));
                Context._animator.SetFloat(AnimIdSpeed, 0);
                Context._animator.ResetTrigger(AnimIdDamageTrigger);
                Context._animator.ResetTrigger(AnimIdAttackTrigger);
            }

            protected override void Exit()
            {
                if (_tween.active) _tween.Kill();
                _tween = null;
            }
        }
    }
}