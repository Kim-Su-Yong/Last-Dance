using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemInfo : MonoBehaviour
{

    // 아이템 1차 분류
    public enum ItemCategory { Equip, Consume, Etc };
    public ItemCategory itemCategory;

    public static ItemInfo itemInfo;

    // 아이템 2차 분류
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


    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
