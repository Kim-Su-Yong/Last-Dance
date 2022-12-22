using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDatabase : MonoBehaviour
{
    public static ItemDatabase instance;
    public List<GameObject> Items;
    private void Awake()
    {
        instance = this;
        //Items = Resources.LoadAll<GameObject>("Items");
    }
    //public List<Item> itemDB;
    [Space(20)]
    public GameObject ItemPrefab;
    public Vector3[] pos;

    //public GameObject[] Items;

    private void Start()
    {
        for (int i = 0; i < 4; i++)
        {
            GameObject item = Instantiate(ItemPrefab, pos[i], Quaternion.identity);
            //GameObject item = Instantiate(Items[Random.Range(0, Items.Length)]);
            //GameObject go =  Instantiate(ItemPrefab, pos[i], Quaternion.identity);
            //go.GetComponent<Item_cy>().(itemDB[Random.Range(0,itemDB.Count - 1)]);
        }        
    }
}
