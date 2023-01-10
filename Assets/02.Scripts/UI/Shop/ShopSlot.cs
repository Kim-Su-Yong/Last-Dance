using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ShopSlot : MonoBehaviour
{
    public bool potionSoldOut; //���� �ǸŰ� �ƴ��� üũ�ϴ� ����
    public bool weaponSoldOut;
    public bool chestSoldOut;
    public bool bootsSoldOut;
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
    void Update()
    {
        if(potionSoldOut)
        {
            CantBuy();
        }
    }
    public void BuyPotion() //���� ���� �Լ�
    {
        if (PlayerStat.instance.money >= 400 && !potionSoldOut) //�������� 500���̻��϶� ���� �����ϰ� �ϴ� ���ǹ�
        {
            Inventory.instance.GetAnItem(111, 1);
            potionSoldOut = true;
            PlayerStat.instance.money -= 400; //�����ݿ��� 500�� ����
            button1.interactable = false; //��ư ��ȣ�ۿ� false�Ǹ鼭 ȸ����
            button2.interactable = false;
        }
    }
    private void CantBuy()
    {
        button1.interactable = false;
        button2.interactable = false;
    }
    public void BuyWeapon() //���� ���� �Լ�
    {
        if (PlayerStat.instance.money >= 4000 && !weaponSoldOut)
        {
            Inventory.instance.GetAnItem(211, 1);
            weaponSoldOut = true;
            PlayerStat.instance.money -= 4000;
            button1.interactable = false;
            button2.interactable = false;
        }
    }
    public void BuyChest() //���� ���� �Լ�
    {
        if (PlayerStat.instance.money >= 4000 && !chestSoldOut)
        {
            Inventory.instance.GetAnItem(231, 1);
            chestSoldOut = true;
            PlayerStat.instance.money -= 4000;
            button1.interactable = false;
            button2.interactable = false;
        }
    }
    public void BuyBoots() //�Ź� ���� �Լ�
    {
        if (PlayerStat.instance.money >= 4000 && !bootsSoldOut)
        {
            Inventory.instance.GetAnItem(241, 1);
            bootsSoldOut = true;
            PlayerStat.instance.money -= 4000;
            button1.interactable = false;
            button2.interactable = false;
        }
    }
}
