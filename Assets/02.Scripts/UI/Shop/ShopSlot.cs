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
    public void Init(InventoryUI lui) //���� ó�� ���������� ���̴� �Լ�
    {
        inventoryUI = lui;
    }
    public void UpdateSlotUI() //���� �������� ���� ������Ʈ�ϴ� �Լ�
    {
        //itemIcon.sprite = item.GetComponent<Item>().itemIcon;
        itemIcon.gameObject.SetActive(true);
        if (soldOut)
            itemIcon.color = new Color(0.5f, 0.5f, 0.5f);
    }
    public void RemoveSlot() //���� �ݾ����� ���� �Ⱥ��̰� �ϴ� �Լ�
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
