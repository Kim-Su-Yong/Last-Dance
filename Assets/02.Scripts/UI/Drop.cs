using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Drop : MonoBehaviour, IDropHandler
{
    // 창 분류(장비창 or 인벤토리창)
    public enum WindowType { EquipWindow, InvenWindow };
    public WindowType windowType;

    // 장비창 슬롯 분류
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

    // 슬롯 분류(아이템 카테고리와 대응 -> ItemInfo.cs 참고)
    public enum SlotType { EquipmentSlot, ConsumeSlot, EtcSlot, EquipSlot };
    public SlotType slotType;


    public void OnDrop(PointerEventData eventData)
    {
        GameObject dropped = eventData.pointerDrag;
        Drag dragItem = dropped.GetComponent<Drag>();
        ItemInfo dragItemInfo = dropped.GetComponent<ItemInfo>();

        // ★ 추후 장비 장착 시 데이터 적용을 위해 Type으로 분류하였음
        switch (windowType)
        {
            case WindowType.InvenWindow:
                // 아이템 카테고리 유효성 검사
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

            // 장비창 장착 가능한 슬롯인지 유효성 검사
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