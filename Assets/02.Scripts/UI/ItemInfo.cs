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
