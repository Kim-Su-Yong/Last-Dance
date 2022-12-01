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

    // �巡�� �̺�Ʈ
    public void OnDrag(PointerEventData eventData)
    {
        // �巡�� �̺�Ʈ�� �߻��ϸ� �������� ��ġ�� ���콺 Ŀ���� ��ġ�� ����
        itemTr.position = Input.mousePosition;
    }

    // �巡�׸� ������ �� �� �� ȣ��Ǵ� �̺�Ʈ
    public void OnBeginDrag(PointerEventData eventData)
    {
        // �θ� Inventory�� ����
        this.transform.SetParent(inventoryTr);
    }
}
