using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HoverTip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public string titleToShow;
    public string tipToShow;
    public string countToShow;
    public int itemCount;
    public Sprite itemToShow;
    private float timeToWait = 0.5f;

    public void OnPointerEnter(PointerEventData eventData)
    {
        StopAllCoroutines();
        StartCoroutine(StartTimer());
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        StopAllCoroutines();
        HoverTipManager.OnMouseLoseFocus();
    }
    private void ShowMessage()
    {
        HoverTipManager.OnMouseHover(titleToShow, tipToShow, countToShow, itemToShow, Input.mousePosition);
    }
    private IEnumerator StartTimer()
    {
        // 0.5초 있다가 메시지 보여준다.
        yield return new WaitForSeconds(timeToWait);

        ShowMessage();
    }
}
