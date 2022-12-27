using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DatabaseManager : MonoBehaviour
{
    static public DatabaseManager instance;
    
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
    public string[] var_name;
    public float[] var;

    public string[] switch_name;
    public bool[] switches;

    public List<ItemInfo> itemList = new List<ItemInfo>();
    //public List<EquipStat> equipStat = new List<EquipStat>();
    void Start()
    {
        itemList.Add(new ItemInfo(111, "일반체력포션", "체력을 10 회복시켜주는 기적의 물약", ItemInfo.ItemType.Consume, 10));
        itemList.Add(new ItemInfo(112, "고급체력포션", "체력을 20 회복시켜주는 기적의 물약", ItemInfo.ItemType.Consume, 20));
        itemList.Add(new ItemInfo(113, "전설체력포션", "체력을 50 회복시켜주는 기적의 물약", ItemInfo.ItemType.Consume, 50));
        //itemList.Add(new ItemInfo(121, "일반마나포션", "마나를 10 회복시켜주는 기적의 물약", ItemInfo.ItemType.Consume));
        //itemList.Add(new ItemInfo(122, "고급마나포션", "마나를 20 회복시켜주는 기적의 물약", ItemInfo.ItemType.Consume));
        //itemList.Add(new ItemInfo(123, "전설마나포션", "마나를 50 회복시켜주는 기적의 물약", ItemInfo.ItemType.Consume));
        itemList.Add(new ItemInfo(211, "일반 검", "일반 용사의 검", ItemInfo.ItemType.Equip, ItemInfo.EquipType.Weapon, 5));
        itemList.Add(new ItemInfo(212, "고급 검", "고급 용사의 검", ItemInfo.ItemType.Equip, ItemInfo.EquipType.Weapon, 10));
        itemList.Add(new ItemInfo(213, "전설 검", "전설 용사의검", ItemInfo.ItemType.Equip, ItemInfo.EquipType.Weapon, 20));
        itemList.Add(new ItemInfo(214, "일반 완드", "일반 마법사의 지팡이", ItemInfo.ItemType.Equip, ItemInfo.EquipType.Weapon, 5));
        itemList.Add(new ItemInfo(215, "고급 완드", "고급 마법사의 지팡이", ItemInfo.ItemType.Equip, ItemInfo.EquipType.Weapon, 10));
        itemList.Add(new ItemInfo(216, "전설 완드", "전설 마법사의 지팡이", ItemInfo.ItemType.Equip, ItemInfo.EquipType.Weapon, 20));
        itemList.Add(new ItemInfo(221, "일반 투구", "일반 투구", ItemInfo.ItemType.Equip, ItemInfo.EquipType.Helmet, 0, 5));
        itemList.Add(new ItemInfo(222, "고급 투구", "고급 투구", ItemInfo.ItemType.Equip, ItemInfo.EquipType.Helmet, 0, 10));
        itemList.Add(new ItemInfo(223, "전설 투구", "전설 투구", ItemInfo.ItemType.Equip, ItemInfo.EquipType.Helmet, 0, 20));
        itemList.Add(new ItemInfo(231, "일반 갑옷", "일반 갑옷", ItemInfo.ItemType.Equip, ItemInfo.EquipType.Armor, 0, 5));
        itemList.Add(new ItemInfo(232, "고급 갑옷", "고급 갑옷", ItemInfo.ItemType.Equip, ItemInfo.EquipType.Armor, 0, 10));
        itemList.Add(new ItemInfo(233, "전설 갑옷", "전설 갑옷", ItemInfo.ItemType.Equip, ItemInfo.EquipType.Armor, 0, 20));
        itemList.Add(new ItemInfo(241, "일반 신발", "일반 신발", ItemInfo.ItemType.Equip, ItemInfo.EquipType.Boots, 0, 0, 0, 1f));
        itemList.Add(new ItemInfo(242, "고급 신발", "고급 신발", ItemInfo.ItemType.Equip, ItemInfo.EquipType.Boots, 0, 0, 0, 2f));
        itemList.Add(new ItemInfo(243, "전설 신발", "전설 신발", ItemInfo.ItemType.Equip, ItemInfo.EquipType.Boots, 0, 0, 0, 4f));
        itemList.Add(new ItemInfo(251, "일반 장갑", "일반 장갑", ItemInfo.ItemType.Equip, ItemInfo.EquipType.Gloves, 3));
        itemList.Add(new ItemInfo(252, "고급 장갑", "고급 장갑", ItemInfo.ItemType.Equip, ItemInfo.EquipType.Gloves, 5));
        itemList.Add(new ItemInfo(253, "전설 장갑", "전설 장갑", ItemInfo.ItemType.Equip, ItemInfo.EquipType.Gloves, 10));
        itemList.Add(new ItemInfo(261, "일반 반지", "일반 반지", ItemInfo.ItemType.Equip, ItemInfo.EquipType.Totem, 0, 0, 50));
        itemList.Add(new ItemInfo(262, "고급 반지", "고급 반지", ItemInfo.ItemType.Equip, ItemInfo.EquipType.Totem, 0, 0, 100));
        itemList.Add(new ItemInfo(263, "전설 반지", "전설 반지", ItemInfo.ItemType.Equip, ItemInfo.EquipType.Totem, 0, 0, 200));
        itemList.Add(new ItemInfo(264, "일반 목걸이", "일반 목걸이", ItemInfo.ItemType.Equip, ItemInfo.EquipType.Totem2, 0, 0, 50));
        itemList.Add(new ItemInfo(265, "고급 목걸이", "고급 목걸이", ItemInfo.ItemType.Equip, ItemInfo.EquipType.Totem2, 0, 0, 100));
        itemList.Add(new ItemInfo(266, "전설 목걸이", "전설 목걸이", ItemInfo.ItemType.Equip, ItemInfo.EquipType.Totem2, 0, 0, 200));
        itemList.Add(new ItemInfo(311, "버섯", "자연산 송이버섯", ItemInfo.ItemType.ETC));
        itemList.Add(new ItemInfo(312, "해골", "스켈레톤의 해골", ItemInfo.ItemType.ETC));
        itemList.Add(new ItemInfo(313, "생선", "피쉬맨이 잡은 생선", ItemInfo.ItemType.ETC));
        itemList.Add(new ItemInfo(314, "두루마리", "고대 상형문자가 쓰여있는 두루마리", ItemInfo.ItemType.ETC));
    }
}
