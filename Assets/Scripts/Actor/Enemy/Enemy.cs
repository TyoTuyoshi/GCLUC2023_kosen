using System;
using UniRx;
using UnityEngine;

namespace Actor.Enemy
{
    public class Enemy : ActorBase
    {
        private readonly Subject<IActorEvent> _onActorEvent = new();
        public IObservable<IActorEvent> OnActorEvent => _onActorEvent;

        public override int Level => 1; // TODO 
        public override void PublishActorEvent(IActorEvent ev)
        {
            _onActorEvent.OnNext(ev);
            Debug.Log($"Publish event: {ev.GetType()}");
        }
    }
}