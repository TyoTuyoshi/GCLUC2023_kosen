using System;
using Actor;
using UnityEngine;

namespace Item.Weapon
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class Weapon : ItemBase
    {
        [SerializeField] private WeaponData weaponData;

        public override IItemData ItemData => weaponData;

        public override void PickItem()
        {
            throw new NotImplementedException();
        }

        public override void Use(ActorBase actor)
        {
            throw new NotImplementedException();
        }
    }
}