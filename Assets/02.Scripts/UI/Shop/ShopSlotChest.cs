using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ShopSlotChest : MonoBehaviour
{
    public bool chestSoldOut;
    public int itemID;  //������ID�� �ش� �� ���Ÿ� ���� ����
    public int _count;  //������ ���� ����
    private Button button;
    private Button button1;
    private Button button2;
    void Awake()
    {
        button = GetComponent<Button>();
        button1 = button.transform.GetChild(1).GetComponentInChildren<Button>(); //�����ϸ� �ش� ��ư ��ȣ�ۿ��� false�Ǹ鼭 �̹����� ȸ������ ǥ�õǱ� ���� ��ư����
        button2 = button.transform.GetChild(2).GetComponentInChildren<Button>(); //�����ϸ� �ش� ��ư ��ȣ�ۿ��� false�Ǹ鼭 ���Ź�ư�� ȸ������ ǥ�õǱ� ���� ��ư����
    }
    public void BuyChest() //���� ���� �Լ�
    {
        if (PlayerStat.instance.money >= 4000)
        {
            Inventory.instance.GetAnItem(231, 1);
            chestSoldOut = true;
            PlayerStat.instance.ChangeMoney(-4000);
            cantbuy();
        }
    }
    public void cantbuy()
    {
        button1.interactable = false;
        button2.interactable = false;
    }
}
