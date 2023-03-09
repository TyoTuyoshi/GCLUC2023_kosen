namespace Actor
{
    /// <summary>
    ///     単一Actorのコンポーネント間でやり取りされるイベントデータ
    /// </summary>
    public interface IActorEvent
    {
    }

    /// <summary>
    ///     Actorがダメージを受けたときに発行されるイベント
    /// </summary>
    public class DamageEvent : IActorEvent
    {
        public float Damage { get; init; }
    }

    /// <summary>
    ///     Actorでのアニメーションイベント
    /// </summary>
    internal class AnimationEvent : IActorEvent
    {
        public string EventName { get; init; }
    }

    /// <summary>
    ///     動けなくなるイベント
    /// </summary>
    public class StanEvent : IActorEvent
    {
        /// <summary>
        ///     動けなくなる時間
        /// </summary>
        public float StanDuration { get; init; }
    }

    internal class DeathEvent : IActorEvent
    {
    }
}