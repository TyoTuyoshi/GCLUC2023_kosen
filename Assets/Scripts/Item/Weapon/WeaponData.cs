using UnityEngine;

namespace Item.Weapon
{
    /// <summary>
    ///     武器データを保存するスクリプタブルオブジェクト
    /// </summary>
    [CreateAssetMenu(fileName = "WeaponData", menuName = "Data/Weapon", order = 0)]
    public class WeaponData : ItemDataScriptable
    {
        [SerializeField] private string weaponName;
        [SerializeField] private string description;
        [SerializeField] private ItemRare rare;
        [SerializeField] private float attackPower;
        [SerializeField] private Weapon itemPrefab;

        /// <summary>
        ///     武器の攻撃力
        /// </summary>
        public float AttackPower => attackPower;

        public override string Name => weaponName;
        public override string Description => description;
        public override ItemRare Rare => rare;
        public override IItem ItemPrefab => itemPrefab;
    }
}