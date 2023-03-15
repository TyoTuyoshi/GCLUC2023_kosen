using Actor;
using Event;
using UnityEngine;

namespace Item.Default
{
    /// <summary>
    ///     回復用のアイテム
    /// </summary>
    public class CureItem : MonoBehaviour, IItem
    {
        [SerializeField] private CureItemData itemData;
        public IItemData ItemData => itemData;

        public void PickItem()
        {
            // TODO: 演出
            Destroy(gameObject);
        }

        public void Use(ActorBase actor)
        {
            EventPublisher.Instance.PublishEvent(new HealEvent
            {
                Amount = itemData.HealAmount,
                Target = actor
            });
        }
    }
}