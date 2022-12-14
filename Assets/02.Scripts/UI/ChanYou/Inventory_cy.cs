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
