using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory_cy : MonoBehaviour
{
    public static Inventory_cy instance;

    private DatabaseManager_cy theData;

    public List<RectTransform> Nodes;
    private List<Item_cy> itemicon;

    public GameObject prefab_floating_text;
    public Transform messageTr;


    void Start()
    {
        instance = this;
        theData = FindObjectOfType<DatabaseManager_cy>();
        itemicon = new List<Item_cy>();
    }


    void Update()
    {
        
    }

    public void GetAnItem(int itemID, int _count)
    {
        for (int i = 0; i < theData.itemList.Count; i++) //�����ͺ��̽����� ������ �˻�
        {
            if (itemID == theData.itemList[i].itemID)
            {
                GameObject clone = Instantiate(prefab_floating_text, messageTr.position, Quaternion.Euler(Vector3.zero));
                clone.GetComponent<FloatingText>().text.text = theData.itemList[i].itemName + " " + _count + "�� ȹ�� +";
                clone.transform.SetParent(this.transform);

                itemicon.Add(theData.itemList[i]);
            }
        }
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
