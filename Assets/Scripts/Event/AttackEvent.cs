using Actor;

namespace Event
{
    /// <summary>
    ///     攻撃を伝えるイベント
    /// </summary>
    public class AttackEvent : IEvent
    {
        /// <param name="source">攻撃したActor</param>
        /// <param name="attackRange">攻撃範囲</param>
        /// <param name="amount">攻撃で与えるダメージ量</param>
        public AttackEvent(ActorBase source, float attackRange, float amount)
        {
            Source = source;
            AttackRange = attackRange;
            Amount = amount;
        }

        /// <summary>
        ///     攻撃をしたActor
        /// </summary>
        public ActorBase Source { get; init; }

        /// <summary>
        ///     攻撃の範囲
        /// </summary>
        public float AttackRange { get; init; }

        /// <summary>
        ///     ダメージ量
        /// </summary>
        public float Amount { get; init; }

        /// <summary>
        ///     イベント発行
        /// </summary>
        public void Publish()
        {
            EventPublisher.Instance.PublishEvent(this);
        }
    }
}