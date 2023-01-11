using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ShopSlotPotion : MonoBehaviour
{
    public bool potionSoldOut; //���� �ǸŰ� �ƴ��� üũ�ϴ� ����
    public int itemID;  //������ID�� �ش� �� ���Ÿ� ���� ����
    public int _count;  //������ ���� ����
    private Button button;
    private Button button1;
    private Button button2;
    void Start()
    {
        button = GetComponent<Button>();
        button1 = button.transform.GetChild(1).GetComponentInChildren<Button>(); //�����ϸ� �ش� ��ư ��ȣ�ۿ��� false�Ǹ鼭 �̹����� ȸ������ ǥ�õǱ� ���� ��ư����
        button2 = button.transform.GetChild(2).GetComponentInChildren<Button>(); //�����ϸ� �ش� ��ư ��ȣ�ۿ��� false�Ǹ鼭 ���Ź�ư�� ȸ������ ǥ�õǱ� ���� ��ư����
    }
    public void BuyPotion() //���� ���� �Լ�
    {
        if (PlayerStat.instance.money >= 400) //�������� 500���̻��϶� ���� �����ϰ� �ϴ� ���ǹ�
        {
            Inventory.instance.GetAnItem(111, 1);
            potionSoldOut = true;
            PlayerStat.instance.ChangeMoney(-400); //�����ݿ��� 400�� ����
            cantbuy();
        }
    }
    public void cantbuy()
    {
        if (potionSoldOut)
        {
            button1.interactable = false;
            button2.interactable = false;
        }
    }
}
