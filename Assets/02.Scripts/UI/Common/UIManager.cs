using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    public Image coolImg; //쿨타임 이미지
    public Text cooltxt;  //쿨타임 시간 텍스트
    public bool isCool;   //쿨타임인지 판단

    public GameObject invenGO; // 인벤토리 오브젝트
    public GameObject equipGO; // 장비창 오브젝트
    public GameObject shopGO;

    public bool activeInven; //인벤토리가 켜지면 true
    public bool activeEquip; //장비창이 켜지면 true
    public bool activeShop;

    void Start()
    {
        equipGO.SetActive(false);
        invenGO.SetActive(false);
        coolImg.fillAmount = 0f;
        cooltxt.enabled = false;
        coolImg.enabled = false;
        isCool = true;
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.X) && isCool)
        {
            isCool = false;
            cooltxt.enabled = true;
            coolImg.enabled = true;
            StartCoroutine(CoolTime(3f));
        }
        ShowInven();
        ShowEquip();
        ShowShop();
    }
    IEnumerator CoolTime(float cool)
    {
        float cTxt = cool;
        while (cTxt > 0)
        {
            cTxt -= Time.deltaTime;
            coolImg.fillAmount = cTxt / cool;
            cooltxt.text = cTxt.ToString("0.0");
            yield return new WaitForFixedUpdate();
        }
        coolImg.fillAmount = 0f;
        cooltxt.enabled = false;
        coolImg.enabled = false;
        isCool = true;
    }
    void ShowInven()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            activeInven = !activeInven;
            if (activeInven)
            {
                invenGO.SetActive(true);
                //SeletedItem();
            }
            else
            {
                invenGO.SetActive(false);
            }
        }
    }
    void ShowEquip()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            activeEquip = !activeEquip;
            if (activeEquip)
            {
                equipGO.SetActive(true);
            }
            else
            {
                equipGO.SetActive(false);
            }
        }
    }
    void ShowShop()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            activeShop = !activeShop;
            if (activeShop)
            {
                shopGO.SetActive(true);
                invenGO.SetActive(true);
            }
            else
            {
                shopGO.SetActive(false);
                invenGO.SetActive(false);
            }
        }
    }
    public void CloseShop()
    {
        shopGO.SetActive(false);
        invenGO.SetActive(false);
    }
}