namespace Actor
{
    /// <summary>
    /// 単一Actorのコンポーネント間でやり取りされるイベントデータ
    /// </summary>
    public interface IActorEvent
    {
    }

    /// <summary>
    /// Actorがダメージを受けたときに発行されるイベント
    /// </summary>
    public class DamageEvent : IActorEvent
    {
        public float Damage { get; init; }
    }
}