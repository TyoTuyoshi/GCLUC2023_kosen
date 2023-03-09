using IceMilkTea.Core;

namespace Actor.Enemy
{
    public partial class Enemy
    {
        private class DeathState : ImtStateMachine<Enemy, EnemyState>.State
        {
            protected override void Enter()
            {
                // TODO: しっかりしたデスエフェクト
                Destroy(Context.gameObject);
            }
        }
    }
}