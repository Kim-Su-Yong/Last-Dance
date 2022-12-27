using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DatabaseManager : MonoBehaviour
{
    static public DatabaseManager instance;
    
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            DontDestroyOnLoad(this.gameObject);
            instance = this;
        }
    }
    public string[] var_name;
    public float[] var;

    public string[] switch_name;
    public bool[] switches;

    public List<ItemInfo> itemList = new List<ItemInfo>();
    //public List<EquipStat> equipStat = new List<EquipStat>();
    void Start()
    {
        itemList.Add(new ItemInfo(111, "�Ϲ�ü������", "ü���� 10 ȸ�������ִ� ������ ����", ItemInfo.ItemType.Consume, 10));
        itemList.Add(new ItemInfo(112, "���ü������", "ü���� 20 ȸ�������ִ� ������ ����", ItemInfo.ItemType.Consume, 20));
        itemList.Add(new ItemInfo(113, "����ü������", "ü���� 50 ȸ�������ִ� ������ ����", ItemInfo.ItemType.Consume, 50));
        //itemList.Add(new ItemInfo(121, "�Ϲݸ�������", "������ 10 ȸ�������ִ� ������ ����", ItemInfo.ItemType.Consume));
        //itemList.Add(new ItemInfo(122, "��޸�������", "������ 20 ȸ�������ִ� ������ ����", ItemInfo.ItemType.Consume));
        //itemList.Add(new ItemInfo(123, "������������", "������ 50 ȸ�������ִ� ������ ����", ItemInfo.ItemType.Consume));
        itemList.Add(new ItemInfo(211, "�Ϲ� ��", "�Ϲ� ����� ��", ItemInfo.ItemType.Equip, ItemInfo.EquipType.Weapon, 5));
        itemList.Add(new ItemInfo(212, "��� ��", "��� ����� ��", ItemInfo.ItemType.Equip, ItemInfo.EquipType.Weapon, 10));
        itemList.Add(new ItemInfo(213, "���� ��", "���� ����ǰ�", ItemInfo.ItemType.Equip, ItemInfo.EquipType.Weapon, 20));
        itemList.Add(new ItemInfo(214, "�Ϲ� �ϵ�", "�Ϲ� �������� ������", ItemInfo.ItemType.Equip, ItemInfo.EquipType.Weapon, 5));
        itemList.Add(new ItemInfo(215, "��� �ϵ�", "��� �������� ������", ItemInfo.ItemType.Equip, ItemInfo.EquipType.Weapon, 10));
        itemList.Add(new ItemInfo(216, "���� �ϵ�", "���� �������� ������", ItemInfo.ItemType.Equip, ItemInfo.EquipType.Weapon, 20));
        itemList.Add(new ItemInfo(221, "�Ϲ� ����", "�Ϲ� ����", ItemInfo.ItemType.Equip, ItemInfo.EquipType.Helmet, 0, 5));
        itemList.Add(new ItemInfo(222, "��� ����", "��� ����", ItemInfo.ItemType.Equip, ItemInfo.EquipType.Helmet, 0, 10));
        itemList.Add(new ItemInfo(223, "���� ����", "���� ����", ItemInfo.ItemType.Equip, ItemInfo.EquipType.Helmet, 0, 20));
        itemList.Add(new ItemInfo(231, "�Ϲ� ����", "�Ϲ� ����", ItemInfo.ItemType.Equip, ItemInfo.EquipType.Armor, 0, 5));
        itemList.Add(new ItemInfo(232, "��� ����", "��� ����", ItemInfo.ItemType.Equip, ItemInfo.EquipType.Armor, 0, 10));
        itemList.Add(new ItemInfo(233, "���� ����", "���� ����", ItemInfo.ItemType.Equip, ItemInfo.EquipType.Armor, 0, 20));
        itemList.Add(new ItemInfo(241, "�Ϲ� �Ź�", "�Ϲ� �Ź�", ItemInfo.ItemType.Equip, ItemInfo.EquipType.Boots, 0, 0, 0, 1f));
        itemList.Add(new ItemInfo(242, "��� �Ź�", "��� �Ź�", ItemInfo.ItemType.Equip, ItemInfo.EquipType.Boots, 0, 0, 0, 2f));
        itemList.Add(new ItemInfo(243, "���� �Ź�", "���� �Ź�", ItemInfo.ItemType.Equip, ItemInfo.EquipType.Boots, 0, 0, 0, 4f));
        itemList.Add(new ItemInfo(251, "�Ϲ� �尩", "�Ϲ� �尩", ItemInfo.ItemType.Equip, ItemInfo.EquipType.Gloves, 3));
        itemList.Add(new ItemInfo(252, "��� �尩", "��� �尩", ItemInfo.ItemType.Equip, ItemInfo.EquipType.Gloves, 5));
        itemList.Add(new ItemInfo(253, "���� �尩", "���� �尩", ItemInfo.ItemType.Equip, ItemInfo.EquipType.Gloves, 10));
        itemList.Add(new ItemInfo(261, "�Ϲ� ����", "�Ϲ� ����", ItemInfo.ItemType.Equip, ItemInfo.EquipType.Totem, 0, 0, 50));
        itemList.Add(new ItemInfo(262, "��� ����", "��� ����", ItemInfo.ItemType.Equip, ItemInfo.EquipType.Totem, 0, 0, 100));
        itemList.Add(new ItemInfo(263, "���� ����", "���� ����", ItemInfo.ItemType.Equip, ItemInfo.EquipType.Totem, 0, 0, 200));
        itemList.Add(new ItemInfo(264, "�Ϲ� �����", "�Ϲ� �����", ItemInfo.ItemType.Equip, ItemInfo.EquipType.Totem2, 0, 0, 50));
        itemList.Add(new ItemInfo(265, "��� �����", "��� �����", ItemInfo.ItemType.Equip, ItemInfo.EquipType.Totem2, 0, 0, 100));
        itemList.Add(new ItemInfo(266, "���� �����", "���� �����", ItemInfo.ItemType.Equip, ItemInfo.EquipType.Totem2, 0, 0, 200));
        itemList.Add(new ItemInfo(311, "����", "�ڿ��� ���̹���", ItemInfo.ItemType.ETC));
        itemList.Add(new ItemInfo(312, "�ذ�", "���̷����� �ذ�", ItemInfo.ItemType.ETC));
        itemList.Add(new ItemInfo(313, "����", "�ǽ����� ���� ����", ItemInfo.ItemType.ETC));
        itemList.Add(new ItemInfo(314, "�η縶��", "��� �������ڰ� �����ִ� �η縶��", ItemInfo.ItemType.ETC));
    }
}
