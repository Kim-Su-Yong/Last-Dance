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
    public enum ItemType { 
        Goggles, 
        Helmet,
        Totem, 
        Weapon, 
        Armor, 
        Gloves, 
        Shield, 
        Boots, 
        Backpack, 
        Potion, 
        etc };
    public ItemType itemType;


    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
