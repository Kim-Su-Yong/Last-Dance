using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ShopSlotChest : MonoBehaviour
{
    public bool chestSoldOut;
    public int itemID;  //아이템ID로 해당 템 구매를 위한 변수
    public int _count;  //아이템 갯수 변수
    private Button button;
    private Button button1;
    private Button button2;
    void Awake()
    {
        button = GetComponent<Button>();
        button1 = button.transform.GetChild(1).GetComponentInChildren<Button>(); //구매하면 해당 버튼 상호작용이 false되면서 이미지가 회색으로 표시되기 위한 버튼변수
        button2 = button.transform.GetChild(2).GetComponentInChildren<Button>(); //구매하면 해당 버튼 상호작용이 false되면서 구매버튼이 회색으로 표시되기 위한 버튼변수
    }
    public void BuyChest() //갑옷 구매 함수
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
