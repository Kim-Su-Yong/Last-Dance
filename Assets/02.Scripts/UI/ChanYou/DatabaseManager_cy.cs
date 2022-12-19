using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DatabaseManager_cy : MonoBehaviour
{
    static public DatabaseManager_cy instance;
    
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            DontDestroyOnLoad(this.gameObject);
            instance = this;
        }
    }
    
    public List<ItemInfo> itemList = new List<ItemInfo>();
    
    void Start()
    {
        itemList.Add(new ItemInfo(111, "빨간 포션", "체력을 50 회복시켜주는 기적의 물약", ItemInfo.ItemType.Consume));
        itemList.Add(new ItemInfo(121, "파란 포션", "마나를 15 회복시켜주는 기적의 물약", ItemInfo.ItemType.Consume));
        itemList.Add(new ItemInfo(112, "농축 빨간 포션", "체력을 350 회복시켜주는 기적의 농축 물약", ItemInfo.ItemType.Consume));
        itemList.Add(new ItemInfo(122, "농축 파란 포션", "마나를 80 회복시켜주는 기적의 농축 물약", ItemInfo.ItemType.Consume));
        itemList.Add(new ItemInfo(211, "검", "기본검입니다", ItemInfo.ItemType.Equip, ItemInfo.EquipType.Weapon));
        itemList.Add(new ItemInfo(212, "좋은검", "좋은검입니다", ItemInfo.ItemType.Equip, ItemInfo.EquipType.Weapon));
        itemList.Add(new ItemInfo(221, "투구", "기본투구입니다", ItemInfo.ItemType.Equip, ItemInfo.EquipType.Helmet));
        itemList.Add(new ItemInfo(222, "좋은투구", "좋은투구입니다", ItemInfo.ItemType.Equip, ItemInfo.EquipType.Helmet));
        itemList.Add(new ItemInfo(231, "갑옷", "기본갑옷입니다", ItemInfo.ItemType.Equip, ItemInfo.EquipType.Armor));
        itemList.Add(new ItemInfo(232, "좋은갑옷", "좋은갑옷입니다", ItemInfo.ItemType.Equip, ItemInfo.EquipType.Armor));
        itemList.Add(new ItemInfo(241, "신발", "기본신발입니다", ItemInfo.ItemType.Equip, ItemInfo.EquipType.Boots));
        itemList.Add(new ItemInfo(251, "악세사리", "기본악세사리입니다", ItemInfo.ItemType.Equip, ItemInfo.EquipType.Totem));
        itemList.Add(new ItemInfo(252, "좋은악세사리", "좋은악세사리입니다", ItemInfo.ItemType.Equip, ItemInfo.EquipType.Totem));
        itemList.Add(new ItemInfo(311, "퀘스트아이템", "퀘스트아이템입니다", ItemInfo.ItemType.ETC));
        itemList.Add(new ItemInfo(312, "퀘스트아이템2", "퀘스트아이템입니다2", ItemInfo.ItemType.ETC));
    }
}
