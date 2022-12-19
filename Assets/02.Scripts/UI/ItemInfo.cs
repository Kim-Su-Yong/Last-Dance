using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemInfo : MonoBehaviour
{
    // ������ 1�� �з�
    public enum ItemCategory { Equip, Consume, Etc };
    public ItemCategory itemCategory;

    public static ItemInfo itemInfo;

    // ������ 2�� �з�
    public enum ItemType
    {
        Goggles     = 0,
        Helmet      = 1,
        Totem       = 2,
        Weapon      = 3,
        Armor       = 4,
        Gloves      = 5,
        Shield      = 6,
        Boots       = 7,
        Backpack    = 8,
        Potion      = 9,
        etc         = 11
    };
    public ItemType itemType;
}
