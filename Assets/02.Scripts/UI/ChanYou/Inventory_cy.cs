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
        for (int i = 0; i < theData.itemList.Count; i++) //�����ͺ��̽����� ������ �˻�
        {
            if (itemID == theData.itemList[i].itemID)
            {
                itemicon.Add(theData.itemList[i]);
            }
        }
        Debug.LogError("�����ͺ��̽��� �ش� ID���� ���� �������� �������� �ʽ��ϴ�."); //�����ͺ��̽��� itemID����
    }

    //private void OnEnable() ���� ��� Ʈ�������� ���ؼ� ������ �Ǿ����...
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
