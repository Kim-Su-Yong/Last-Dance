using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ShopSlot : MonoBehaviour
{
    public bool soldOut;
    public int itemID;
    public int _count;
    private Button button;
    private Button button1;
    private Button button2;

    public Inventory theInven;
    private void Awake()
    {
        button = GetComponent<Button>();
        button1 = button.transform.GetChild(1).GetComponentInChildren<Button>();
        button2 = button.transform.GetChild(2).GetComponentInChildren<Button>();
    }
    void Update()
    {
        if(Input.GetMouseButtonDown(1))
        {
            Sell();
        }
    }
    public void Buy()
    {
        Inventory.instance.GetAnItem(111, 1);
        soldOut = true;
        button1.interactable = false;
        button2.interactable = false;
    }
    public void Sell()
    {
        theInven.inventoryItemList.RemoveAt(0); //�������� ��������
        theInven.obj1.transform.GetChild(0).gameObject.SetActive(false); //�ǸŵǴ°�ó�� ���̴µ� �� ĭ�� �������� �ȵ�
        //Destroy(theInven.obj1.transform.GetChild(0).gameObject); //iteminfo���� �ı����ѹ���
    }
}
