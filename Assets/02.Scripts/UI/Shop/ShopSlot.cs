using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ShopSlot : MonoBehaviour
{
    public bool potionSoldOut; //포션 판매가 됐는지 체크하는 변수
    public bool weaponSoldOut;
    public bool chestSoldOut;
    public bool bootsSoldOut;
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
    void Update()
    {
        if(potionSoldOut)
        {
            CantBuy();
        }
    }
    public void BuyPotion() //포션 구매 함수
    {
        if (PlayerStat.instance.money >= 400 && !potionSoldOut) //소지금이 500원이상일때 구매 가능하게 하는 조건문
        {
            Inventory.instance.GetAnItem(111, 1);
            potionSoldOut = true;
            PlayerStat.instance.money -= 400; //소지금에서 500원 감소
            button1.interactable = false; //버튼 상호작용 false되면서 회색됨
            button2.interactable = false;
        }
    }
    private void CantBuy()
    {
        button1.interactable = false;
        button2.interactable = false;
    }
    public void BuyWeapon() //무기 구매 함수
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
    public void BuyChest() //갑옷 구매 함수
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
    public void BuyBoots() //신발 구매 함수
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
