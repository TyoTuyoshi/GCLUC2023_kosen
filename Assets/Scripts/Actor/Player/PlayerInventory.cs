using System;
using Actor.Bullet;
using Item;
using Item.Default;
using Item.Weapon;
using UI;
using UnityEngine;

namespace Actor.Player
{
    /// <summary>
    ///     プレイヤーのアイテムを保持する
    /// </summary>
    public class PlayerInventory : MonoBehaviour
    {
        [SerializeField] private ItemSlot[] itemSlots = new ItemSlot[3];
        private int _selectSlot;

        public int SelectSlot
        {
            get => _selectSlot;
            set
            {
                if (_selectSlot is < 0 or > 2) return;
                itemSlots[_selectSlot].Selected = false;
                itemSlots[value].Selected = true;
                _selectSlot = value;
            }
        }

        public void AddItem(IItem item)
        {
            GetSlot(item).Count++;
        }

        public void UseItem(int slot, Player player)
        {
            if (itemSlots[slot].Count == 0) return;

            itemSlots[slot].UseItem(player);
            itemSlots[slot].Count--;
        }

        private ItemSlot GetSlot(IItem item)
        {
            var num = item switch
            {
                NormalBullet => 0,
                CureItem => 1,
                Weapon => 2,
                _ => throw new ArgumentOutOfRangeException(nameof(item))
            };
            return itemSlots[num];
        }
    }
}