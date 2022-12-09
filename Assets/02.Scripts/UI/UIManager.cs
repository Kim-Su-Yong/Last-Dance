using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    public Image coolImg; //��Ÿ�� �̹���
    public Text cooltxt;  //��Ÿ�� �ð� �ؽ�Ʈ
    public bool isCool;   //��Ÿ������ �Ǵ�

    public GameObject invenGO; // �κ��丮 ������Ʈ
    public GameObject equipGO; // ���â ������Ʈ

    public bool activInven; //�κ��丮�� ������ true
    public bool activEquip; //���â�� ������ true



    void Start()
    {
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
            activInven = !activInven;
            if (activInven)
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
            activEquip = !activEquip;
            if (activEquip)
            {
                equipGO.SetActive(true);
            }
            else
            {
                equipGO.SetActive(false);
            }
        }
    }
}