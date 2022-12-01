using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public static Inventory instance;

    private DatabaseManager theDatabase;
    private SoundManager theSound;
    private OrderManager theOrder;
    private OkOrCancel theOOC;

    public string key_sound;
    public string enter_sound;
    public string cancel_sound;
    public string open_sound;
    public string beep_sound;
    [SerializeField]
    private InventorySlot[] slots; //�κ��丮 ���Ե�

    private List<Item> inventoryItemList; //�÷��̾ ������ ������ ����Ʈ
    private List<Item> inventoryTabList; //������ �ǿ� ���� �ٸ��� ������ ������ ����Ʈ

    public Text Description_Text; //�ο� ����
    public string[] tabDescription; //�� �ο� ����

    public Transform tf; //slot �θ�ü

    public GameObject go; //�κ��丮 Ȱ��ȭ ��Ȱ��ȭ
    public GameObject[] selectedTabImages;
    public GameObject go_OOC; //������ Ȱ��ȭ ��Ȱ��ȭ
    public GameObject prefab_floating_text;

    private int selectedItem; //���õ� ������
    private int selectedTab; //���õ� ��

    private bool activated; //�κ��丮 Ȱ��ȭ�� true
    private bool tabActivated; //�� Ȱ��ȭ�� true
    private bool itemActivated; //������ Ȱ��ȭ�� true
    private bool stopKeyInput; //Ű�Է� ����(�Һ��� �� ���ǰ� ���� �ٵ�, �� �� Ű�Է� ����)
    private bool preventExec; //�ߺ����� ����

    private WaitForSeconds waitTime = new WaitForSeconds(0.01f);

    void Start()
    {
        instance = this;
        theDatabase = FindObjectOfType<DatabaseManager>();
        theSound = FindObjectOfType<SoundManager>();
        theOrder = FindObjectOfType<OrderManager>();
        theOOC = FindObjectOfType<OkOrCancel>();

        inventoryItemList = new List<Item>();
        inventoryTabList = new List<Item>();
        slots = tf.GetComponentsInChildren<InventorySlot>();
    }
    public void GetAnItem(int _itemID, int _count = 1)
    {
        for(int i = 0; i< theDatabase.itemList.Count; i++) //�����ͺ��̽� ������ �˻�
        {
            if(_itemID == theDatabase.itemList[i].itemID) //�����ͺ��̽��� ������ �߰�
            {
                GameObject clone = Instantiate(prefab_floating_text, PlayerAction.instance.transform.position, Quaternion.Euler(Vector3.zero));
                clone.GetComponent<FloatingText>().text.text = theDatabase.itemList[i].itemName + " " + _count + "�� ȹ�� +";
                clone.transform.SetParent(this.transform);

                for (int j = 0; j < inventoryItemList.Count; j++) //����ǰ�� ���� �������� �ִ��� �˻�
                {
                    if(inventoryItemList[i].itemID == _itemID) //����ǰ�� ���� �������� �ִ� -> ������ ����������
                    {
                        if (inventoryItemList[i].itemType == Item.ItemType.Use)
                        {
                            inventoryItemList[j].itemCount += _count;
                        }
                        else
                        {
                            inventoryItemList.Add(theDatabase.itemList[i]);
                        }
                        return;
                    }
                }
                inventoryItemList.Add(theDatabase.itemList[i]); //����ǰ�� �ش� ������ �߰�
                inventoryItemList[inventoryItemList.Count - 1].itemCount = _count;
                return;
            }
        }
        Debug.LogError("�����ͺ��̽��� �ش� ID���� ���� �������� �������� �ʽ��ϴ�."); //�����ͺ��̽��� itemID����
    }
    public void ShowTab()
    {
        RemoveSlot();
        SelectedTab();
    } //�� Ȱ��ȭ
    public void RemoveSlot()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].RemoveItem();
            slots[i].gameObject.SetActive(false);
        }
    } //�κ��丮 ���� �ʱ�ȭ
    public void SelectedTab()
    {
        StopAllCoroutines();
        Color color = selectedTabImages[selectedTab].GetComponent<Image>().color;
        color.a = 0f;
        for (int i = 0; i < selectedTabImages.Length; i++)
        {
            selectedTabImages[i].GetComponent<Image>().color = color;
        }
        Description_Text.text = tabDescription[selectedTab];
        StartCoroutine(SelectedTabEffectCoroutine());
    } //���õ� ���� �����ϰ� �ٸ� ��� ���� �÷� �˾ư� 0���� ����
    IEnumerator SelectedTabEffectCoroutine()
    {
        while (tabActivated)
        {
            Color color = selectedTabImages[0].GetComponent<Image>().color;
            while (color.a < 0.5f)
            {
                color.a += 0.03f;
                selectedTabImages[selectedTab].GetComponent<Image>().color = color;
                yield return waitTime;
            }
            while (color.a > 0f)
            {
                color.a -= 0.03f;
                selectedTabImages[selectedTab].GetComponent<Image>().color = color;
                yield return waitTime;
            }
            yield return new WaitForSeconds(0.3f);
        }
    } //���õ� �� ��¦�� ȿ��
    public void ShowItem()
    {
        inventoryTabList.Clear();
        RemoveSlot();
        selectedItem = 0;

        switch (selectedTab)
        {
            case 0:
                for (int i = 0; i < inventoryItemList.Count; i++)
                {
                    if (Item.ItemType.Use == inventoryItemList[i].itemType)
                        inventoryTabList.Add(inventoryItemList[i]);
                }
                break;
            case 1:
                for (int i = 0; i < inventoryItemList.Count; i++)
                {
                    if (Item.ItemType.Equip == inventoryItemList[i].itemType)
                        inventoryTabList.Add(inventoryItemList[i]);
                }
                break;
            case 2:
                for (int i = 0; i < inventoryItemList.Count; i++)
                {
                    if (Item.ItemType.Quest == inventoryItemList[i].itemType)
                        inventoryTabList.Add(inventoryItemList[i]);
                }
                break;
            case 3:
                for (int i = 0; i < inventoryItemList.Count; i++)
                {
                    if (Item.ItemType.ETC == inventoryItemList[i].itemType)
                        inventoryTabList.Add(inventoryItemList[i]);
                }
                break;
        } //�ǿ� ���� ������ �з�, �װ��� �κ��丮 �� ����Ʈ�� �߰�

        for (int i = 0; i < inventoryTabList.Count; i++)
        {
            slots[i].gameObject.SetActive(true);
            slots[i].Additem(inventoryTabList[i]);
        } //�κ��丮 �� ����Ʈ�� ������, �κ��丮 ���Կ� �߰�
        SelectedItem();
    } //������ Ȱ��ȭ (inventoryTabList�� ������ �´� �����۵鸸 �־��ְ� �κ��丮 ���Կ� ���)
    public void SelectedItem()
    {
        StopAllCoroutines();
        if (inventoryTabList.Count > 0)
        {
            Color color = slots[0].selected_item.GetComponent<Image>().color;
            color.a = 0f;
            for (int i = 0; i < inventoryTabList.Count; i++)
            {
                slots[i].selected_item.GetComponent<Image>().color = color;
            }
            Description_Text.text = inventoryTabList[selectedItem].itemDescription;
            StartCoroutine(SelectedItemEffectCoroutine());
        }
        else
            Description_Text.text = "�ش� Ÿ���� �������� �����ϰ� ���� �ʽ��ϴ�.";
    } //���õ� �������� �����ϰ� �ٸ� ��� ���� �÷��� ���İ��� 0���� ����
    IEnumerator SelectedItemEffectCoroutine()
    {
        while (itemActivated)
        {
            Color color = slots[0].GetComponent<Image>().color;
            while (color.a < 0.5f)
            {
                color.a += 0.03f;
                slots[selectedItem].selected_item.GetComponent<Image>().color = color;
                yield return waitTime;
            }
            while (color.a > 0f)
            {
                color.a -= 0.03f;
                slots[selectedItem].selected_item.GetComponent<Image>().color = color;
                yield return waitTime;
            }
            yield return new WaitForSeconds(0.3f);
        }
    } //���õ� ������ ��¦�� ȿ��
    void Update()
    {
        if (!stopKeyInput)
        {
            if (Input.GetKeyDown(KeyCode.I))
            {
                activated = !activated;
                if (activated)
                {
                    theSound.Play(open_sound);
                    //theOrder.NotMove();
                    go.SetActive(true);
                    selectedTab = 0;
                    tabActivated = true;
                    itemActivated = false;
                    ShowTab();
                }
                else
                {
                    theSound.Play(cancel_sound);
                    StopAllCoroutines();
                    go.SetActive(false);
                    tabActivated = false;
                    itemActivated = false;
                    //theOrder.Move();
                }
            }
            if (activated)
            {
                if (tabActivated)
                {
                    if (Input.GetKeyDown(KeyCode.RightArrow))
                    {
                        if (selectedTab < selectedTabImages.Length - 1)
                            selectedTab++;
                        else
                            selectedTab = 0;
                        theSound.Play(key_sound);
                        SelectedTab();
                    }
                    else if (Input.GetKeyDown(KeyCode.LeftArrow))
                    {
                        if (selectedTab > 0)
                            selectedTab--;
                        else
                            selectedTab = selectedTabImages.Length - 1;
                        theSound.Play(key_sound);
                        SelectedTab();
                    }
                    else if (Input.GetKeyDown(KeyCode.Z))
                    {
                        theSound.Play(enter_sound);
                        Color color = selectedTabImages[selectedTab].GetComponent<Image>().color;
                        color.a = 0.25f;
                        selectedTabImages[selectedTab].GetComponent<Image>().color = color;
                        itemActivated = true;
                        tabActivated = false;
                        preventExec = true;
                        ShowItem();
                    }
                } //�� Ȱ��ȭ�� Ű�Է� ó��

                else if(itemActivated)
                {
                    if(Input.GetKeyDown(KeyCode.DownArrow))
                    {
                        if (selectedItem < inventoryTabList.Count - 2)
                            selectedItem += 2;
                        else
                            selectedItem %= 2;
                        theSound.Play(key_sound);
                        SelectedItem();
                    }
                    else if (Input.GetKeyDown(KeyCode.UpArrow))
                    {
                        if (selectedItem > 1)
                            selectedItem -= 2;
                        else
                            selectedItem = inventoryTabList.Count - 1 - selectedItem;
                        theSound.Play(key_sound);
                        SelectedItem();
                    }
                    else if (Input.GetKeyDown(KeyCode.RightArrow))
                    {
                        if (selectedItem < inventoryTabList.Count - 1)
                            selectedItem++;
                        else
                            selectedItem = 0;
                        theSound.Play(key_sound);
                        SelectedItem();
                    }
                    else if (Input.GetKeyDown(KeyCode.LeftArrow))
                    {
                        if (selectedItem > 0)
                            selectedItem--;
                        else
                            selectedItem = inventoryTabList.Count - 1;
                        theSound.Play(key_sound);
                        SelectedItem();
                    }
                    else if (Input.GetKeyDown(KeyCode.Z) && preventExec)
                    {
                        if(selectedTab == 0) //�Ҹ�ǰ
                        {
                            theSound.Play(enter_sound);
                            stopKeyInput = true;
                            //������ ���� �ų�? ���� ������ ȣ��
                            StartCoroutine(OOCCoroutine());
                        }
                        else if(selectedTab == 1)
                        {
                            //��� ����
                        }
                        else //������ ���
                        {
                            theSound.Play(beep_sound);
                        }
                    }
                    else if (Input.GetKeyDown(KeyCode.X))
                    {
                        theSound.Play(cancel_sound);
                        StopAllCoroutines();
                        itemActivated = false;
                        tabActivated = true;
                        ShowTab();
                    }
                } //������ Ȱ��ȭ�� Ű�Է� ó��
                if (Input.GetKeyUp(KeyCode.Z)) //�ߺ� ���� ����
                    preventExec = false;
            }
        }
    }
    IEnumerator OOCCoroutine()
    {
        go_OOC.SetActive(true);
        theOOC.ShowTwoChoice("���", "���");
        yield return new WaitUntil(() => !theOOC.activated);
        if(theOOC.GetResult())
        {
            for(int i =0; i< inventoryItemList.Count; i++)
            {
                if(inventoryItemList[i].itemID == inventoryTabList[selectedItem].itemID)
                {
                    if (inventoryItemList[i].itemCount > 1)
                        inventoryItemList[i].itemCount--;
                    else
                        inventoryItemList.RemoveAt(i);

                    ShowItem();
                    break;
                }
            }
        }
        stopKeyInput = false;
        go_OOC.SetActive(false);
    }
}