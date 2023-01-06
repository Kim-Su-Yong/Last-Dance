using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public static Inventory instance;

    private DatabaseManager theData;
    private SoundManager theSound;

    private HoverTip hoverTip;
    private ItemInfo itemInfo;

    public GameObject[] EquipSlots;
    public GameObject[] ConsumeSlots;
    public GameObject[] ETCSlots;
    
    public int ItemNumber; //������ ���̵�
    public int ItemCount; //������ ����

    [SerializeField]
    private InventorySlot slots; //�κ��丮 ���Ե�
    
    public List<ItemInfo> inventoryItemList; //�÷��̾ ������ ������ ����Ʈ
    public List<ItemInfo> equipmentItemList; //���â�� ����ִ� ������ ����Ʈ

    public GameObject ItemSlot; //�κ��丮 ������ �� ������ ������ ��� ������Ʈ
    public GameObject theItem;

    public GameObject prefab_floating_text;
    public Transform messageTr;

    public GameObject clone;

    [Header("PlayerStat����")]
    public Text LvText;
    public Text AtkText;
    public Text DefText;
    public Text MaxhpText;
    public Text SpeedText;

    private void Awake()
    {
        EquipSlots = GameObject.FindGameObjectsWithTag("Equip");
        ConsumeSlots = GameObject.FindGameObjectsWithTag("Consume");
        ETCSlots = GameObject.FindGameObjectsWithTag("ETC");
        ConsumeSlots[0].transform.parent.gameObject.SetActive(false);
        ETCSlots[0].transform.parent.gameObject.SetActive(false);
        ItemSlot = Resources.Load<GameObject>("Item/item");
    }
    void Start()
    {
        instance = this;
        theData = FindObjectOfType<DatabaseManager>();
        theSound = FindObjectOfType<SoundManager>();
        inventoryItemList = new List<ItemInfo>();
        equipmentItemList = new List<ItemInfo>();
    }
    private void Update()
    {
        LvText.text = "LV : " + PlayerStat.instance.character_Lv.ToString();
        AtkText.text = "���ݷ� : " + PlayerStat.instance.atk.ToString();
        DefText.text = "���� : " + PlayerStat.instance.def.ToString();
        MaxhpText.text = "�ִ�ü�� : " + PlayerStat.instance.maxHP.ToString();
        SpeedText.text = "�̵��ӵ� : " + PlayerStat.instance.speed.ToString();
    }
    public void GetAnItem(int itemID, int _count)
    {
        for (int i = 0; i < theData.itemList.Count; i++) //�����ͺ��̽����� ������ �˻�
        {
            if (itemID == theData.itemList[i].itemID)
            {
                clone = Instantiate(prefab_floating_text, messageTr.position, Quaternion.Euler(Vector3.zero));
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
    public void AddItem(GameObject[] ItemType)
    {
        bool inInventory = false;
        for (int i = 0; i < inventoryItemList.Count; i++)
        {
            if (ItemNumber == inventoryItemList[i].GetComponent<ItemInfo>().itemID)
            {
                inInventory = true;
                if (inInventory == true) //������ �ߺ���
                {
                    inventoryItemList[i].GetComponent<ItemInfo>().itemCount += ItemCount;
                    inventoryItemList[i].GetComponent<InventorySlot>().itemCount += ItemCount;
                    inventoryItemList[i].GetComponent<InventorySlot>().itemCount_Text.text =
                        inventoryItemList[i].GetComponent<InventorySlot>().itemCount.ToString();
                    inventoryItemList[i].GetComponent<HoverTip>().itemCount += ItemCount;
                    inventoryItemList[i].GetComponent<HoverTip>().countToShow =
                        "���� : " + inventoryItemList[i].GetComponent<HoverTip>().itemCount.ToString() + "��";
                    break;

                }
            }
        }
        if(inInventory==false)
        {
            ItemCreate(ItemType);
        }
    }

    public void ItemCreate(GameObject[] ItemType) //������ �ߺ� �ȵɽ� �����ϴ� �Լ�
    {
        clone = Instantiate(ItemSlot, Vector3.zero, Quaternion.identity);
        clone.transform.SetParent(ItemGetIN(ItemType).transform);
        clone.tag = "Consume";
        hoverTip = clone.GetComponent<HoverTip>();
        slots = clone.GetComponent<InventorySlot>();
        itemInfo = clone.GetComponent<ItemInfo>();
        inventoryItemList.Add(clone.GetComponent<ItemInfo>());
        if (ItemType == ConsumeSlots)
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
                itemInfo.itemCount = theData.itemList[i].itemCount;
                itemInfo.itemIcon = theData.itemList[i].itemIcon;
                itemInfo.AddHp = theData.itemList[i].AddHp;
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
    public void AddItem(GameObject[] ItemType, ItemInfo.EquipType equipType)
    {
        clone = Instantiate(ItemSlot, Vector3.zero, Quaternion.identity);
        clone.transform.SetParent(ItemGetIN(ItemType).transform);
        clone.tag = "Equip";
        hoverTip = clone.GetComponent<HoverTip>();
        slots = clone.GetComponent<InventorySlot>();
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
            case ItemInfo.EquipType.Gloves:
                itemInfo.equipType = ItemInfo.EquipType.Gloves;
                break;
            case ItemInfo.EquipType.Totem:
                itemInfo.equipType = ItemInfo.EquipType.Totem;
                break;
            case ItemInfo.EquipType.Totem2:
                itemInfo.equipType = ItemInfo.EquipType.Totem2;
                break;
        }
        inventoryItemList.Add(clone.GetComponent<ItemInfo>());
        for (int i = 0; i < theData.itemList.Count; i++) //�����ͺ��̽����� ������ �˻�
        {
            if (ItemNumber == theData.itemList[i].itemID)
            {
                itemInfo.itemID = theData.itemList[i].itemID;
                itemInfo.itemIcon = theData.itemList[i].itemIcon;
                hoverTip.titleToShow = theData.itemList[i].itemName;
                hoverTip.tipToShow = theData.itemList[i].itemDescription;
                hoverTip.itemToShow = theData.itemList[i].itemIcon;
                slots.icon.sprite = theData.itemList[i].itemIcon;
                if (theData.itemList[i].Atk != 0)
                {
                    itemInfo.Atk = theData.itemList[i].Atk;
                    hoverTip.countToShow = "���ݷ� + " + theData.itemList[i].Atk.ToString();
                }
                else if (theData.itemList[i].Def != 0)
                {
                    itemInfo.Def = theData.itemList[i].Def;
                    hoverTip.countToShow = "���� + " + theData.itemList[i].Def.ToString();
                }
                else if (theData.itemList[i].AddHp != 0)
                {
                    itemInfo.AddHp = theData.itemList[i].AddHp;
                    hoverTip.countToShow = "�ִ�ü�� + " + theData.itemList[i].AddHp.ToString();
                }
                else if (theData.itemList[i].Speed != 0)
                {
                    itemInfo.Speed = theData.itemList[i].Speed;
                    hoverTip.countToShow = "�̵��ӵ� + " + theData.itemList[i].Speed.ToString();
                }
                break;
            }
        }
    }

    public GameObject ItemGetIN(GameObject[] ItemType)
    {
        GameObject emptyInven = null;
        for (int i = 0; i < ItemType.Length; i++)
        {
            if (ItemType[i].transform.childCount == 0)
            {
                emptyInven = ItemType[i];
                Debug.Log("�θ� ã��");
                break;
            }
            else if(ItemType[i].transform.childCount == 1 && !ItemType[i].transform.gameObject)
            {
                //�� �߰� ����
            }
        }
        return emptyInven;
    }
    //public void Sell()
    //{
    //    inventoryItemList.RemoveAt(0); //�������� ��������
    //    obj1.transform.GetChild(0).gameObject.SetActive(false); //�ǸŵǴ°�ó�� ���̴µ� �� ĭ�� �������� �ȵ�
    //}
    public List<ItemInfo> SaveItem()
    {
        return inventoryItemList;
    }
    public void LoadItem(List<ItemInfo> _itemList)
    {
        inventoryItemList = _itemList;
    }
}
