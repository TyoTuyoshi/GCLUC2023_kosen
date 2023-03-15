using Actor;

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
    }
}