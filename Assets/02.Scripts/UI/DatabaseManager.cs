using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DatabaseManager : MonoBehaviour
{
    static public DatabaseManager instance;
    private PlayerStat thePlayerStat;
    PlayerDamage playerDamage;
    public GameObject prefabs_Floating_Text;
    public GameObject parent;
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
    private void FloatingText(int number, string color)
    {
        Vector3 vector = thePlayerStat.transform.position;
        vector.y += 60;

        GameObject clone = Instantiate(prefabs_Floating_Text, vector, Quaternion.Euler(Vector3.zero));
        clone.GetComponent<FloatingText>().text.text = number.ToString();
        if(color == "GREEN")
            clone.GetComponent<FloatingText>().text.color = Color.green;
        else if(color == "BLUE")
            clone.GetComponent<FloatingText>().text.color = Color.blue;
        clone.GetComponent<FloatingText>().text.fontSize = 25;
        clone.transform.SetParent(parent.transform);
    }
    public void UseItem(int _itemID)
    {
        switch(_itemID)
        {
            case 10001:
                if (thePlayerStat.initHP >= playerDamage.curHp + 50)
                    playerDamage.curHp += 50;
                else
                    playerDamage.curHp = thePlayerStat.initHP;
                FloatingText(50, "GREEN");
                break;
        }
    }
    void Start()
    {
        thePlayerStat = FindObjectOfType<PlayerStat>();
        playerDamage = FindObjectOfType<PlayerDamage>();
        itemList.Add(new Item(10001, "���� ����", "ü���� 50 ȸ�������ִ� ������ ����", Item.ItemType.Use));
        itemList.Add(new Item(10002, "�Ķ� ����", "������ 15 ȸ�������ִ� ������ ����", Item.ItemType.Use));
        itemList.Add(new Item(10003, "���� ���� ����", "ü���� 350 ȸ�������ִ� ������ ���� ����", Item.ItemType.Use));
        itemList.Add(new Item(10004, "���� �Ķ� ����", "������ 80 ȸ�������ִ� ������ ���� ����", Item.ItemType.Use));
        itemList.Add(new Item(11001, "���� ����", "�������� ������ ���´�. ���� Ȯ���� ��", Item.ItemType.Use));
        itemList.Add(new Item(20001, "ª�� ��", "�⺻���� ����� ��", Item.ItemType.Equip));
        itemList.Add(new Item(20301, "�����̾� ����", "1�п� �̴� 1�� ȸ�������ִ� ���� ����", Item.ItemType.Equip));
        itemList.Add(new Item(30001, "��� ������ ����1", "������ �ɰ��� ��� ������ ����", Item.ItemType.Quest));
        itemList.Add(new Item(30002, "��� ������ ����2", "������ �ɰ��� ��� ������ ����", Item.ItemType.Quest));
        itemList.Add(new Item(30003, "��� ����", "��� ������ �����ִ� ����� ����", Item.ItemType.Quest));
    }
}
