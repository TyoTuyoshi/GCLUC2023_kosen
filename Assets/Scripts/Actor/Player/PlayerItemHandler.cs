using Item;
using UnityEngine;

namespace Actor.Player
{
    public class PlayerItemHandler : MonoBehaviour
    {
        [SerializeField] private PlayerInventory inventory;
        [SerializeField] private LayerMask itemLayer;
        [SerializeField] private int _currentItemSlot;

        private Player _player;

        private void Start()
        {
            TryGetComponent(out _player);
        }

        private void Update()
        {
            SlotSelect();
            UseItem();
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            if ((itemLayer & (1 << col.gameObject.layer)) == 0) return;
            if (!col.gameObject.TryGetComponent(out IItem item)) return;

            item.PickItem();
            inventory.AddItem(item);
        }

        private void UseItem()
        {
            if (_player.Input.ItemUse.WasPressedThisFrame()) inventory.UseItem(_currentItemSlot, _player);
        }

        private void SlotSelect()
        {
            var selectValue = _player.Input.ItemSelect.ReadValue<float>();
            if (selectValue == 0) return;

            _currentItemSlot += selectValue < 0 ? 1 : -1;
            _currentItemSlot = _currentItemSlot switch
            {
                < 0 => 2,
                > 2 => 0,
                _ => _currentItemSlot
            };
            inventory.SelectSlot = _currentItemSlot;
        }
    }
}