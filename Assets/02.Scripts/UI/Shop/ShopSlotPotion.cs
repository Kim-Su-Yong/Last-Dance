using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ShopSlotPotion : MonoBehaviour
{
    public int itemID;  //아이템ID로 해당 템 구매를 위한 변수
    public int _count;  //아이템 갯수 변수
    private Button button;
    private Button button1;
    private Button button2;
    public SoundManager theSound;
    public string click_sound;
    public bool PotionBuy;
    void Awake()
    {
        theSound = FindObjectOfType<SoundManager>();
        button = GetComponent<Button>();
        button1 = button.transform.GetChild(1).GetComponentInChildren<Button>(); //구매하면 해당 버튼 상호작용이 false되면서 이미지가 회색으로 표시되기 위한 버튼변수
        button2 = button.transform.GetChild(2).GetComponentInChildren<Button>(); //구매하면 해당 버튼 상호작용이 false되면서 구매버튼이 회색으로 표시되기 위한 버튼변수
    }
    public void BuyPotion() //포션 구매 함수
    {
        theSound.Play(click_sound);
        if (PlayerStat.instance.money >= 400) //소지금이 500원이상일때 구매 가능하게 하는 조건문
        {
            Inventory.instance.GetAnItem(111, 1);
            PlayerStat.instance.ChangeMoney(-400); //소지금에서 400원 감소
            PotionBuy = true;
            cantbuy();
        }
    }
    public void cantbuy()
    {
        button1.interactable = false;
        button2.interactable = false;
    }
    private void OnEnable()
    {
        if(PotionBuy)
        {
            button1.interactable = false;
            button2.interactable = false;
        }
    }
}
