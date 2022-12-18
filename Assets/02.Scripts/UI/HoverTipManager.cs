using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HoverTipManager : MonoBehaviour
{
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI tipText;
    public Image itemImage;

    public RectTransform tipWindow;

    public static Action<string, string, Sprite, Vector2> OnMouseHover;
    public static Action OnMouseLoseFocus;

    private void OnEnable()
    {
        OnMouseHover += ShowTip;
        OnMouseLoseFocus += HideTip;
    }
    private void OnDisable()
    {
        OnMouseHover -= ShowTip;
        OnMouseLoseFocus -= HideTip;
    }
    void Start()
    {
        HideTip();
    }
    private void ShowTip(string title, string tip, Sprite item, Vector2 mousePos)
    {
        titleText.text = title;
        tipText.text = tip;
        itemImage.sprite = item;
        tipWindow.sizeDelta = new Vector2(650, 250);

        tipWindow.gameObject.SetActive(true);
        //tipWindow.transform.position = new Vector2(mousePos.x + tipWindow.sizeDelta.x, mousePos.y - tipWindow.sizeDelta.y * 2);
        tipWindow.transform.position = new Vector2(mousePos.x + 30, mousePos.y - 30);
    }
    private void HideTip()
    {
        tipText.text = default;
        tipWindow.gameObject.SetActive(false);

    }
}
