using IceMilkTea.Core;
using UnityEditor;
using UnityEngine;

namespace Actor.Player
{
    public partial class Player : ActorBase, IDamageableActor
    {
        private Animator _animator;
        private GameInput.PlayerActions _input;
        private Rigidbody2D _rigid;
        private ImtStateMachine<Player, PlayerState> _stateMachine;
        public override int Level => 1;
#if UNITY_EDITOR
        public string CurrentState => _stateMachine.CurrentStateName;
#endif

        private void Start()
        {
            _input = new GameInput().Player;
            _input.Enable();

            TryGetComponent(out _rigid);
            TryGetComponent(out _animator);

            InitStateMachine();
            InitDamageable();
        }

        private void FixedUpdate()
        {
            _stateMachine.Update();

            if (_input.Attack.IsPressed()) _stateMachine.SendEvent(PlayerState.GunAttack);
        }

        private void InitStateMachine()
        {
            _stateMachine = new ImtStateMachine<Player, PlayerState>(this);

            _stateMachine.AddTransition<IdleState, MoveState>(PlayerState.Move);
            _stateMachine.AddTransition<MoveState, IdleState>(PlayerState.Idle);

            _stateMachine.AddAnyTransition<DamageState>(PlayerState.Damage);
            _stateMachine.AddTransition<DamageState, IdleState>(PlayerState.Idle);

            _stateMachine.AddTransition<IdleState, GunAttackState>(PlayerState.GunAttack);
            _stateMachine.AddTransition<MoveState, GunAttackState>(PlayerState.GunAttack);
            _stateMachine.AddTransition<GunAttackState, IdleState>(PlayerState.Idle);

            _stateMachine.AddTransition<IdleState, PhysicalAttackState>(PlayerState.Attack);
            _stateMachine.AddTransition<MoveState, PhysicalAttackState>(PlayerState.Attack);
            _stateMachine.AddTransition<PhysicalAttackState, IdleState>(PlayerState.Idle);

            _stateMachine.SetStartState<MoveState>();
        }
    }
#if UNITY_EDITOR
    [CustomEditor(typeof(Player))]
    public class PlayerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (!Application.isPlaying) return;

            var manager = (Player)target;
            EditorGUILayout.LabelField($"State: {manager.CurrentState}");
            EditorGUILayout.LabelField($"HP: {manager.CurrentHp}");
        }
    }
#endif
}