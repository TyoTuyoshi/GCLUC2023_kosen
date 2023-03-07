using System;
using UniRx;
using UnityEngine;

namespace Actor.Enemy
{
    public partial class Enemy : ActorBase
    {
        private readonly Subject<IActorEvent> _onActorEvent = new();

        private Animator _animator;
        private ActorBase _playerActor;
        public IObservable<IActorEvent> OnActorEvent => _onActorEvent;

        public override int Level => 1; // TODO 

        private void Start()
        {
            TryGetComponent(out _animator);
            GameObject.FindWithTag("Player").TryGetComponent(out _playerActor);
        }

        public override void PublishActorEvent(IActorEvent ev)
        {
            _onActorEvent.OnNext(ev);
            Debug.Log($"Publish event: {ev.GetType()}");
        }
    }
}