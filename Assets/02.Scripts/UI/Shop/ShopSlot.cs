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
        button1 = button.transform.GetChild(1).GetComponent<Button>(); //구매하면 해당 버튼 상호작용이 false되면서 이미지가 회색으로 표시되기 위한 버튼변수
        button2 = button.transform.GetChild(2).GetComponent<Button>(); //구매하면 해당 버튼 상호작용이 false되면서 구매버튼이 회색으로 표시되기 위한 버튼변수
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
