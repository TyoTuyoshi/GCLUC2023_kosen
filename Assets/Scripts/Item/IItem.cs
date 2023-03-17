using Actor;
using UnityEngine;

namespace Item
{
    /// <summary>
    ///     アイテムのインターフェース
    /// </summary>
    public interface IItem
    {
        public IItemData ItemData { get; }

        /// <summary>
        ///     アイテムを取得したときの処理
        /// </summary>
        public void PickItem();

        /// <summary>
        ///     アイテムを使用したときの処理
        /// </summary>
        public void Use(ActorBase actor);
    }

    public abstract class ItemBase : MonoBehaviour, IItem
    {
        public abstract IItemData ItemData { get; }
        public abstract void PickItem();
        public abstract void Use(ActorBase actor);
    }
}