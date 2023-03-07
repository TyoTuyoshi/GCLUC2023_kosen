using System;
using IceMilkTea.Core;
using UniRx;
using UnityEditor;
using UnityEngine;

namespace Actor.Enemy
{
    public partial class Enemy : ActorBase
    {
        private readonly Subject<IActorEvent> _onActorEvent = new();

        private Animator _animator;
        private ActorBase _playerActor;
        private Rigidbody2D _rigid;
        private ImtStateMachine<Enemy, EnemyState> _stateMachine;
        private Vector2 _initPos; // 初期状態の座標
        public IObservable<IActorEvent> OnActorEvent => _onActorEvent;
        public string CurrentStateName => _stateMachine.CurrentStateName;

        public override int Level => 1; // TODO 

        private void Start()
        {
            TryGetComponent(out _animator);
            TryGetComponent(out _rigid);
            GameObject.FindWithTag("Player").TryGetComponent(out _playerActor);
            _initPos = _rigid.position;

            _stateMachine = new ImtStateMachine<Enemy, EnemyState>(this);
            _stateMachine.AddTransition<IdleState, MoveState>(EnemyState.Move);
            _stateMachine.AddTransition<AttackState, MoveState>(EnemyState.Move);
            _stateMachine.AddTransition<MoveState, IdleState>(EnemyState.Idle);
            _stateMachine.AddTransition<AttackState, IdleState>(EnemyState.Idle);
            _stateMachine.AddTransition<IdleState, AttackState>(EnemyState.Attack);
            _stateMachine.AddTransition<MoveState, AttackState>(EnemyState.Attack);

            _stateMachine.SetStartState<IdleState>();
        }

        private void Update()
        {
            _stateMachine.Update();
        }

        public override void PublishActorEvent(IActorEvent ev)
        {
            _onActorEvent.OnNext(ev);
        }
    }

    internal enum EnemyState
    {
        Move,
        Attack,
        Idle
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