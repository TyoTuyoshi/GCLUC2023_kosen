using UnityEngine;

namespace Item.Default
{
    [CreateAssetMenu(fileName = "CureItemData", menuName = "Data/CureItem", order = 0)]
    public class CureItemData : ItemDataScriptable
    {
        [SerializeField] private string itemName;
        [SerializeField] private string description;
        [SerializeField] private ItemRare rare;
        [SerializeField] private CureItem item;
        [SerializeField] private float healAmount = 2f;

        public override string Name => itemName;
        public override string Description => description;
        public override ItemRare Rare => rare;
        public override IItem ItemPrefab => item;
        public float HealAmount => healAmount;
    }
}