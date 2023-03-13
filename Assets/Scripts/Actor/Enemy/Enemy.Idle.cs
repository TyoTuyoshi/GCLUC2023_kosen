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
                _tween = DOVirtual.DelayedCall(1, () => Context.ChangeState(EnemyState.Move));
            }

            protected override void Exit()
            {
                if (_tween.active) _tween.Kill();
                _tween = null;
            }
        }
    }
}