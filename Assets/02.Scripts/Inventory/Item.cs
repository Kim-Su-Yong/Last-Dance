using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Item
{
    public int itemID; //아이템의 고유 ID값. 중복 불가
    public string itemName; //아이템의 이름. 중복 가능
    public string itemDescription; //아이템 설명
    public int itemCount; //소지 갯수
    public Sprite itemIcon; //아이템의 아이콘.
    public ItemType itemType;
    public enum ItemType
    {
        Use,
        Equip,
        Quest,
        ETC
    }
    public Item(int _itemID, string _itemName, string _itemDes, ItemType _itemType, int _itemCount = 1)
    {
        itemID = _itemID;
        itemName = _itemName;
        itemDescription = _itemDes;
        itemType = _itemType;
        itemCount = _itemCount;
        itemIcon = Resources.Load("ItemIcon/" + _itemID.ToString(), typeof(Sprite)) as Sprite;
    }
    void Start()
    {
        
    }
    void Update()
    {
        
    }
}
