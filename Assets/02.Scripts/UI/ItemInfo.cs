using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class ItemInfo : MonoBehaviour
{
    //public static ItemInfo itemInfo;
    public int itemID; //�������� ���� ID��. �ߺ� �Ұ�
    public string itemName; //�������� �̸�. �ߺ� ����
    public string itemDescription; //������ ����
    public int Atk; //���ݷ�
    public int Def; //����
    public int AddHp; //ü��
    public float Speed; //�̵��ӵ�
    public int itemCount; //���� ����
    public Sprite itemIcon; //�������� ������.
    // ������ 1�� �з�
    public enum ItemType { Equip, Consume, ETC };
    // ��� 2�� �з�
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

    //// ������ 2�� �з�
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
