using System;
using Actor;
using UnityEngine;

namespace Item.Weapon
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class Weapon : MonoBehaviour, IItem
    {
        [SerializeField] private WeaponData weaponData;

        public IItemData ItemData => weaponData;

        public void PickItem()
        {
            throw new NotImplementedException();
        }

        public void Use(ActorBase actor)
        {
            throw new NotImplementedException();
        }
    }
}