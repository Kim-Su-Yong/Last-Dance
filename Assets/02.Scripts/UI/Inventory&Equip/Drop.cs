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
        Weapon, Helmet, Armor, Boots, Gloves, Totem, Totem2
        //GogglesSlot = 0,
        //HelmetSlot  = 1,
        //TotemSlot   = 2,
        //WeaponSlot  = 3,
        //ArmorSlot   = 4,
        //GlovesSlot  = 5,
        //ShieldSlot  = 6,
        //BootsSlot   = 7,
        //BackpackSlot= 8,
        //Totem2Slot = 3,
        };
    public EquipType equipType;

    // ���� �з�(������ ī�װ��� ���� -> ItemInfo.cs ����)
    public enum SlotType { EquipmentSlot, ConsumeSlot, EtcSlot, EquipSlot };
    public SlotType slotType;

    public GameObject prefab_floating_text;
    public Transform messageTr;

    PlayerDamage playerDamage;
    void Start()
    {
        playerDamage = FindObjectOfType<PlayerDamage>();
    }

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
                switch (dragItemInfo.itemType)
                {
                    
                    case ItemInfo.ItemType.Equip:
                        if (this.slotType == SlotType.EquipmentSlot)
                        {
                            if (transform.childCount == 0)
                            {
                                Drag.draggingItem.transform.SetParent(this.transform);
                            }
                        }
                        break;

                    case ItemInfo.ItemType.Consume:
                        if (this.slotType == SlotType.ConsumeSlot)
                        {
                            if (transform.childCount == 0)
                            {
                                Drag.draggingItem.transform.SetParent(this.transform);
                            }
                        }
                        break;

                    case ItemInfo.ItemType.ETC:
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
                
                switch (dragItemInfo.equipType)
                {
                    case ItemInfo.EquipType.Helmet:
                        if (this.equipType == EquipType.Helmet)
                        {
                            if (transform.childCount == 0)
                            {
                                Drag.draggingItem.transform.SetParent(this.transform);
                                if (Input.GetMouseButtonUp(0) && Drag.draggingItem.GetComponent<Drag>().isEquip == false)
                                {
                                    PlayerStat.instance.def += Drag.draggingItem.GetComponent<ItemInfo>().Def;
                                    ShowText();
                                    Inventory.instance.equipmentItemList.Add(Drag.draggingItem.GetComponent<ItemInfo>());
                                }
                            }
                        }
                        break;
                    case ItemInfo.EquipType.Totem:
                        if (this.equipType == EquipType.Totem)
                        {
                            if (transform.childCount == 0)
                            {
                                Drag.draggingItem.transform.SetParent(this.transform);
                                if (Input.GetMouseButtonUp(0) && Drag.draggingItem.GetComponent<Drag>().isEquip == false)
                                {
                                    PlayerStat.instance.maxHP += Drag.draggingItem.GetComponent<ItemInfo>().AddHp;
                                    ShowText();
                                    playerDamage.hpUpdate();
                                    Inventory.instance.equipmentItemList.Add(Drag.draggingItem.GetComponent<ItemInfo>());
                                }
                            }
                        }
                        break;
                    case ItemInfo.EquipType.Weapon:
                        if (this.equipType == EquipType.Weapon)
                        {
                            if (transform.childCount == 0)
                            {
                                Drag.draggingItem.transform.SetParent(this.transform);
                                if (Input.GetMouseButtonUp(0) && Drag.draggingItem.GetComponent<Drag>().isEquip == false)
                                {
                                    PlayerStat.instance.atk += Drag.draggingItem.GetComponent<ItemInfo>().Atk;
                                    ShowText();
                                    Inventory.instance.equipmentItemList.Add(Drag.draggingItem.GetComponent<ItemInfo>());
                                }
                            }
                        }
                        break;
                    case ItemInfo.EquipType.Armor:
                        if (this.equipType == EquipType.Armor)
                        {
                            if (transform.childCount == 0)
                            {
                                Drag.draggingItem.transform.SetParent(this.transform);
                                if (Input.GetMouseButtonUp(0) && Drag.draggingItem.GetComponent<Drag>().isEquip == false)
                                {
                                    PlayerStat.instance.def += Drag.draggingItem.GetComponent<ItemInfo>().Def;
                                    ShowText();
                                    Inventory.instance.equipmentItemList.Add(Drag.draggingItem.GetComponent<ItemInfo>());
                                }
                            }
                        }
                        break;
                    case ItemInfo.EquipType.Gloves:
                        if (this.equipType == EquipType.Gloves)
                        {
                            if (transform.childCount == 0)
                            {
                                Drag.draggingItem.transform.SetParent(this.transform);
                                if (Input.GetMouseButtonUp(0) && Drag.draggingItem.GetComponent<Drag>().isEquip == false)
                                {
                                    PlayerStat.instance.atk += Drag.draggingItem.GetComponent<ItemInfo>().Atk;
                                    ShowText();
                                    Inventory.instance.equipmentItemList.Add(Drag.draggingItem.GetComponent<ItemInfo>());
                                }
                            }
                        }
                        break;
                    case ItemInfo.EquipType.Boots:
                        if (this.equipType == EquipType.Boots)
                        {
                            if (transform.childCount == 0)
                            {
                                Drag.draggingItem.transform.SetParent(this.transform);
                                if (Input.GetMouseButtonUp(0) && Drag.draggingItem.GetComponent<Drag>().isEquip == false)
                                {
                                    PlayerStat.instance.speed += Drag.draggingItem.GetComponent<ItemInfo>().Speed;
                                    ShowText();
                                    Inventory.instance.equipmentItemList.Add(Drag.draggingItem.GetComponent<ItemInfo>());
                                }
                            }
                        }
                        break;
                    case ItemInfo.EquipType.Totem2:
                        if (this.equipType == EquipType.Totem2)
                        {
                            if (transform.childCount == 0)
                            {
                                Drag.draggingItem.transform.SetParent(this.transform);
                                if (Input.GetMouseButtonUp(0) && Drag.draggingItem.GetComponent<Drag>().isEquip == false)
                                {
                                    PlayerStat.instance.maxHP += Drag.draggingItem.GetComponent<ItemInfo>().AddHp;
                                    ShowText();
                                    playerDamage.hpUpdate();
                                    Inventory.instance.equipmentItemList.Add(Drag.draggingItem.GetComponent<ItemInfo>());
                                }
                            }
                        }
                        break;
                }
                break;
        }
    }

    private void ShowText()
    {
        GameObject Textclone = Instantiate(prefab_floating_text, messageTr.position, Quaternion.Euler(Vector3.zero));
        Textclone.GetComponent<FloatingText>().text.text = Drag.draggingItem.GetComponent<ItemInfo>().GetComponent<HoverTip>().countToShow.ToString();
        Textclone.transform.SetParent(messageTr.transform);
    }
}