using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class ItemInfo : MonoBehaviour
{
    //public static ItemInfo itemInfo;
    public int itemID; //아이템의 고유 ID값. 중복 불가
    public string itemName; //아이템의 이름. 중복 가능
    public string itemDescription; //아이템 설명
    public int Atk; //공격력
    public int Def; //방어력
    public int AddHp; //체력
    public float Speed; //이동속도
    public int itemCount; //소지 갯수
    public Sprite itemIcon; //아이템의 아이콘.
    // 아이템 1차 분류
    public enum ItemType { Equip, Consume, ETC };
    // 장비 2차 분류
    public enum EquipType
    {
        None, Weapon, Helmet, Armor, Boots, Gloves, Totem
    }
       
    public ItemType itemType;
    public EquipType equipType;

    //public List<EquipStat> equipStat = new List<EquipStat>();

    public ItemInfo(int _itemID, string _itemName, string _itemDes, ItemType _itemType, int _AddHp = 0, int _itemCount = 1)
    {
        itemID = _itemID;
        itemName = _itemName;
        itemDescription = _itemDes;
        itemType = _itemType;
        AddHp = _AddHp;
        itemCount = _itemCount;
        itemIcon = Resources.Load("ItemImages/" + _itemID.ToString(), typeof(Sprite)) as Sprite;
    }
    public ItemInfo(int _itemID, string _itemName, string _itemDes, ItemType _itemType, EquipType _equipType,
        int _Atk = 0, int _Def = 0, int _AddHp = 0, float _Speed = 0, int _itemCount = 1)
    {
        itemID = _itemID;
        itemName = _itemName;
        itemDescription = _itemDes;
        itemType = _itemType;
        equipType = _equipType;
        Atk = _Atk; Def = _Def; AddHp = _AddHp; Speed = _Speed;
        itemCount = _itemCount;
        itemIcon = Resources.Load("ItemImages/" + _itemID.ToString(), typeof(Sprite)) as Sprite;
    }

    //// 아이템 2차 분류
    //public enum ItemType
    //{
    //    Goggles     = 0,
    //    Helmet      = 1,
    //    Totem       = 2,
    //    Weapon      = 3,
    //    Armor       = 4,
    //    Gloves      = 5,
    //    Shield      = 6,
    //    Boots       = 7,
    //    Backpack    = 8,
    //    Potion      = 9,
    //    etc         = 11
    //};
    public bool Use()
    {
        bool isUsed = false;
        return isUsed;
    }
}
