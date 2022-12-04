using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Drop : MonoBehaviour, IDropHandler
{
    // â �з�(���â or �κ��丮â)
    public enum WindowType { EquipWindow, InvenWindow };
    public WindowType windowType;

    // ���â ���� �з�
    public enum EquipType {
        GogglesSlot = 0,
        HelmetSlot  = 1,
        TotemSlot   = 2,
        WeaponSlot  = 3,
        ArmorSlot   = 4,
        GlovesSlot  = 5,
        ShieldSlot  = 6,
        BootsSlot   = 7,
        BackpackSlot= 8 };
    public EquipType equipType;

    // ���� �з�(������ ī�װ��� ���� -> ItemInfo.cs ����)
    public enum SlotType { EquipmentSlot, ConsumeSlot, EtcSlot, EquipSlot };
    public SlotType slotType;


    public void OnDrop(PointerEventData eventData)
    {
        GameObject dropped = eventData.pointerDrag;
        Drag dragItem = dropped.GetComponent<Drag>();
        ItemInfo dragItemInfo = dropped.GetComponent<ItemInfo>();

        // �� ���� ��� ���� �� ������ ������ ���� Type���� �з��Ͽ���
        switch (windowType)
        {
            case WindowType.InvenWindow:
                // ������ ī�װ� ��ȿ�� �˻�
                switch (dragItemInfo.itemCategory)
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
                break;

            // ���â ���� ������ �������� ��ȿ�� �˻�
            case WindowType.EquipWindow:
                switch (dragItemInfo.itemType)
                {
                    case ItemInfo.ItemType.Goggles:
                        if (this.equipType == EquipType.GogglesSlot)
                        {
                            if (transform.childCount == 0)
                            {
                                Drag.draggingItem.transform.SetParent(this.transform);
                            }
                        }
                        break;
                    case ItemInfo.ItemType.Helmet:
                        if (this.equipType == EquipType.HelmetSlot)
                        {
                            if (transform.childCount == 0)
                            {
                                Drag.draggingItem.transform.SetParent(this.transform);
                            }
                        }
                        break;
                    case ItemInfo.ItemType.Totem:
                        if (this.equipType == EquipType.TotemSlot)
                        {
                            if (transform.childCount == 0)
                            {
                                Drag.draggingItem.transform.SetParent(this.transform);
                            }
                        }
                        break;
                    case ItemInfo.ItemType.Weapon:
                        if (this.equipType == EquipType.WeaponSlot)
                        {
                            if (transform.childCount == 0)
                            {
                                Drag.draggingItem.transform.SetParent(this.transform);
                            }
                        }
                        break;
                    case ItemInfo.ItemType.Armor:
                        if (this.equipType == EquipType.ArmorSlot)
                        {
                            if (transform.childCount == 0)
                            {
                                Drag.draggingItem.transform.SetParent(this.transform);
                            }
                        }
                        break;
                    case ItemInfo.ItemType.Gloves:
                        if (this.equipType == EquipType.GlovesSlot)
                        {
                            if (transform.childCount == 0)
                            {
                                Drag.draggingItem.transform.SetParent(this.transform);
                            }
                        }
                        break;
                    case ItemInfo.ItemType.Shield:
                        if (this.equipType == EquipType.ShieldSlot)
                        {
                            if (transform.childCount == 0)
                            {
                                Drag.draggingItem.transform.SetParent(this.transform);
                            }
                        }
                        break;
                    case ItemInfo.ItemType.Boots:
                        if (this.equipType == EquipType.BootsSlot)
                        {
                            if (transform.childCount == 0)
                            {
                                Drag.draggingItem.transform.SetParent(this.transform);
                            }
                        }
                        break;
                    case ItemInfo.ItemType.Backpack:
                        if (this.equipType == EquipType.BackpackSlot)
                        {
                            if (transform.childCount == 0)
                            {
                                Drag.draggingItem.transform.SetParent(this.transform);
                            }
                        }
                        break;
                }
                break;
        }
    }
}