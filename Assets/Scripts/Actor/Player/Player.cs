using System;
using IceMilkTea.Core;
using UnityEngine;

namespace Actor.Player
{
    public partial class Player : ActorBase
    {
        private GameInput.PlayerActions _input;
        private Rigidbody2D _rigid;
        private ImtStateMachine<Player, PlayerState> _stateMachine;
        public override int Level => 1;

        private void Start()
        {
            _input = new GameInput().Player;
            _input.Enable();

            TryGetComponent(out _rigid);

            InitStateMachine();
        }

        private void FixedUpdate()
        {
            _stateMachine.Update();
        }

        private void InitStateMachine()
        {
            _stateMachine = new ImtStateMachine<Player, PlayerState>(this);

            _stateMachine.AddTransition<IdleState, MoveState>(PlayerState.Move);

            _stateMachine.SetStartState<MoveState>();
        }
    }
}