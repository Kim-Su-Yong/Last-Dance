using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class ShopSlot : MonoBehaviour //, IPointerUpHandler
{
    public int slotnum;
    public GameObject item;
    public Image itemIcon;
    public bool soldOut = false;
    InventoryUI inventoryUI;

    public int itemID;
    public int _count;
    public Button button;
    public Button button1;
    private void Awake()
    {
        //ConsumeSlots = GameObject.FindGameObjectsWithTag("Consume");
        button = GetComponent<Button>();
        button1 = button.transform.GetChild(2).GetComponentInChildren<Button>();
    }
    public void Init(InventoryUI lui) //게임 처음 시작했을때 쓰이는 함수
    {
        inventoryUI = lui;
    }
    public void UpdateSlotUI() //상점 열었을때 슬롯 업데이트하는 함수
    {
        //itemIcon.sprite = item.GetComponent<Item>().itemIcon;
        itemIcon.gameObject.SetActive(true);
        if (soldOut)
            itemIcon.color = new Color(0.5f, 0.5f, 0.5f);
    }
    public void RemoveSlot() //상점 닫았을때 슬롯 안보이게 하는 함수
    {
        item = null;
        soldOut = false;
        itemIcon.gameObject.SetActive(false);
    }
    //public void OnPointerUp(PointerEventData eventData)
    public void Buy()
    { 
        //if (item != null)
        //{
            //if (DatabaseManager.instance.money >= item.itemCost && !soldOut && Inventory.instance.items.Count < Inventory.instance.SlotCnt)
            //{
                //DatabaseManager.instance.money -= item.itemCost;
                Inventory.instance.GetAnItem(itemID, _count);
                soldOut = true;
                button.interactable = false;
                button1.interactable = false;
                //inventoryUI.Buy(slotnum);
                UpdateSlotUI();
            //}
        //}
    }
}
