using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot_cy : MonoBehaviour
{
    public Image icon;
    public Text itemName_Text;
    public Text itemCount_Text;
    public GameObject selected_item;

    public void Additem(Item_cy _item)
    {
        itemName_Text.text = _item.itemName;
        icon.sprite = _item.itemIcon;
        if (Item_cy.ItemType.Consume == _item.itemType)
        {
            if (_item.itemCount > 0)
                itemCount_Text.text = "x " + _item.itemCount.ToString();
            else
                itemCount_Text.text = "";
        }
    }
    public void RemoveItem()
    {
        itemName_Text.text = "";
        itemCount_Text.text = "";
        icon.sprite = null;
    }
}
