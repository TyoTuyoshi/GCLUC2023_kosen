namespace Actor
{
    /// <summary>
    ///     ダメージを受けることができるアクター
    /// </summary>
    public interface IDamageableActor
    {
        /// <summary>
        ///     最大体力
        /// </summary>
        public float MaxHp { get; }

        /// <summary>
        ///     現在の体力
        /// </summary>
        public float CurrentHp { get; }
    }
}