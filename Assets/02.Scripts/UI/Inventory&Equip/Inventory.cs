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
    
    public int ItemNumber; //아이템 아이디
    public int ItemCount; //아이템 수량

    [SerializeField]
    private InventorySlot slots; //인벤토리 슬롯들
    
    public List<ItemInfo> inventoryItemList; //플레이어가 소지한 아이템 리스트
    public List<ItemInfo> equipmentItemList; //장비창에 들어있는 아이템 리스트

    public GameObject ItemSlot; //인벤토리 하위에 들어갈 아이템 정보가 담긴 오브젝트
    public GameObject theItem;

    public GameObject prefab_floating_text;
    public Transform messageTr;

    public GameObject clone;

    [Header("PlayerStat관련")]
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
        AtkText.text = "공격력 : " + PlayerStat.instance.atk.ToString();
        DefText.text = "방어력 : " + PlayerStat.instance.def.ToString();
        MaxhpText.text = "최대체력 : " + PlayerStat.instance.maxHP.ToString();
        SpeedText.text = "이동속도 : " + PlayerStat.instance.speed.ToString();
    }
    public void GetAnItem(int itemID, int _count)
    {
        for (int i = 0; i < theData.itemList.Count; i++) //데이터베이스에서 아이템 검색
        {
            if (itemID == theData.itemList[i].itemID)
            {
                clone = Instantiate(prefab_floating_text, messageTr.position, Quaternion.Euler(Vector3.zero));
                clone.GetComponent<FloatingText>().text.text = theData.itemList[i].itemName + " " + _count + "개 획득 +";
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

    //소비, 기타템 인벤토리에 추가하는 함수
    public void AddItem(GameObject[] ItemType)
    {
        bool inInventory = false;
        for (int i = 0; i < inventoryItemList.Count; i++)
        {
            if (ItemNumber == inventoryItemList[i].GetComponent<ItemInfo>().itemID)
            {
                inInventory = true;
                if (inInventory == true) //아이템 중복시
                {
                    inventoryItemList[i].GetComponent<ItemInfo>().itemCount += ItemCount;
                    inventoryItemList[i].GetComponent<InventorySlot>().itemCount += ItemCount;
                    inventoryItemList[i].GetComponent<InventorySlot>().itemCount_Text.text =
                        inventoryItemList[i].GetComponent<InventorySlot>().itemCount.ToString();
                    inventoryItemList[i].GetComponent<HoverTip>().itemCount += ItemCount;
                    inventoryItemList[i].GetComponent<HoverTip>().countToShow =
                        "수량 : " + inventoryItemList[i].GetComponent<HoverTip>().itemCount.ToString() + "개";
                    break;

                }
            }
        }
        if(inInventory==false)
        {
            ItemCreate(ItemType);
        }
    }

    public void ItemCreate(GameObject[] ItemType) //아이템 중복 안될시 생성하는 함수
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
        for (int i = 0; i < theData.itemList.Count; i++) //데이터베이스에서 아이템 검색
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
                hoverTip.countToShow = "수량 : " + hoverTip.itemCount.ToString() + "개";
                hoverTip.itemToShow = theData.itemList[i].itemIcon;
                slots.icon.sprite = theData.itemList[i].itemIcon;
                slots.itemCount = theData.itemList[i].itemCount;
                slots.itemCount_Text.text = slots.itemCount.ToString();
                break;
            }
        }
    }

    //장비템 인벤토리에 추가하는 함수
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
        for (int i = 0; i < theData.itemList.Count; i++) //데이터베이스에서 아이템 검색
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
                    hoverTip.countToShow = "공격력 + " + theData.itemList[i].Atk.ToString();
                }
                else if (theData.itemList[i].Def != 0)
                {
                    itemInfo.Def = theData.itemList[i].Def;
                    hoverTip.countToShow = "방어력 + " + theData.itemList[i].Def.ToString();
                }
                else if (theData.itemList[i].AddHp != 0)
                {
                    itemInfo.AddHp = theData.itemList[i].AddHp;
                    hoverTip.countToShow = "최대체력 + " + theData.itemList[i].AddHp.ToString();
                }
                else if (theData.itemList[i].Speed != 0)
                {
                    itemInfo.Speed = theData.itemList[i].Speed;
                    hoverTip.countToShow = "이동속도 + " + theData.itemList[i].Speed.ToString();
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
                Debug.Log("부모 찾음");
                break;
            }
            else if(ItemType[i].transform.childCount == 1 && !ItemType[i].transform.gameObject)
            {
                //낼 추가 예정
            }
        }
        return emptyInven;
    }
    //public void Sell()
    //{
    //    inventoryItemList.RemoveAt(0); //아이콘이 남아있음
    //    obj1.transform.GetChild(0).gameObject.SetActive(false); //판매되는것처럼 보이는데 그 칸에 아이템이 안들어감
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
