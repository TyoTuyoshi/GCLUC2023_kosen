using Actor;

namespace Item
{
    /// <summary>
    ///     アイテムのインターフェース
    /// </summary>
    public interface IItem
    {
        public IItemData ItemData { get; }
        
        /// <summary>
        /// アイテムを取得したときの処理
        /// </summary>
        public void PickItem();

        /// <summary>
        /// アイテムを使用したときの処理
        /// </summary>
        public void Use(ActorBase actor);
    }
}