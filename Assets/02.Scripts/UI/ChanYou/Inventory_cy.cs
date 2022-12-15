using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory_cy : MonoBehaviour
{
    public static Inventory_cy instance;

    private DatabaseManager_cy theData;
    

    public List<RectTransform> Nodes;
    private List<Item_cy> itemicon;


    void Start()
    {
        theData = FindObjectOfType<DatabaseManager_cy>();
        itemicon = new List<Item_cy>();
    }


    void Update()
    {
        
    }

    public void GetAnItem(int itemID)
    {
        for (int i = 0; i < theData.itemList.Count; i++) //데이터베이스에서 아이템 검색
        {
            if (itemID == theData.itemList[i].itemID)
            {
                itemicon.Add(theData.itemList[i]);
            }
        }
        Debug.LogError("데이터베이스에 해당 ID값을 가진 아이템이 존재하지 않습니다."); //데이터베이스에 itemID없음
    }

    //private void OnEnable() 하위 모든 트랜스폼에 대해서 적용이 되어버림...
    //{
    //    RectTransform[] transforms = GetComponentsInChildren<RectTransform>();
    //    Nodes = new List<RectTransform>();

    //    foreach (RectTransform tr in transforms)
    //    {
    //        if (tr != this.transform)
    //            Nodes.Add(tr);
    //    }
    //}
}
