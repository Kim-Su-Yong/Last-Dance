using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DoubleClick : MonoBehaviour, IPointerClickHandler
{
    float interval = 0.25f;
    float doubleClickedTime = -1.0f;
    bool isDoubleClicked = false;

    ItemInfo itemInfo;
    PlayerDamage playerDamage;

    void Start()
    {
        itemInfo = this.GetComponent<ItemInfo>();
        playerDamage = FindObjectOfType<PlayerDamage>();
    }
    void Update()
    {
        if (itemInfo.itemType == ItemInfo.ItemType.Consume && isDoubleClicked)
        {
            if (itemInfo.itemCount > 1)
            {
                playerDamage.RestoreHp(itemInfo.AddHp);
                itemInfo.itemCount--;
                itemInfo.GetComponent<HoverTip>().itemCount--;
                itemInfo.GetComponent<HoverTip>().countToShow =
                    "¼ö·® : " + itemInfo.GetComponent<HoverTip>().itemCount.ToString() + "°³";
                itemInfo.GetComponent<InventorySlot>().itemCount--;
                itemInfo.GetComponent<InventorySlot>().itemCount_Text.text =
                    itemInfo.GetComponent<InventorySlot>().itemCount.ToString();
                isDoubleClicked = false;
            }
            else if (itemInfo.itemCount == 1)
            {
                playerDamage.RestoreHp(itemInfo.AddHp);
                Inventory.instance.inventoryItemList.Remove(itemInfo);
                Destroy(this.gameObject);
                isDoubleClicked = false;
            }
        }
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if ((Time.time - doubleClickedTime) < interval)
        {
            isDoubleClicked = true;
            doubleClickedTime = -1.0f;

            Debug.Log("double click!");
        }
        else
        {
            isDoubleClicked = false;
            doubleClickedTime = Time.time;
        }
    }
}
