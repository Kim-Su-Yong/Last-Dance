using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EquipStat : MonoBehaviour
{
    public static EquipStat instance;

    [SerializeField]
    private List<ItemInfo> equipItemList; //���â�� ���ִ� ������ ����Ʈ

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
        for (int i = 0; i < equipslots.Length; i++) //��� ���Ը�ŭ �˻�
        {
            if (equipslots[i].transform.childCount != 0) //��񽽷��� ������� �ʴٸ�
                equipCount++;
        }
        return equipCount;
    }

    public void AddEquip()
    {
        for (int i = 0; i < equipslots.Length; i++)
        {
            if (equipslots[i].transform.childCount != 0) //��񽽷��� ������� �ʴٸ�
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
        //for (int i = 0; i < equipslots.Length; i++) //��� ���Ը�ŭ �˻�
        //{
        //    if (equipslots[i].transform.childCount != 0) //��񽽷��� ������� �ʴٸ�
        //    {
        //        for (int j = 0; j < equipItemList.Count; j++) //���������ŭ �˻�
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
        for (int i = 0; i < equipslots.Length; i++) //��� ���Ը�ŭ �˻�
        {

        }
    }
}