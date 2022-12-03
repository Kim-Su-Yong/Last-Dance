using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Image coolImg;
    //public float inittime = 0f;
    //public float cooltime = 5f;
    public Text cooltxt;
    public bool isCool;
    public static UIManager uIManager;


    void Start()
    {
        coolImg.fillAmount = 1f;
        coolImg.enabled = false;
        cooltxt.enabled = false;
        isCool = true;
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z) && isCool)
        {
            isCool = false;
            cooltxt.enabled = true;
            coolImg.enabled = true;
            StartCoroutine(CoolTime(3f));
        }
    }
    IEnumerator CoolTime(float cool)
    {
        while (cool >= 0f)
        {
            cool -= Time.deltaTime;
            coolImg.fillAmount = (cool / 3f);
            cooltxt.text = cool.ToString("0.0");
            yield return new WaitForFixedUpdate();
        }
        coolImg.fillAmount = 1f;
        coolImg.enabled = false;
        cooltxt.enabled = false;
        isCool = true;
    }
}