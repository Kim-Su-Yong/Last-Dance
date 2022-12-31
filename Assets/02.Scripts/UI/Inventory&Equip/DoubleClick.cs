using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DoubleClick : MonoBehaviour, IPointerClickHandler
{
    private HoverTipManager hoverTipManager;

    float interval = 0.25f;
    float doubleClickedTime = -1.0f;
    bool isDoubleClicked = false;

    ItemInfo itemInfo;
    PlayerDamage playerDamage;

    public GameObject[] equipmentslots; //장비창의 슬롯들

    void Start()
    {
        hoverTipManager = FindObjectOfType<HoverTipManager>();
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
                    "수량 : " + itemInfo.GetComponent<HoverTip>().itemCount.ToString() + "개";
                itemInfo.GetComponent<InventorySlot>().itemCount--;
                itemInfo.GetComponent<InventorySlot>().itemCount_Text.text =
                    itemInfo.GetComponent<InventorySlot>().itemCount.ToString();
                isDoubleClicked = false;
            }
            else if (itemInfo.itemCount == 1)
            {
                playerDamage.RestoreHp(itemInfo.AddHp);
                Inventory.instance.inventoryItemList.Remove(itemInfo);
                hoverTipManager.tipWindow.gameObject.SetActive(false);
                Destroy(this.gameObject);
                isDoubleClicked = false;
            }
        }
        //else if (itemInfo.itemType == ItemInfo.ItemType.Equip && isDoubleClicked)
        //{
        //    equipSlot();
        //    Destroy(this.gameObject);
        //    isDoubleClicked = false;
        //}
    }
    //public void equipSlot()
    //{
    //    for (int i = 0; i < equipmentslots.Length; i++)
    //    {
    //        switch (equipmentslots[i].GetComponent<Drop>().equipType)
    //        {
    //            case Drop.EquipType.WeaponSlot:
    //                if (itemInfo.equipType == ItemInfo.EquipType.Weapon)
    //                {
    //                    GameObject clone = Instantiate(this.gameObject, Vector3.zero, Quaternion.identity);
    //                    clone.transform.SetParent(equipmentslots[i].transform);
    //                }
    //                break;
    //            case Drop.EquipType.HelmetSlot:
    //                if (itemInfo.equipType == ItemInfo.EquipType.Helmet)
    //                {
    //                    GameObject clone = Instantiate(this.gameObject, Vector3.zero, Quaternion.identity);
    //                    clone.transform.SetParent(equipmentslots[i].transform);
    //                }
    //                break;
    //        }
    //    }
    //}
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
