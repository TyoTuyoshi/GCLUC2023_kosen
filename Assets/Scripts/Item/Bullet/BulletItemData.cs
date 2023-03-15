using Actor.Bullet;
using UnityEngine;

namespace Item.Bullet
{
    [CreateAssetMenu(fileName = "BulletData", menuName = "Data/Bullet", order = 0)]
    public class BulletItemData : ScriptableObject, IItemData
    {
        [SerializeField] private string itemName;
        [SerializeField] private string description;
        [SerializeField] private ItemRare rare;
        [SerializeField] private BulletBase bulletPrefab;
        [SerializeField] private float damageAmount = 1f;
        public float Damage => damageAmount;

        public string Name => itemName;
        public string Description => description;
        public ItemRare Rare => rare;
        public IItem ItemPrefab => bulletPrefab;
    }
}