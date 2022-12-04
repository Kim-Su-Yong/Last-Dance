using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Drop : MonoBehaviour, IDropHandler
{
    // 창 분류(장비창 or 인벤토리창)
    public enum WindowType { EquipWindow, InvenWindow };
    public WindowType windowType;

    // 슬롯 분류(아이템 카테고리와 대응 -> ItemInfo.cs 참고)
    public enum SlotType { EquipmentSlot, ConsumeSlot, EtcSlot };
    public SlotType slotType;


    public void OnDrop(PointerEventData eventData)
    {
        GameObject dropped = eventData.pointerDrag;
        Drag dragItem = dropped.GetComponent<Drag>();

        // 아이템 카테고리 유효성 검사
        switch (ItemInfo.itemInfo.itemCategory)
        {
            case ItemInfo.ItemCategory.Equip:
                if (this.slotType == SlotType.EquipmentSlot)
                {
                    if (transform.childCount == 0)
                    {
                        Drag.draggingItem.transform.SetParent(this.transform);
                    }
                }
                break;

            case ItemInfo.ItemCategory.Consume:
                if (this.slotType == SlotType.ConsumeSlot)
                {
                    if (transform.childCount == 0)
                    {
                        Drag.draggingItem.transform.SetParent(this.transform);
                    }
                }
                break;

            case ItemInfo.ItemCategory.Etc:
                if (this.slotType == SlotType.EtcSlot)
                {
                    if (transform.childCount == 0)
                    {
                        Drag.draggingItem.transform.SetParent(this.transform);
                    }
                }
                break;
        }
        // ★ 추후 장비 장착 시 데이터 적용을 위해 Type으로 분류하였음
        switch (windowType)
        {
            case WindowType.InvenWindow:
                if (transform.childCount == 0)
                {
                    Drag.draggingItem.transform.SetParent(this.transform);
                }
                break;
            case WindowType.EquipWindow:
                if (transform.childCount == 0)
                {
                    Drag.draggingItem.transform.SetParent(this.transform);
                }
                break;
        }
    }
}