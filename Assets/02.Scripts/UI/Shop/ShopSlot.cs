using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ShopSlot : MonoBehaviour
{
    public GameObject button;
    public Button button1;
    public Button button2;
    public ShopSlotPotion thePotion;
    void Start()
    {
        button = GameObject.Find("Canvas_UI").transform.GetChild(2).gameObject.transform.GetChild(2).gameObject.transform.GetChild(1).gameObject.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject;
        button1 = button.transform.GetChild(1).GetComponent<Button>(); //�����ϸ� �ش� ��ư ��ȣ�ۿ��� false�Ǹ鼭 �̹����� ȸ������ ǥ�õǱ� ���� ��ư����
        button2 = button.transform.GetChild(2).GetComponent<Button>(); //�����ϸ� �ش� ��ư ��ȣ�ۿ��� false�Ǹ鼭 ���Ź�ư�� ȸ������ ǥ�õǱ� ���� ��ư����
    }
    public void cantbuy()
    {
        if (thePotion.potionSoldOut)
        {
            button1.interactable = false;
            button2.interactable = false;
        }
    }
}
