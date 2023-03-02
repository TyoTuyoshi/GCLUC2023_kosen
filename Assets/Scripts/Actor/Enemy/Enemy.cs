using System;
using UniRx;
using UnityEngine;

namespace Actor.Enemy
{
    public class Enemy : MonoBehaviour
    {
        private readonly Subject<IActorEvent> _onActorEvent = new();
        public IObservable<IActorEvent> OnActorEvent => _onActorEvent;

        public void PublishEvent(IActorEvent ev)
        {
            _onActorEvent.OnNext(ev);
        }
    }
}