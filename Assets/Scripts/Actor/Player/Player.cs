using IceMilkTea.Core;
using UI;
using UnityEditor;
using UnityEngine;

namespace Actor.Player
{
    public partial class Player : ActorBase, IDamageableActor
    {
        [Header("Main")] [Space] [SerializeField]
        private LayerMask enemyLayer;

        [SerializeField] private TipsUI tipsUI;

        private Animator _animator;
        public GameInput.PlayerActions Input { get; private set; }
        private Rigidbody2D _rigid;
        private ImtStateMachine<Player, PlayerState> _stateMachine;
        public override int Level => 1;
#if UNITY_EDITOR
        public string CurrentState => _stateMachine.CurrentStateName;
#endif

        private void Start()
        {
            Input = new GameInput().Player;
            Input.Enable();

            TryGetComponent(out _rigid);
            TryGetComponent(out _animator);

            InitStateMachine();
            InitDamageable();
        }

        private void FixedUpdate()
        {
            _stateMachine.Update();

            if (Input.Attack.IsPressed()) TransAttack();
        }

        private void TransAttack()
        {
            // 近くに敵がいるか
            if (Physics2D.OverlapCircle(_rigid.position, _animator.GetFloat(AnimIdAttackRange), enemyLayer) != null)
                _stateMachine.SendEvent(PlayerState.Attack);
            else
                _stateMachine.SendEvent(PlayerState.GunAttack);
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

            _stateMachine.AddAnyTransition<DeathState>(PlayerState.Death);

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