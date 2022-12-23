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
        //itemList.Add(new ItemInfo(111, "���� ����", "ü���� 50 ȸ�������ִ� ������ ����", ItemInfo.ItemType.Consume));
        //itemList.Add(new ItemInfo(121, "�Ķ� ����", "������ 15 ȸ�������ִ� ������ ����", ItemInfo.ItemType.Consume));
        //itemList.Add(new ItemInfo(112, "���� ���� ����", "ü���� 350 ȸ�������ִ� ������ ���� ����", ItemInfo.ItemType.Consume));
        //itemList.Add(new ItemInfo(122, "���� �Ķ� ����", "������ 80 ȸ�������ִ� ������ ���� ����", ItemInfo.ItemType.Consume));
        //itemList.Add(new ItemInfo(211, "��", "�⺻���Դϴ�", ItemInfo.ItemType.Equip, ItemInfo.EquipType.Weapon));
        //itemList.Add(new ItemInfo(212, "������", "�������Դϴ�", ItemInfo.ItemType.Equip, ItemInfo.EquipType.Weapon));
        //itemList.Add(new ItemInfo(221, "����", "�⺻�����Դϴ�", ItemInfo.ItemType.Equip, ItemInfo.EquipType.Helmet));
        //itemList.Add(new ItemInfo(222, "��������", "���������Դϴ�", ItemInfo.ItemType.Equip, ItemInfo.EquipType.Helmet));
        //itemList.Add(new ItemInfo(231, "����", "�⺻�����Դϴ�", ItemInfo.ItemType.Equip, ItemInfo.EquipType.Armor));
        //itemList.Add(new ItemInfo(232, "��������", "���������Դϴ�", ItemInfo.ItemType.Equip, ItemInfo.EquipType.Armor));
        //itemList.Add(new ItemInfo(241, "�Ź�", "�⺻�Ź��Դϴ�", ItemInfo.ItemType.Equip, ItemInfo.EquipType.Boots));
        //itemList.Add(new ItemInfo(251, "�Ǽ��縮", "�⺻�Ǽ��縮�Դϴ�", ItemInfo.ItemType.Equip, ItemInfo.EquipType.Totem));
        //itemList.Add(new ItemInfo(252, "�����Ǽ��縮", "�����Ǽ��縮�Դϴ�", ItemInfo.ItemType.Equip, ItemInfo.EquipType.Totem));
        //itemList.Add(new ItemInfo(311, "����Ʈ������", "����Ʈ�������Դϴ�", ItemInfo.ItemType.ETC));
        //itemList.Add(new ItemInfo(312, "����Ʈ������2", "����Ʈ�������Դϴ�2", ItemInfo.ItemType.ETC));

        itemList.Add(new ItemInfo(111, "�Ϲ�ü������", "ü���� 50 ȸ�������ִ� ������ ����", ItemInfo.ItemType.Consume));
        itemList.Add(new ItemInfo(112, "���ü������", "ü���� 150 ȸ�������ִ� ������ ����", ItemInfo.ItemType.Consume));
        itemList.Add(new ItemInfo(113, "����ü������", "ü���� 300 ȸ�������ִ� ������ ����", ItemInfo.ItemType.Consume));
        itemList.Add(new ItemInfo(121, "�Ϲݸ�������", "������ 50 ȸ�������ִ� ������ ����", ItemInfo.ItemType.Consume));
        itemList.Add(new ItemInfo(122, "��޸�������", "������ 150 ȸ�������ִ� ������ ����", ItemInfo.ItemType.Consume));
        itemList.Add(new ItemInfo(123, "������������", "������ 300 ȸ�������ִ� ������ ����", ItemInfo.ItemType.Consume));
        itemList.Add(new ItemInfo(211, "�Ϲ� ��", "�Ϲ� ����� ��", ItemInfo.ItemType.Equip, ItemInfo.EquipType.Weapon));
        itemList.Add(new ItemInfo(212, "��� ��", "��� ����� ��", ItemInfo.ItemType.Equip, ItemInfo.EquipType.Weapon));
        itemList.Add(new ItemInfo(213, "���� ��", "���� ����ǰ�", ItemInfo.ItemType.Equip, ItemInfo.EquipType.Weapon));
        itemList.Add(new ItemInfo(214, "�Ϲ� �ϵ�", "�Ϲ� �������� ������", ItemInfo.ItemType.Equip, ItemInfo.EquipType.Weapon));
        itemList.Add(new ItemInfo(215, "��� �ϵ�", "��� �������� ������", ItemInfo.ItemType.Equip, ItemInfo.EquipType.Weapon));
        itemList.Add(new ItemInfo(216, "���� �ϵ�", "���� �������� ������", ItemInfo.ItemType.Equip, ItemInfo.EquipType.Weapon));
        itemList.Add(new ItemInfo(221, "�Ϲ� ����", "�Ϲ� ����", ItemInfo.ItemType.Equip, ItemInfo.EquipType.Helmet));
        itemList.Add(new ItemInfo(222, "��� ����", "��� ����", ItemInfo.ItemType.Equip, ItemInfo.EquipType.Helmet));
        itemList.Add(new ItemInfo(223, "���� ����", "���� ����", ItemInfo.ItemType.Equip, ItemInfo.EquipType.Helmet));
        itemList.Add(new ItemInfo(231, "�Ϲ� ����", "�Ϲ� ����", ItemInfo.ItemType.Equip, ItemInfo.EquipType.Armor));
        itemList.Add(new ItemInfo(232, "��� ����", "��� ����", ItemInfo.ItemType.Equip, ItemInfo.EquipType.Armor));
        itemList.Add(new ItemInfo(233, "���� ����", "���� ����", ItemInfo.ItemType.Equip, ItemInfo.EquipType.Armor));
        itemList.Add(new ItemInfo(241, "�Ϲ� �Ź�", "�Ϲ� �Ź�", ItemInfo.ItemType.Equip, ItemInfo.EquipType.Boots));
        itemList.Add(new ItemInfo(242, "��� �Ź�", "��� �Ź�", ItemInfo.ItemType.Equip, ItemInfo.EquipType.Boots));
        itemList.Add(new ItemInfo(243, "���� �Ź�", "���� �Ź�", ItemInfo.ItemType.Equip, ItemInfo.EquipType.Boots));
        itemList.Add(new ItemInfo(251, "�Ϲ� �尩", "�Ϲ� �尩", ItemInfo.ItemType.Equip, ItemInfo.EquipType.Gloves));
        itemList.Add(new ItemInfo(252, "��� �尩", "��� �尩", ItemInfo.ItemType.Equip, ItemInfo.EquipType.Gloves));
        itemList.Add(new ItemInfo(253, "���� �尩", "���� �尩", ItemInfo.ItemType.Equip, ItemInfo.EquipType.Gloves));
        itemList.Add(new ItemInfo(261, "�Ϲ� ����", "�Ϲ� ����", ItemInfo.ItemType.Equip, ItemInfo.EquipType.Totem));
        itemList.Add(new ItemInfo(262, "��� ����", "��� ����", ItemInfo.ItemType.Equip, ItemInfo.EquipType.Totem));
        itemList.Add(new ItemInfo(263, "���� ����", "���� ����", ItemInfo.ItemType.Equip, ItemInfo.EquipType.Totem));
        itemList.Add(new ItemInfo(264, "�Ϲ� �����", "�Ϲ� �����", ItemInfo.ItemType.Equip, ItemInfo.EquipType.Totem));
        itemList.Add(new ItemInfo(265, "��� �����", "��� �����", ItemInfo.ItemType.Equip, ItemInfo.EquipType.Totem));
        itemList.Add(new ItemInfo(266, "���� �����", "���� �����", ItemInfo.ItemType.Equip, ItemInfo.EquipType.Totem));
        itemList.Add(new ItemInfo(311, "����", "�ڿ��� ���̹���", ItemInfo.ItemType.ETC));
        itemList.Add(new ItemInfo(312, "�ذ�", "���̷����� �ذ�", ItemInfo.ItemType.ETC));
        itemList.Add(new ItemInfo(313, "����", "�ǽ����� ���� ����", ItemInfo.ItemType.ETC));
        itemList.Add(new ItemInfo(314, "�η縶��", "��� �������ڰ� �����ִ� �η縶��", ItemInfo.ItemType.ETC));
    }
}
