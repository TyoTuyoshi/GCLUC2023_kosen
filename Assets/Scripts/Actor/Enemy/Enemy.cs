using System.Linq;
using IceMilkTea.Core;
using UnityEditor;
using UnityEngine;
using Utils;

namespace Actor.Enemy
{
    public partial class Enemy : ActorBase, IDamageableActor
    {
        [SerializeField] private Pair<EnemyState, string>[] animateStates;

        private Animator _animator;
        private Vector2 _initPos; // 初期状態の座標
        private float _lastPlayerSearched;
        private ActorBase _playerActor;
        private Rigidbody2D _rigid;
        private ImtStateMachine<Enemy, EnemyState> _stateMachine;
        public string CurrentStateName => _stateMachine.CurrentStateName;

        public override int Level => 1; // TODO 

        private void Start()
        {
            TryGetComponent(out _animator);
            TryGetComponent(out _rigid);
            _initPos = _rigid.position;

            _stateMachine = new ImtStateMachine<Enemy, EnemyState>(this);
            _stateMachine.AddTransition<IdleState, MoveState>(EnemyState.Move);
            _stateMachine.AddTransition<AttackState, MoveState>(EnemyState.Move);
            _stateMachine.AddTransition<MoveState, IdleState>(EnemyState.Idle);
            _stateMachine.AddTransition<AttackState, IdleState>(EnemyState.Idle);
            _stateMachine.AddTransition<IdleState, AttackState>(EnemyState.Attack);
            _stateMachine.AddTransition<MoveState, AttackState>(EnemyState.Attack);
            _stateMachine.AddAnyTransition<DeathState>(EnemyState.Death);

            _stateMachine.SetStartState<IdleState>();

            InitDamageable();
        }

        private void FixedUpdate()
        {
            // プレイヤーアクターが存在しないなら更新せずに2秒ごとに探す
            if (_playerActor == null || !_playerActor.isActiveAndEnabled)
            {
                if (Time.time - _lastPlayerSearched < 2f) return;

                GameObject.FindWithTag("Player")?.TryGetComponent(out _playerActor);
                _lastPlayerSearched = Time.time;
                return;
            }

            _stateMachine.Update();
        }

        private void ChangeState(EnemyState state)
        {
            _stateMachine.SendEvent(state);

            if (animateStates.FirstOrDefault(v => v.First == state) is not { } animState) return;
            _animator.CrossFade(animState.Second, 0.2f);
        }
    }

    internal enum EnemyState
    {
        Move,
        Attack,
        Idle,
        Death
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(Enemy))]
    public class EnemyEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (Application.isPlaying && target is Enemy enemy) GUILayout.Label($"State: {enemy.CurrentStateName}");
        }
    }
#endif
}