using System;
using UniRx;
using UnityEngine;

namespace Event
{
    public class EventPublisher : MonoBehaviour
    {
        private static EventPublisher _instance;
        private readonly Subject<IEvent> _onEvent = new();
        public static EventPublisher Instance => _instance;
        public IObservable<IEvent> OnEvent => _onEvent;

        private void Awake()
        {
            if (_instance != null) Destroy(gameObject);
            else TryGetComponent(out _instance);
        }

        public IObservable<T> RegisterListener<T>()
        {
            return _onEvent.Where(e => e is T).Select(e => (T)e);
        }

        public void PublishEvent(IEvent e)
        {
            _onEvent.OnNext(e);
        }
    }
}