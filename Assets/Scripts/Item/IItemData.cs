using UnityEngine;

namespace Item
{
    /// <summary>
    ///     アイテムのデータのインターフェース
    /// </summary>
    public interface IItemData
    {
        /// <summary>
        ///     アイテムの名前
        /// </summary>
        public string Name { get; }

        /// <summary>
        ///     アイテムの説明
        /// </summary>
        public string Description { get; }

        /// <summary>
        ///     アイテムのレア度
        /// </summary>
        public ItemRare Rare { get; }

        /// <summary>
        ///     アイテムのスプライト
        /// </summary>
        public Sprite ItemSprite { get; }
    }
}