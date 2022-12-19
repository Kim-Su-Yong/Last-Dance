using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DatabaseManager_cy : MonoBehaviour
{
    static public DatabaseManager_cy instance;
    
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
    
    public List<ItemInfo> itemList = new List<ItemInfo>();
    
    void Start()
    {
        itemList.Add(new ItemInfo(111, "���� ����", "ü���� 50 ȸ�������ִ� ������ ����", ItemInfo.ItemType.Consume));
        itemList.Add(new ItemInfo(121, "�Ķ� ����", "������ 15 ȸ�������ִ� ������ ����", ItemInfo.ItemType.Consume));
        itemList.Add(new ItemInfo(112, "���� ���� ����", "ü���� 350 ȸ�������ִ� ������ ���� ����", ItemInfo.ItemType.Consume));
        itemList.Add(new ItemInfo(122, "���� �Ķ� ����", "������ 80 ȸ�������ִ� ������ ���� ����", ItemInfo.ItemType.Consume));
        itemList.Add(new ItemInfo(211, "��", "�⺻���Դϴ�", ItemInfo.ItemType.Equip, ItemInfo.EquipType.Weapon));
        itemList.Add(new ItemInfo(212, "������", "�������Դϴ�", ItemInfo.ItemType.Equip, ItemInfo.EquipType.Weapon));
        itemList.Add(new ItemInfo(221, "����", "�⺻�����Դϴ�", ItemInfo.ItemType.Equip, ItemInfo.EquipType.Helmet));
        itemList.Add(new ItemInfo(222, "��������", "���������Դϴ�", ItemInfo.ItemType.Equip, ItemInfo.EquipType.Helmet));
        itemList.Add(new ItemInfo(231, "����", "�⺻�����Դϴ�", ItemInfo.ItemType.Equip, ItemInfo.EquipType.Armor));
        itemList.Add(new ItemInfo(232, "��������", "���������Դϴ�", ItemInfo.ItemType.Equip, ItemInfo.EquipType.Armor));
        itemList.Add(new ItemInfo(241, "�Ź�", "�⺻�Ź��Դϴ�", ItemInfo.ItemType.Equip, ItemInfo.EquipType.Boots));
        itemList.Add(new ItemInfo(251, "�Ǽ��縮", "�⺻�Ǽ��縮�Դϴ�", ItemInfo.ItemType.Equip, ItemInfo.EquipType.Totem));
        itemList.Add(new ItemInfo(252, "�����Ǽ��縮", "�����Ǽ��縮�Դϴ�", ItemInfo.ItemType.Equip, ItemInfo.EquipType.Totem));
        itemList.Add(new ItemInfo(311, "����Ʈ������", "����Ʈ�������Դϴ�", ItemInfo.ItemType.ETC));
        itemList.Add(new ItemInfo(312, "����Ʈ������2", "����Ʈ�������Դϴ�2", ItemInfo.ItemType.ETC));
    }
}
