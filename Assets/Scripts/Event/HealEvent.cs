using System;
using Actor;
using UniRx;

namespace Event
{
    /// <summary>
    ///     Actorを回復させるイベント
    /// </summary>
    public class HealEvent : IEvent
    {
        /// <summary>
        ///     回復量
        /// </summary>
        public float Amount { get; init; }

        /// <summary>
        ///     対象
        /// </summary>
        public ActorBase Target { get; init; }

        public static IObservable<HealEvent> RegisterListenerInTarget(ActorBase actor)
        {
            return EventPublisher.Instance
                .RegisterListener<HealEvent>()
                .Where(e => e.Target == actor);
        }
    }
}