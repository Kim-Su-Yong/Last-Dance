using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Drop : MonoBehaviour, IDropHandler
{
    // â �з�(���â or �κ��丮â)
    public enum WindowType { EquipWindow, InvenWindow };
    public WindowType windowType;

    // ���� �з�(������ ī�װ��� ���� -> ItemInfo.cs ����)
    public enum SlotType { EquipmentSlot, ConsumeSlot, EtcSlot };
    public SlotType slotType;


    public void OnDrop(PointerEventData eventData)
    {
        GameObject dropped = eventData.pointerDrag;
        Drag dragItem = dropped.GetComponent<Drag>();

        // ������ ī�װ� ��ȿ�� �˻�
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
        // �� ���� ��� ���� �� ������ ������ ���� Type���� �з��Ͽ���
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