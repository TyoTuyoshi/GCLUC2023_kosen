using System;
using System.Linq;
using IceMilkTea.Core;
using UniRx;
using UnityEditor;
using UnityEngine;
using Utils;

namespace Actor.Enemy
{
    public partial class Enemy : ActorBase
    {
        [SerializeField] private Pair<EnemyState, string>[] animateStates;

        private readonly Subject<IActorEvent> _onActorEvent = new();

        private Animator _animator;
        private Vector2 _initPos; // 初期状態の座標
        private float _LastPlayerSearched;
        private ActorBase _playerActor;
        private Rigidbody2D _rigid;
        private ImtStateMachine<Enemy, EnemyState> _stateMachine;
        public IObservable<IActorEvent> OnActorEvent => _onActorEvent;
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
            _stateMachine
                .ObserveEveryValueChanged(v => v.LastAcceptedEventID)
                .Subscribe(state =>
                {
                    if (animateStates.FirstOrDefault(v => v.First == state) is not { } animState) return;
                    Debug.Log(animState.Second, gameObject);
                    _animator.CrossFade(animState.Second, 0.2f);
                })
                .AddTo(this);

            RegisterEvents();
        }

        private void FixedUpdate()
        {
            // プレイヤーアクターが存在しないなら更新せずに2秒ごとに探す
            if (_playerActor == null || !_playerActor.isActiveAndEnabled)
            {
                if (Time.time - _LastPlayerSearched < 2f) return;
                
                GameObject.FindWithTag("Player")?.TryGetComponent(out _playerActor);
                _LastPlayerSearched = Time.time;
                return;
            }

            _stateMachine.Update();
        }

        public override void PublishActorEvent(IActorEvent ev)
        {
            _onActorEvent.OnNext(ev);
        }

        private void RegisterEvents()
        {
            OnActorEvent
                .Where(e => e is DeathEvent)
                .Subscribe(_ => _stateMachine.SendEvent(EnemyState.Death))
                .AddTo(this);
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