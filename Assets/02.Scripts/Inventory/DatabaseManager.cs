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

    public List<Item> itemList = new List<Item>();
    void Start()
    {
        itemList.Add(new Item(10001, "���� ����", "ü���� 50 ȸ�������ִ� ������ ����", Item.ItemType.Use));
        itemList.Add(new Item(10002, "�Ķ� ����", "������ 15 ȸ�������ִ� ������ ����", Item.ItemType.Use));
        itemList.Add(new Item(10003, "���� ���� ����", "ü���� 350 ȸ�������ִ� ������ ���� ����", Item.ItemType.Use));
        itemList.Add(new Item(10004, "���� �Ķ� ����", "������ 80 ȸ�������ִ� ������ ���� ����", Item.ItemType.Use));
        itemList.Add(new Item(11001, "���� ����", "�������� ������ ���´�. ���� Ȯ���� ��", Item.ItemType.Use));
        itemList.Add(new Item(20001, "ª�� ��", "�⺻���� ����� ��", Item.ItemType.Equip));
        itemList.Add(new Item(21001, "�����̾� ����", "1�п� �̴� 1�� ȸ�������ִ� ���� ����", Item.ItemType.Equip));
        itemList.Add(new Item(30001, "��� ������ ����1", "������ �ɰ��� ��� ������ ����", Item.ItemType.Quest));
        itemList.Add(new Item(30002, "��� ������ ����2", "������ �ɰ��� ��� ������ ����", Item.ItemType.Quest));
        itemList.Add(new Item(30003, "��� ����", "��� ������ �����ִ� ����� ����", Item.ItemType.Quest));
    }
}
