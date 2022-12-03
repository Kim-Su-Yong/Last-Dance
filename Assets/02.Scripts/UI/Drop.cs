using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Drop : MonoBehaviour, IDropHandler
{
    public enum Type { Equipment, Inventory };
    public Type SlotType;
    public void OnDrop(PointerEventData eventData)
    {
        GameObject dropped = eventData.pointerDrag;
        Drag dragItem = dropped.GetComponent<Drag>();

        // ★ 추후 장비 장착 데이터 적용을 위해 Type으로 분류하였음
        switch (SlotType)
        {
            case Type.Inventory:
                if (transform.childCount == 0)
                {
                    Drag.draggingItem.transform.SetParent(this.transform);
                }
                break;
            case Type.Equipment:
                if (transform.childCount == 0)
                {
                    Drag.draggingItem.transform.SetParent(this.transform);
                }
                break;
        }
    }
}