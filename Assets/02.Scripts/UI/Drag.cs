using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Drag : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    private Transform itemTr;
    private Transform inventoryTr;
    private Transform slotTr;
    private Transform originTr;
    [SerializeField]
    private Transform itemListTr;
    private CanvasGroup canvasGroup;

    public static GameObject draggingItem = null;

    void Start()
    {
        itemTr = GetComponent<Transform>();
        //inventoryTr = GameObject.Find("Inventory").GetComponent<Transform>();
        slotTr = GameObject.Find("SlotList").GetComponent<Transform>();
        itemListTr = GameObject.Find("ItemList").GetComponent<Transform>();

        // Canvas Group ������Ʈ ����
        canvasGroup = GetComponent<CanvasGroup>();
    }

    // �巡�׸� ������ �� �� �� ȣ��Ǵ� �̺�Ʈ
    public void OnBeginDrag(PointerEventData eventData)
    {
        originTr = transform.parent;
        // �θ� Inventory�� ����
        this.transform.SetParent(slotTr);
        // �巡�װ� ���۵Ǹ� �巡�׵Ǵ� ������ ������ ������
        draggingItem = this.gameObject;
        // �巡�װ� ���۵Ǹ� �ٸ� UI �̺�Ʈ�� ���� �ʵ��� ����
        canvasGroup.blocksRaycasts = false;
    }

    // �巡�� �̺�Ʈ
    public void OnDrag(PointerEventData eventData)
    {
        // �巡�� �̺�Ʈ�� �߻��ϸ� �������� ��ġ�� ���콺 Ŀ���� ��ġ�� ����
        itemTr.position = Input.mousePosition;
    }


    // �巡�װ� ������� �� �� �� ȣ��Ǵ� �̺�Ʈ
    public void OnEndDrag(PointerEventData eventData)
    {
        // ���Կ� �巡������ �ʾ��� �� �ʱ� ��ġ�� �ǵ�����.
        if (itemTr.parent == slotTr)
        {
            itemTr.SetParent(originTr);
        }
        // �巡�װ� ����Ǹ� �巡�� �������� null�� ����
        draggingItem = null;
        // �巡�װ� ����Ǹ� �ٸ� UI �̺�Ʈ�� �޵��� ����
        canvasGroup.blocksRaycasts = true;

    }
}
