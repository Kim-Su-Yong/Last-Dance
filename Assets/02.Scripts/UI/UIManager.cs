using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Image coolImg;
    public Text cooltxt;
    public bool isCool;
    public static UIManager uIManager;


    void Start()
    {
        coolImg.fillAmount = 0f;
        cooltxt.enabled = false;
        isCool = true;
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z) && isCool)
        {
            isCool = false;
            cooltxt.enabled = true;
            StartCoroutine(CoolTime(3f));
        }
    }
    IEnumerator CoolTime(float cool)
    {
        float cTime = 0f;
        float cTxt = cool;
        while (cool > cTime)
        {
            cTime += Time.deltaTime;
            cTxt -= Time.deltaTime;
            coolImg.fillAmount = (cTime / cool);
            cooltxt.text = cTxt.ToString("0.0");
            yield return new WaitForFixedUpdate();
        }
        coolImg.fillAmount = 0f;
        cooltxt.enabled = false;
        isCool = true;
    }
}