using System;
using UniRx;
using UnityEngine;

namespace Actor.Enemy
{
    public class Enemy : MonoBehaviour, IActor
    {
        private readonly Subject<IActorEvent> _onActorEvent = new();
        public IObservable<IActorEvent> OnActorEvent => _onActorEvent;

        public int Level => 1; // TODO 
        public void PublishActorEvent(IActorEvent ev)
        {
            _onActorEvent.OnNext(ev);
        }
    }
}