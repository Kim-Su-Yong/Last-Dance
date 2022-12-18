using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory_cy : MonoBehaviour
{
    public static Inventory_cy instance;

    private DatabaseManager_cy theData;

    private HoverTip hoverTip;

    private List<Item_cy> InventoryItem;
    private List<Item_cy> Item_Consume;
    private List<Item_cy> Item_Equip;
    private List<Item_cy> Item_ETC;

    //test
    public GameObject[] test2;
    public GameObject test;
    public int ItemNumber;

    [SerializeField]
    private InventorySlot_cy slots; //�κ��丮 ���Ե�
    public GameObject ItemSlot; //�κ��丮 ������ �� ������ ������ ��� ������Ʈ

    public GameObject prefab_floating_text;
    public Transform messageTr;

    private void Awake()
    {
        
    }

    void Start()
    {
        instance = this;
        theData = FindObjectOfType<DatabaseManager_cy>();
        InventoryItem = new List<Item_cy>();
        Item_Consume = new List<Item_cy>();
        Item_Equip = new List<Item_cy>();
        Item_ETC = new List<Item_cy>();

        slots = Resources.Load<GameObject>("Item/item").GetComponent<InventorySlot_cy>();
        ItemSlot = Resources.Load<GameObject>("Item/item");


    }


    void Update()
    {
        
    }

    public void GetAnItem(int itemID, int _count)
    {
        for (int i = 0; i < theData.itemList.Count; i++) //�����ͺ��̽����� ������ �˻�
        {
            if (itemID == theData.itemList[i].itemID)
            {
                ItemNumber = itemID;
                AddItem();
                GameObject clone = Instantiate(prefab_floating_text, messageTr.position, Quaternion.Euler(Vector3.zero));
                clone.GetComponent<FloatingText>().text.text = theData.itemList[i].itemName + " " + _count + "�� ȹ�� +";
                clone.transform.SetParent(this.transform);

                for (int j = 0; j < InventoryItem.Count; j++) //����ǰ�� ���� �������� �ִ��� �˻�
                {
                    if (InventoryItem[j].itemID == itemID)
                    {
                        if (InventoryItem[j].itemType == Item_cy.ItemType.Equip)
                        {
                            InventoryItem.Add(theData.itemList[i]);
                        }
                        else  //����ǰ�� ���� �������� �ִ� -> ������ ����������
                        {
                            InventoryItem[j].itemCount += _count;
                        }
                        return;
                    }
                }
                InventoryItem.Add(theData.itemList[i]); //����ǰ�� �ش� ������ �߰�
                InventoryItem[InventoryItem.Count - 1].itemCount = _count;
                return;
            }
        }
    }

    public void AddItem()
    {
        GameObject clone = Instantiate(ItemSlot, Vector3.zero, Quaternion.identity);
        clone.transform.SetParent(ItemGetIN().transform);
        hoverTip = clone.GetComponent<HoverTip>();
        slots = clone.GetComponent<InventorySlot_cy>();

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

    public void ShowItem()
    {
        for (int i = 0; i < InventoryItem.Count; i++)
        {
            if (Item_cy.ItemType.Consume == InventoryItem[i].itemType)
                Item_Consume.Add(InventoryItem[i]);
            else if (Item_cy.ItemType.Equip == InventoryItem[i].itemType)
                Item_Equip.Add(InventoryItem[i]);
            else
                Item_ETC.Add(InventoryItem[i]);
        }
    }

    public GameObject ItemGetIN()
    {
        GameObject emptyInven = null;
        for (int i = 0; i < test2.Length; i++)
        {
            if (test2[i].transform.childCount == 0)
            {
                emptyInven = test2[i];
                Debug.Log("�θ� ã��");
                break;
            }
        }
        return emptyInven;
    }

}
