using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopData : MonoBehaviour
{
    public List<Item_cy> stocks = new List<Item_cy>();
    public bool[] soldOuts;
    void Start()
    {
        stocks.Add(DatabaseManager_cy.instance.itemList[0]);
        stocks.Add(DatabaseManager_cy.instance.itemList[1]);
        stocks.Add(DatabaseManager_cy.instance.itemList[2]);
        stocks.Add(DatabaseManager_cy.instance.itemList[3]);
        stocks.Add(DatabaseManager_cy.instance.itemList[4]);
        soldOuts = new bool[stocks.Count];
        for(int i = 0; i < soldOuts.Length; i++)
        {
            soldOuts[i] = false;
        }
    }
}
