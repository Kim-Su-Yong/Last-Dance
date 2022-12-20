using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory_cy : MonoBehaviour
{
    public static Inventory_cy instance;

    private DatabaseManager_cy theData;

    private HoverTip hoverTip;
    private ItemInfo itemInfo;

    public GameObject[] EquipSlots;
    public GameObject[] ConsumeSlots;
    public GameObject[] ETCSlots;
    
    public int ItemNumber; //������ ���̵�
    public int ItemCount; //������ ����

    [SerializeField]
    private InventorySlot_cy slots; //�κ��丮 ���Ե�
    private List<ItemInfo> inventoryItemList; //�÷��̾ ������ ������ ����Ʈ

    public GameObject ItemSlot; //�κ��丮 ������ �� ������ ������ ��� ������Ʈ


    public GameObject prefab_floating_text;
    public Transform messageTr;
    private void Awake()
    {
        EquipSlots = GameObject.FindGameObjectsWithTag("Equip");
        ConsumeSlots = GameObject.FindGameObjectsWithTag("Consume");
        ETCSlots = GameObject.FindGameObjectsWithTag("ETC");
        ConsumeSlots[0].transform.parent.gameObject.SetActive(false);
        ETCSlots[0].transform.parent.gameObject.SetActive(false);
    }
    void Start()
    {
        instance = this;
        theData = FindObjectOfType<DatabaseManager_cy>();
        inventoryItemList = new List<ItemInfo>();
        
        ItemSlot = Resources.Load<GameObject>("Item/item");
    }
    public void GetAnItem(int itemID, int _count)
    {
        for (int i = 0; i < theData.itemList.Count; i++) //�����ͺ��̽����� ������ �˻�
        {
            if (itemID == theData.itemList[i].itemID)
            {
                GameObject clone = Instantiate(prefab_floating_text, messageTr.position, Quaternion.Euler(Vector3.zero));
                clone.GetComponent<FloatingText>().text.text = theData.itemList[i].itemName + " " + _count + "�� ȹ�� +";
                clone.transform.SetParent(this.transform);

                ItemNumber = itemID;
                ItemCount = _count;

                for (int j = 0; j < inventoryItemList.Count; j++)
                {
                    if (theData.itemList[i].itemType == ItemInfo.ItemType.Equip)
                    {
                        AddItem(EquipSlots, theData.itemList[i].equipType);
                        inventoryItemList.Add(theData.itemList[i]);
                        return;
                    }
                    else
                    {
                        if (inventoryItemList[j].itemID == itemID)
                            inventoryItemList[j].itemCount += _count;
                        else
                        {
                            if (theData.itemList[i].itemType == ItemInfo.ItemType.Consume)
                            {
                                AddItem(ConsumeSlots);
                                inventoryItemList.Add(theData.itemList[i]);
                            }
                            else
                            {
                                AddItem(ETCSlots);
                                inventoryItemList.Add(theData.itemList[i]);
                            }
                        }
                        return;
                    }
                }
                //else
                //{
                //    for (int j = 0; j < inventoryItemList.Count; j++)
                //    {
                //        if (inventoryItemList[j].itemID == ItemNumber)
                //            inventoryItemList[j].itemCount += ItemCount;
                //        else
                //        {
                //            if (theData.itemList[i].itemType == ItemInfo.ItemType.Consume)
                //            {
                //                AddItem(ConsumeSlots);
                //                inventoryItemList.Add(theData.itemList[i]);
                //            }
                //            else
                //            {
                //                AddItem(ETCSlots);
                //                inventoryItemList.Add(theData.itemList[i]);
                //            }
                //        }
                //        return;
                //    }
                //}

                
                inventoryItemList.Add(theData.itemList[i]);
                inventoryItemList[inventoryItemList.Count - 1].itemCount = _count;
                return;
            }
        }
    }
    public void AddItem(GameObject[] ItemTpye)
    {
        GameObject clone = Instantiate(ItemSlot, Vector3.zero, Quaternion.identity);
        clone.transform.SetParent(ItemGetIN(ItemTpye).transform);
        hoverTip = clone.GetComponent<HoverTip>();
        slots = clone.GetComponent<InventorySlot_cy>();
        itemInfo = clone.GetComponent<ItemInfo>();
        if (ItemTpye == ConsumeSlots)
        {
            itemInfo.itemType = ItemInfo.ItemType.Consume;
            itemInfo.equipType = ItemInfo.EquipType.None;
        }
        else
        {
            itemInfo.itemType = ItemInfo.ItemType.ETC;
            itemInfo.equipType = ItemInfo.EquipType.None;
        }
        for (int i = 0; i < theData.itemList.Count; i++) //�����ͺ��̽����� ������ �˻�
        {
            if (ItemNumber == theData.itemList[i].itemID)
            {
                //for (int j = 0; j < inventoryItemList.Count; j++)
                //{
                //    if (inventoryItemList[j].itemID == ItemNumber)
                //    {
                //        inventoryItemList[j].itemCount += ItemCount;
                //        slots.itemCount_Text.text = inventoryItemList[j].itemCount.ToString();
                //    }
                //    else
                //    {
                //        inventoryItemList.Add(theData.itemList[i]);
                //        slots.itemCount_Text.text = inventoryItemList[j].itemCount.ToString();
                //    }
                    
                //}
                hoverTip.titleToShow = theData.itemList[i].itemName;
                hoverTip.tipToShow = theData.itemList[i].itemDescription;
                hoverTip.countToShow = "���� : " + theData.itemList[i].itemCount.ToString() + "��";
                hoverTip.itemToShow = theData.itemList[i].itemIcon;
                slots.icon.sprite = theData.itemList[i].itemIcon;
                slots.itemCount_Text.text = theData.itemList[i].itemCount.ToString();
                
                break;
            }
        }
    }
    public void AddItem(GameObject[] ItemTpye, ItemInfo.EquipType equipType)
    {
        GameObject clone = Instantiate(ItemSlot, Vector3.zero, Quaternion.identity);
        clone.transform.SetParent(ItemGetIN(ItemTpye).transform);
        hoverTip = clone.GetComponent<HoverTip>();
        slots = clone.GetComponent<InventorySlot_cy>();
        itemInfo = clone.GetComponent<ItemInfo>();
        itemInfo.itemType = ItemInfo.ItemType.Equip;
        switch (equipType)
        {
            case ItemInfo.EquipType.Weapon:
                itemInfo.equipType = ItemInfo.EquipType.Weapon;
                break;
            case ItemInfo.EquipType.Helmet:
                itemInfo.equipType = ItemInfo.EquipType.Helmet;
                break;
            case ItemInfo.EquipType.Armor:
                itemInfo.equipType = ItemInfo.EquipType.Armor;
                break;
            case ItemInfo.EquipType.Boots:
                itemInfo.equipType = ItemInfo.EquipType.Boots;
                break;
            case ItemInfo.EquipType.Totem:
                itemInfo.equipType = ItemInfo.EquipType.Totem;
                break;
        }
        
        for (int i = 0; i < theData.itemList.Count; i++) //�����ͺ��̽����� ������ �˻�
        {
            if (ItemNumber == theData.itemList[i].itemID)
            {
                inventoryItemList.Add(theData.itemList[i]);
                hoverTip.titleToShow = theData.itemList[i].itemName;
                hoverTip.tipToShow = theData.itemList[i].itemDescription;
                hoverTip.itemToShow = theData.itemList[i].itemIcon;
                slots.icon.sprite = theData.itemList[i].itemIcon;
                break;
            }
        }
    }
    public GameObject ItemGetIN(GameObject[] ItemTpye)

    {
        GameObject emptyInven = null;
        for (int i = 0; i < ItemTpye.Length; i++)
        {
            if (ItemTpye[i].transform.childCount == 0)
            {
                emptyInven = ItemTpye[i];
                Debug.Log("�θ� ã��");
                break;
            }
        }
        return emptyInven;
    }
}
