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
    
    [SerializeField]
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
                if (theData.itemList[i].itemType == ItemInfo.ItemType.Equip)
                {
                    AddItem(EquipSlots, theData.itemList[i].equipType);
                }
                else if (theData.itemList[i].itemType == ItemInfo.ItemType.Consume)
                {
                    AddItem(ConsumeSlots);
                }
                else
                {
                    AddItem(ETCSlots);
                }
                return;
            }
        }
    }

    //�Һ�, ��Ÿ�� �κ��丮�� �߰��ϴ� �Լ�
    public void AddItem(GameObject[] ItemTpye)
    {
        bool inInventory = false;
        for (int i = 0; i < inventoryItemList.Count; i++)
        {
            if (ItemNumber == inventoryItemList[i].GetComponent<ItemInfo>().itemID)
            {
                inInventory = true;
                if (inInventory == true)
                {
                    inventoryItemList[i].GetComponent<InventorySlot_cy>().itemCount += ItemCount;
                    inventoryItemList[i].GetComponent<InventorySlot_cy>().itemCount_Text.text =
                        inventoryItemList[i].GetComponent<InventorySlot_cy>().itemCount.ToString();
                    inventoryItemList[i].GetComponent<HoverTip>().itemCount += ItemCount;
                    inventoryItemList[i].GetComponent<HoverTip>().countToShow =
                        "���� : " + inventoryItemList[i].GetComponent<HoverTip>().itemCount.ToString() + "��";
                    break;

                }
            }
        }
        if(inInventory==false)
        {
            ItemCreate(ItemTpye);
        }
    }

    private void ItemCreate(GameObject[] ItemTpye)
    {
        GameObject clone = Instantiate(ItemSlot, Vector3.zero, Quaternion.identity);
        clone.transform.SetParent(ItemGetIN(ItemTpye).transform);
        hoverTip = clone.GetComponent<HoverTip>();
        slots = clone.GetComponent<InventorySlot_cy>();
        itemInfo = clone.GetComponent<ItemInfo>();
        inventoryItemList.Add(clone.GetComponent<ItemInfo>());
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
                itemInfo.itemID = theData.itemList[i].itemID;
                hoverTip.titleToShow = theData.itemList[i].itemName;
                hoverTip.tipToShow = theData.itemList[i].itemDescription;
                hoverTip.itemCount = theData.itemList[i].itemCount;
                hoverTip.countToShow = "���� : " + hoverTip.itemCount.ToString() + "��";
                hoverTip.itemToShow = theData.itemList[i].itemIcon;
                slots.icon.sprite = theData.itemList[i].itemIcon;
                slots.itemCount = theData.itemList[i].itemCount;
                slots.itemCount_Text.text = slots.itemCount.ToString();
                break;
            }
        }
    }

    //����� �κ��丮�� �߰��ϴ� �Լ�
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
        inventoryItemList.Add(clone.GetComponent<ItemInfo>());
        for (int i = 0; i < theData.itemList.Count; i++) //�����ͺ��̽����� ������ �˻�
        {
            if (ItemNumber == theData.itemList[i].itemID)
            {
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

    public GameObject[] Test(GameObject[] ItemTpye)
    {
        GameObject[] emptyInven = null;
        GameObject test = null;
        for (int i = 0; i < ItemTpye.Length; i++)
        {
            if (ItemTpye[i].transform.childCount != 0)
            {
                test = ItemTpye[i];
                Debug.Log("�θ� ã��");
            }
        }
        return emptyInven;
    }
}
