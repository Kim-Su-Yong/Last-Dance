using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class InventoryUI : MonoBehaviour
{
    bool activeInventory = false;
    //public GameObject inventoryPanel;
    public Slot[] slots;


    public ShopSlot[] shopSlots;
    public Transform shopHolder;
    private ShopData shopData;

    public GameObject shop;
    public Button closeShop;
    public bool isStoreActive;
    void Start()
    {
        shopSlots = shopHolder.GetComponentsInChildren<ShopSlot>();
        for (int i = 0; i < shopSlots.Length; i++)
        {
            shopSlots[i].Init(this);
            shopSlots[i].slotnum = i;
        }
        //closeShop.onClick.AddListener(DeActiveShop); //상점 닫을시
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
            Shop();
    }
    void Shop() //상점 함수
    {
        if (!isStoreActive)
        {
            //ActiveShop(true);
            shopData = GetComponent<ShopData>();
            for (int i = 0; i < shopData.stocks.Count; i++)
            {
                shopSlots[i].item = shopData.stocks[i];
                shopSlots[i].UpdateSlotUI();
            }
        }
    }
    public void Buy(int num) //상점에서 아이템 구매 함수
    {
        shopData.soldOuts[num] = true;
    }
    //public void ActiveShop(bool isOpen) //상점 열기
    //{
    //    if (!activeInventory)
    //    {
    //        isStoreActive = isOpen;
    //        shop.SetActive(isOpen);
    //        //inventoryPanel.SetActive(isOpen);
    //        for (int i = 0; i < slots.Length; i++)
    //        {
    //            slots[i].isShopMode = isOpen;
    //        }
    //    }
    //}
    //public void DeActiveShop() //상점 닫기
    //{
    //    ActiveShop(false);
    //    shopData = null;
    //    for (int i = 0; i < shopSlots.Length; i++)
    //    {
    //        shopSlots[i].RemoveSlot();
    //    }
    //}
    //public void SellBtn() //상점에 아이템 팔기 함수
    //{
    //    for (int i = slots.Length; i > 0; i--)
    //    {
    //        slots[i - 1].SellItem();
    //    }
    //}
}
