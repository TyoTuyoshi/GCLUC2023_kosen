using Actor.Player;
using Item;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class ItemSlot : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI countText;
        [SerializeField] private ItemBase item;
        [SerializeField] private Image selectedImage;

        private int _count;

        public bool Selected
        {
            get => selectedImage.enabled;
            set => selectedImage.enabled = value;
        }

        public int Count
        {
            get => _count;
            set
            {
                _count = Mathf.Max(value, 0);
                countText.text = _count.ToString();
            }
        }

        public void UseItem(Player player)
        {
            item.Use(player);
        }
    }
}