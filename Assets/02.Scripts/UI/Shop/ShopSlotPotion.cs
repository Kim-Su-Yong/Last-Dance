using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ShopSlotPotion : MonoBehaviour
{
    public bool potionSoldOut; //���� �ǸŰ� �ƴ��� üũ�ϴ� ����
    public int itemID;  //������ID�� �ش� �� ���Ÿ� ���� ����
    public int _count;  //������ ���� ����
    //[SerializeField]
    //private GameObject button;
    //[SerializeField]
    //private Button button1;
    //[SerializeField]
    //private Button button2;
    //public static ShopSlotPotion shopSlotPotion;
    public ShopSlot theShop;
    void Awake()
    {
        //if (shopSlotPotion == null)
        //    shopSlotPotion = this;
        //else
        //    Destroy(gameObject);
        //DontDestroyOnLoad(this.gameObject);
    }
    void Start()
    {
        //button = GameObject.Find("Canvas_UI").transform.FindChild("ShopUI").gameObject;
        //button1 = button.transform.GetChild(1).GetComponentInChildren<Button>(); //�����ϸ� �ش� ��ư ��ȣ�ۿ��� false�Ǹ鼭 �̹����� ȸ������ ǥ�õǱ� ���� ��ư����
        //button2 = button.transform.GetChild(2).GetComponentInChildren<Button>(); //�����ϸ� �ش� ��ư ��ȣ�ۿ��� false�Ǹ鼭 ���Ź�ư�� ȸ������ ǥ�õǱ� ���� ��ư����
        theShop = FindObjectOfType<ShopSlot>();
    }
    public void BuyPotion() //���� ���� �Լ�
    {
        if (PlayerStat.instance.money >= 400 && !potionSoldOut) //�������� 500���̻��϶� ���� �����ϰ� �ϴ� ���ǹ�
        {
            Inventory.instance.GetAnItem(111, 1);
            potionSoldOut = true;
            PlayerStat.instance.ChangeMoney(-400); //�����ݿ��� 400�� ����
            theShop.cantbuy();
        }
    }

}
