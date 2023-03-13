using System.Linq;
using IceMilkTea.Core;
using UnityEngine;
using Utils;

namespace Actor.Player
{
    public partial class Player : ActorBase
    {
        [SerializeField] private Pair<PlayerState, string>[] animStates;
        private Animator _animator;
        private GameInput.PlayerActions _input;
        private Rigidbody2D _rigid;
        private ImtStateMachine<Player, PlayerState> _stateMachine;
        public override int Level => 1;

        private void Start()
        {
            _input = new GameInput().Player;
            _input.Enable();

            TryGetComponent(out _rigid);
            TryGetComponent(out _animator);

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
            _stateMachine.AddTransition<MoveState, IdleState>(PlayerState.Idle);

            _stateMachine.SetStartState<MoveState>();
        }

        private void ChangeState(PlayerState state)
        {
            _stateMachine.SendEvent(state);

            if (animStates.FirstOrDefault(v => v.First == state) is not { } animState)
            {
#if UNITY_EDITOR
                Debug.LogWarning($"`{state}` に対応するアニメーションステートが登録されていません。", gameObject);
#endif
                return;
            }

            _animator.CrossFade(animState.Second, 0.2f);
        }
    }
}