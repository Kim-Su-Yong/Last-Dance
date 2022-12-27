using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EquipStat : MonoBehaviour
{
    public static EquipStat instance;

    [SerializeField]
    private List<ItemInfo> equipItemList; //장비창에 들어가있는 아이템 리스트

    public GameObject[] equipslots;

    public int equipCount = 0;

    
    private void Awake()
    {
        
    }

    void Start()
    {
        equipItemList = new List<ItemInfo>();
    }

    void Update()
    {
        if (equipCount == 1)
        {
            addEquip();
        }
        //if (equipCount < equipchange())
        //{
        //    equipCount = equipchange();
        //    AddEquip();
        //}
        //else if (equipCount > equipchange())
        //{
        //    equipCount = equipchange();
        //    RemoveEquip();
        //}
    }
    public void GetAnEquip(ItemInfo.EquipType equipType)
    {

    }

    public void addEquip()
    {

        equipCount = 0;
    }
    
    public int equipchange()
    {
        equipCount = 0;
        for (int i = 0; i < equipslots.Length; i++) //장비 슬롯만큼 검사
        {
            if (equipslots[i].transform.childCount != 0) //장비슬롯이 비어있지 않다면
                equipCount++;
        }
        return equipCount;
    }

    public void AddEquip()
    {
        for (int i = 0; i < equipslots.Length; i++)
        {
            if (equipslots[i].transform.childCount != 0) //장비슬롯이 비어있지 않다면
            {
                if (equipItemList.Count == 0)
                    equipItemList.Add(equipslots[i].GetComponentInChildren<ItemInfo>());
                else
                {
                    for (int j = 0; j < equipItemList.Count; j++)
                    {
                        if (equipItemList[j].itemID == equipslots[i].GetComponentInChildren<ItemInfo>().itemID)
                            continue;
                        //break;
                    }
                    equipItemList.Add(equipslots[i].GetComponentInChildren<ItemInfo>());
                    return;
                }
            }
        }
        //for (int i = 0; i < equipslots.Length; i++) //장비 슬롯만큼 검사
        //{
        //    if (equipslots[i].transform.childCount != 0) //장비슬롯이 비어있지 않다면
        //    {
        //        for (int j = 0; j < equipItemList.Count; j++) //착용한장비만큼 검사
        //        {
        //            if (equipItemList[j].itemID == equipslots[i].GetComponentInChildren<ItemInfo>().itemID)
        //                continue;
        //        }
        //        equipItemList.Add(equipslots[i].GetComponentInChildren<ItemInfo>());
        //        return;
        //    }
        //}
    }
    public void RemoveEquip()
    {
        for (int i = 0; i < equipslots.Length; i++) //장비 슬롯만큼 검사
        {

        }
    }
}