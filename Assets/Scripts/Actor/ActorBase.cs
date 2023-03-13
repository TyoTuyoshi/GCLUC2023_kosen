using System;
using UniRx;
using UnityEngine;

namespace Actor
{
    public abstract class ActorBase : MonoBehaviour
    {
        private readonly Subject<string> _onAnimEvent = new();
        public abstract int Level { get; }
        protected IObservable<string> OnAnimEvent => _onAnimEvent;

        private void AnimEvent(string eventName)
        {
            _onAnimEvent.OnNext(eventName);
        }
    }
}