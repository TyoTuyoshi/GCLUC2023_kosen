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
        /// このアイテムのプレファブオブジェクト
        /// </summary>
        public IItem ItemPrefab { get; }
    }

    public abstract class ItemDataScriptable : ScriptableObject, IItemData
    {
        public abstract string Name { get; }
        public abstract string Description { get; }
        public abstract ItemRare Rare { get; }
        public abstract IItem ItemPrefab { get; }
    }
}