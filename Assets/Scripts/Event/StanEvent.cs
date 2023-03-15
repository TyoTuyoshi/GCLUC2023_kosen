using System;
using UniRx;
using UnityEngine;

namespace Event
{
    /// <summary>
    ///     スタンさせるイベント
    /// </summary>
    public class StanEvent : IEvent
    {
        public float EffectRange { get; init; }
        public float Duration { get; init; }
        public Vector3 SourcePos { get; init; }

        public static IObservable<StanEvent> RegisterListenerInRange(Transform self)
        {
            return EventPublisher.Instance
                .RegisterListener<StanEvent>()
                .Where(e => e.SourcePos != self.position)
                .Where(e => (e.SourcePos - self.position).sqrMagnitude < Mathf.Pow(e.EffectRange, 2));
        }
    }
}