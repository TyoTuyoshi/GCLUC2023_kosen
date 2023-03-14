using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Item_kari
{
    public int item_limit; //所持限界数
    public string item_name; //名前

    public int item_stock; //在庫
    //...
}

public class ItemUse : MonoBehaviour
{
    [SerializeField] private Text[] ItemStock; //ボタンの在庫表示用

    [SerializeField] private Item_kari[] Items;

    //private int item_limit;
    private void Start()
    {
        Items.Select(x => x.item_name = "");
        ItemStock.Select(x => x.text = "");
    }

    // Update is called once per frame
    private void Update()
    {
    }

    public void ItemButton(int num)
    {
        switch (num)
        {
            case 0: //回復系アイテムとか
                ItemStock[num].text = $"{Items[num].item_name}\n{Items[num].item_stock}/{Items[num].item_stock}";
                break;
            case 1: //武器系アイテムとか
                break;
            case 2: //特になし
                break;
        }
    }
}