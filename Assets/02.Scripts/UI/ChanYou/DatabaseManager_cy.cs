using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DatabaseManager_cy : MonoBehaviour
{
    static public DatabaseManager_cy instance;
    //private PlayerStat thePlayerStat;
    //public GameObject prefabs_Floating_Text;
    //public GameObject parent;
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
    //public string[] var_name;
    //public float[] var;

    //public string[] switch_name;
    //public bool[] switches;

    public List<Item_cy> itemList = new List<Item_cy>();
    //private void FloatingText(int number, string color)
    //{
    //    Vector3 vector = thePlayerStat.transform.position;
    //    vector.y += 60;

    //    GameObject clone = Instantiate(prefabs_Floating_Text, vector, Quaternion.Euler(Vector3.zero));
    //    clone.GetComponent<FloatingText>().text.text = number.ToString();
    //    if (color == "GREEN")
    //        clone.GetComponent<FloatingText>().text.color = Color.green;
    //    else if (color == "BLUE")
    //        clone.GetComponent<FloatingText>().text.color = Color.blue;
    //    clone.GetComponent<FloatingText>().text.fontSize = 25;
    //    clone.transform.SetParent(parent.transform);
    //}
    //public void UseItem(int _itemID)
    //{
    //    switch (_itemID)
    //    {
    //        case 10001:
    //            if (thePlayerStat.hp >= thePlayerStat.currentHP + 50)
    //                thePlayerStat.currentHP += 50;
    //            else
    //                thePlayerStat.currentHP = thePlayerStat.hp;
    //            FloatingText(50, "GREEN");
    //            break;
    //        case 10002:
    //            if (thePlayerStat.mp >= thePlayerStat.currentMP + 50)
    //                thePlayerStat.currentMP += 15;
    //            else
    //                thePlayerStat.currentMP = thePlayerStat.mp;
    //            FloatingText(50, "BLUE");
    //            break;
    //    }
    //}
    void Start()
    {
        //thePlayerStat = FindObjectOfType<PlayerStat>();
        itemList.Add(new Item_cy(111, "���� ����", "ü���� 50 ȸ�������ִ� ������ ����", Item_cy.ItemType.Consume));
        //itemList.Add(new Item_cy(10002, "�Ķ� ����", "������ 15 ȸ�������ִ� ������ ����", Item_cy.ItemType.Consume));
        //itemList.Add(new Item_cy(10003, "���� ���� ����", "ü���� 350 ȸ�������ִ� ������ ���� ����", Item_cy.ItemType.Consume));
        //itemList.Add(new Item_cy(10004, "���� �Ķ� ����", "������ 80 ȸ�������ִ� ������ ���� ����", Item_cy.ItemType.Consume));
        //itemList.Add(new Item_cy(11001, "���� ����", "�������� ������ ���´�. ���� Ȯ���� ��", Item_cy.ItemType.Consume));
        //itemList.Add(new Item_cy(20001, "ª�� ��", "�⺻���� ����� ��", Item_cy.ItemType.Equip));
        //itemList.Add(new Item_cy(20301, "�����̾� ����", "1�п� �̴� 1�� ȸ�������ִ� ���� ����", Item_cy.ItemType.Equip));
        //itemList.Add(new Item_cy(30001, "��� ������ ����1", "������ �ɰ��� ��� ������ ����", Item_cy.ItemType.ETC));
        //itemList.Add(new Item_cy(30002, "��� ������ ����2", "������ �ɰ��� ��� ������ ����", Item_cy.ItemType.ETC));
        //itemList.Add(new Item_cy(30003, "��� ����", "��� ������ �����ִ� ����� ����", Item_cy.ItemType.ETC));
    }
}
