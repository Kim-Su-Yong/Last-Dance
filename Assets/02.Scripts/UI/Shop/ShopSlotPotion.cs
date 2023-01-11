using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ShopSlotPotion : MonoBehaviour
{
    public bool potionSoldOut; //포션 판매가 됐는지 체크하는 변수
    public int itemID;  //아이템ID로 해당 템 구매를 위한 변수
    public int _count;  //아이템 갯수 변수
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
        //button1 = button.transform.GetChild(1).GetComponentInChildren<Button>(); //구매하면 해당 버튼 상호작용이 false되면서 이미지가 회색으로 표시되기 위한 버튼변수
        //button2 = button.transform.GetChild(2).GetComponentInChildren<Button>(); //구매하면 해당 버튼 상호작용이 false되면서 구매버튼이 회색으로 표시되기 위한 버튼변수
        theShop = FindObjectOfType<ShopSlot>();
    }
    public void BuyPotion() //포션 구매 함수
    {
        if (PlayerStat.instance.money >= 400 && !potionSoldOut) //소지금이 500원이상일때 구매 가능하게 하는 조건문
        {
            Inventory.instance.GetAnItem(111, 1);
            potionSoldOut = true;
            PlayerStat.instance.ChangeMoney(-400); //소지금에서 400원 감소
            theShop.cantbuy();
        }
    }

}
