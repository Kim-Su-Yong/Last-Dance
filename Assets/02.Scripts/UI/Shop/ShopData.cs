using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopData : MonoBehaviour
{
    public List<GameObject> stocks = new List<GameObject>();
    public bool[] soldOuts;

    void Start()
    {
        stocks.Add(ItemDatabase.instance.Items[0]);
        stocks.Add(ItemDatabase.instance.Items[1]);
        stocks.Add(ItemDatabase.instance.Items[2]);
        stocks.Add(ItemDatabase.instance.Items[3]);
        soldOuts = new bool[stocks.Count];
        for(int i = 0; i < soldOuts.Length; i++)
        {
            soldOuts[i] = false;
        }
    }
}
