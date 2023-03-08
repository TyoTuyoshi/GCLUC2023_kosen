using System;
using UnityEngine;

namespace Item.Weapon
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class Weapon : MonoBehaviour, IItem
    {
        [SerializeField] private WeaponData weaponData;
        private SpriteRenderer _renderer;

        private void Start()
        {
            TryGetComponent(out _renderer);

            _renderer.sprite = weaponData.ItemSprite;
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (_renderer == null && !TryGetComponent(out _renderer))
                Debug.LogWarning("`SpriteRenderer`を取得できませんでした。");
            if (_renderer != null && weaponData is not null) _renderer.sprite = weaponData.ItemSprite;
        }
#endif

        public IItemData ItemData => weaponData;

        public void PickItem()
        {
            throw new NotImplementedException();
        }

        public void Use()
        {
            throw new NotImplementedException();
        }
    }
}