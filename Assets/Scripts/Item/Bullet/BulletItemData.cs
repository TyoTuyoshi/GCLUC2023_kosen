using Actor.Bullet;
using UnityEngine;

namespace Item.Bullet
{
    [CreateAssetMenu(fileName = "BulletData", menuName = "Data/Bullet", order = 0)]
    public class BulletItemData : ItemDataScriptable
    {
        [SerializeField] private string itemName;
        [SerializeField] private string description;
        [SerializeField] private ItemRare rare;
        [SerializeField] private BulletBase bulletPrefab;
        [SerializeField] private float damageAmount = 1f;
        public float Damage => damageAmount;

        public override string Name => itemName;
        public override string Description => description;
        public override ItemRare Rare => rare;
        public override IItem ItemPrefab => bulletPrefab;
    }
}