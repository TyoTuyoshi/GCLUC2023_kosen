using Actor;
using Event;
using UnityEngine;

namespace Item.Default
{
    /// <summary>
    ///     回復用のアイテム
    /// </summary>
    public class CureItem : ItemBase
    {
        [SerializeField] private CureItemData itemData;
        public override IItemData ItemData => itemData;

        public override void PickItem()
        {
            // TODO: 演出
            Destroy(gameObject);
        }

        public override void Use(ActorBase actor)
        {
            EventPublisher.Instance.PublishEvent(new HealEvent
            {
                Amount = itemData.HealAmount,
                Target = actor
            });
        }
    }
}