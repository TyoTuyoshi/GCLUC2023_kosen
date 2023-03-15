using System;
using Cysharp.Threading.Tasks;
using Game;
using IceMilkTea.Core;

namespace Actor.Player
{
    public partial class Player
    {
        private class DeathState : ImtStateMachine<Player, PlayerState>.State
        {
            protected override void Enter()
            {
                GameManager.Instance.GoToResult();
                Destroy(Context.gameObject);
            }
        }
    }
}