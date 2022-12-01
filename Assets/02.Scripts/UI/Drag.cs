using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Drag : MonoBehaviour, IDragHandler, IBeginDragHandler
{
    private Transform itemTr;
    private Transform inventoryTr;


    void Start()
    {
        itemTr = GetComponent<Transform>();
        inventoryTr = GameObject.Find("Inventory").GetComponent<Transform>();
    }

    // 드래그 이벤트
    public void OnDrag(PointerEventData eventData)
    {
        // 드래그 이벤트가 발생하면 아이템의 위치를 마우스 커서의 위치로 변경
        itemTr.position = Input.mousePosition;
    }

    // 드래그를 시작할 때 한 번 호출되는 이벤트
    public void OnBeginDrag(PointerEventData eventData)
    {
        // 부모를 Inventory로 변경
        this.transform.SetParent(inventoryTr);
    }
}
