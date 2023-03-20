using System;
using Game;
using IceMilkTea.Core;
using UniRx;
using UnityEngine;

namespace Actor.Player
{
    public partial class Player
    {
        private static readonly int AnimIdDead = Animator.StringToHash("Dead");

        private class DeathState : ImtStateMachine<Player, PlayerState>.State
        {
            private IDisposable _disposable;

            protected override void Enter()
            {
                Context._animator.SetBool(AnimIdDead, true);

                _disposable = Context.OnAnimEvent
                    .Where(s => s == "OnDead")
                    .Subscribe(OnDeath);
            }

            protected override void Exit()
            {
                _disposable.Dispose();
                _disposable = null;
            }

            private void OnDeath(string _)
            {
                GameManager.Instance.GoToResult(true);
                Destroy(Context.gameObject);
            }
        }
    }
}