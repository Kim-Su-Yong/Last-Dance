using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory_cy : MonoBehaviour
{
    private DatabaseManager_cy theData;

    public List<RectTransform> Nodes;



    void Start()
    {
        theData = FindObjectOfType<DatabaseManager_cy>();
    }


    void Update()
    {
        
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
