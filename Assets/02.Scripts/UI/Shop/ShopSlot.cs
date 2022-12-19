using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class ShopSlot : MonoBehaviour
{
    public int slotnum;
    public ItemInfo item;
    public Image itemIcon;
    public bool soldOut = false;
    InventoryUI inventoryUI;

    public GameObject[] ConsumeSlots;
    private void Awake()
    {
        ConsumeSlots = GameObject.FindGameObjectsWithTag("Consume");
    }
    public void Init(InventoryUI lui)
    {
        inventoryUI = lui;
    }
    public void UpdateSlotUI()
    {
        //itemIcon.sprite = item.itemImage;
        itemIcon.gameObject.SetActive(true);
        if (soldOut)
            itemIcon.color = new Color(0.5f, 0.5f, 0.5f);
    }
    public void RemoveSlot()
    {
        item = null;
        soldOut = false;
        itemIcon.gameObject.SetActive(false);
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        if (item != null)
        {
            //if (DatabaseManager.instance.money >= item.itemCost && !soldOut && Inventory.instance.items.Count < Inventory.instance.SlotCnt)
            //{
                //DatabaseManager.instance.money -= item.itemCost;
                Inventory_cy.instance.AddItem(ConsumeSlots); //좀 더 수정해야함
                soldOut = true;
                inventoryUI.Buy(slotnum);
                UpdateSlotUI();
            //}
        }
    }
}
