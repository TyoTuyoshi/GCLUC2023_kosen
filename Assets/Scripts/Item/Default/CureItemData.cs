using UnityEngine;

namespace Item.Default
{
    [CreateAssetMenu(fileName = "CureItemData", menuName = "Data/CureItem", order = 0)]
    public class CureItemData : ScriptableObject, IItemData
    {
        [SerializeField] private string itemName;
        [SerializeField] private string description;
        [SerializeField] private ItemRare rare;
        [SerializeField] private CureItem item;
        [SerializeField] private float healAmount = 2f;

        public string Name => itemName;
        public string Description => description;
        public ItemRare Rare => rare;
        public IItem ItemPrefab => item;
        public float HealAmount => healAmount;
    }
}