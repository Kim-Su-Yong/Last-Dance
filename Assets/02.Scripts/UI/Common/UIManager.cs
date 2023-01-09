using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    private StandardInput cursorLock;

    public Button UseButton;
    public Text UseText; //����� ��� ����, �Һ��� ��� ���
    public GameObject EquipSlot;
    public GameObject ConsumeSlot;
    public GameObject selectedItem; //����, ����Ϸ��� ���õ� ������
    public GameObject[] equipmentslots; //���â�� ���Ե�
    private string equipType;
    private string equipType2;
    public bool isUse = false;
    private ItemInfo itemInfo;
    private PlayerDamage playerDamage;
    PlayerState playerState;

    public GameObject invenGO; // �κ��丮 ������Ʈ
    public GameObject equipGO; // ���â ������Ʈ
    public GameObject shopGO;
    public GameObject prefab_floating_text;
    public Transform messageTr;

    public bool activeInven; //�κ��丮�� ������ true
    public bool activeEquip; //���â�� ������ true
    public bool activeShop;

    public GameObject deathPanel;

    void Start()
    {
        equipmentslots = GameObject.FindGameObjectsWithTag("Equipment");
        playerDamage = FindObjectOfType<PlayerDamage>();
        playerState = FindObjectOfType<PlayerState>();
        cursorLock = FindObjectOfType<StandardInput>();
        equipGO.SetActive(false);
        invenGO.SetActive(false);

        deathPanel = GameObject.Find("Canvas_UI").transform.GetChild(5).gameObject;
    }
    void Update()
    {
        if (EquipSlot.activeSelf == true)
        {
            UseButton.gameObject.SetActive(true);
            UseText.text = "����";
        }
        else if (ConsumeSlot.activeSelf == true)
        {
            UseButton.gameObject.SetActive(true);
            UseText.text = "���";
        }
        else
            UseButton.gameObject.SetActive(false);

        if(!(playerDamage.isDie))
        {
            ShowInven();
            ShowEquip();
            ShowShop();
            deathPanel.SetActive(false);
        }
        
        if (isUse)
        {
            itemInfo = selectedItem.GetComponent<ItemInfo>();
            isUse = false;
        }

        if (!activeEquip && !activeInven && !activeShop)
            CursorLock(true);

        StartCoroutine(deathPanelShow());
        
    }

    void ShowInven()    // �κ��丮â�� ������ ���� 2����(x��ư ������, iŰ ������)
    {                   // x��ư ������� Ŀ������ �������� �ʴ� ���� �����ؾ���
        if (Input.GetKeyDown(KeyCode.I))
        {
            //activeInven = !activeInven;
            if (!activeInven)
            {
                activeInven = true;
                CursorLock(false);
                invenGO.SetActive(true);
            }
            else
            {
                CloseInven();
            }
        }
    }
    void ShowEquip()
    {
        if (Input.GetKeyDown(KeyCode.E) && !activeShop) //������ ���������� ���â�� ������ �ʵ���
        {
            //activeEquip = !activeEquip;
            if (!activeEquip)
            {
                activeEquip = true;
                CursorLock(false);
                equipGO.SetActive(true);
            }
            else
            {
                CloseEquip();
            }
        }
    }
    void ShowShop()
    {
        if (Input.GetKeyDown(KeyCode.G) && !activeEquip) //���â�� ���������� ������ ������ �ʵ���
        {
            //activeShop = !activeShop;
            if (!activeShop)
            {
                activeShop = true;
                activeInven = true;
                CursorLock(false);                
                shopGO.SetActive(true);
                invenGO.SetActive(true);
            }
            else
            {
                CloseShop();
            }
        }
    }


    public void OnclickUse()
    {
        if (EquipSlot.activeSelf == true) //���â ������ Ȱ��ȭ �Ǿ����� ���
        {
            for (int i = 0; i < equipmentslots.Length; i++)
            {
                equipType = equipmentslots[i].GetComponent<Drop>().equipType.ToString();
                equipType2 = itemInfo.equipType.ToString();
                if (equipType2 == equipType && equipmentslots[i].transform.childCount == 0)
                {
                    GameObject clone = Instantiate(selectedItem, Vector3.zero, Quaternion.identity);
                    clone.transform.SetParent(equipmentslots[i].transform);
                    GameObject Textclone = Instantiate(prefab_floating_text, messageTr.position, Quaternion.Euler(Vector3.zero));
                    Textclone.GetComponent<FloatingText>().text.text = itemInfo.GetComponent<HoverTip>().countToShow.ToString();
                    Textclone.transform.SetParent(messageTr.transform);
                    PlayerStat.instance.atk += itemInfo.Atk;
                    PlayerStat.instance.def += itemInfo.Def;
                    PlayerStat.instance.speed += itemInfo.Speed;
                    PlayerStat.instance.maxHP += itemInfo.AddHp;
                    playerDamage.hpUpdate();
                    Inventory.instance.equipmentItemList.Add(itemInfo);
                    Destroy(selectedItem.gameObject);
                }
            }
        }
        else if (ConsumeSlot.activeSelf == true) //�Һ�â ������ Ȱ��ȭ �Ǿ����� ���
        {
            if (playerDamage.curHp == PlayerStat.instance.maxHP)
            {
                GameObject clone = Instantiate(prefab_floating_text, messageTr.position, Quaternion.Euler(Vector3.zero));
                clone.GetComponent<FloatingText>().text.text = "������ ����� �� �����ϴ�.";
                clone.transform.SetParent(messageTr.transform);
                return;
            }
            if (itemInfo.itemCount > 1)
            {
                GameObject clone = Instantiate(prefab_floating_text, messageTr.position, Quaternion.Euler(Vector3.zero));
                //clone.GetComponent<FloatingText>().text.text = "ü���� " + itemInfo.AddHp.ToString() + " ��ŭ ȸ���մϴ�.";
                clone.GetComponent<FloatingText>().text.text = "������ ����մϴ�.";
                clone.transform.SetParent(messageTr.transform);
                playerDamage.RestoreHp(itemInfo.AddHp);
                playerDamage.UsePotionEffect();
                itemInfo.itemCount--;
                itemInfo.GetComponent<HoverTip>().itemCount--;
                itemInfo.GetComponent<HoverTip>().countToShow =
                    "���� : " + itemInfo.GetComponent<HoverTip>().itemCount.ToString() + "��";
                itemInfo.GetComponent<InventorySlot>().itemCount--;
                itemInfo.GetComponent<InventorySlot>().itemCount_Text.text =
                    itemInfo.GetComponent<InventorySlot>().itemCount.ToString();
            }
            else if (itemInfo.itemCount == 1)
            {
                GameObject clone = Instantiate(prefab_floating_text, messageTr.position, Quaternion.Euler(Vector3.zero));
                clone.GetComponent<FloatingText>().text.text = "������ ����մϴ�.";
                clone.transform.SetParent(messageTr.transform);
                playerDamage.RestoreHp(itemInfo.AddHp);
                playerDamage.UsePotionEffect();
                Inventory.instance.inventoryItemList.Remove(itemInfo);
                //selectedItem = null;
                Destroy(itemInfo.gameObject);
            }
        }


    }

    IEnumerator deathPanelShow()
    {
        if(playerDamage.isDie)
        {
            cursorLock.cursorLocked = false;
            cursorLock.SetCursorState(cursorLock.cursorLocked);
            deathPanel.SetActive(true);
        }
        yield return null;
    }

    void CursorLock(bool locked)
    {
        //Time.timeScale = locked ? 1 : 0;
         
        cursorLock.cursorLocked = locked;
        cursorLock.cursorInputForLook = locked;
        cursorLock.SetCursorState(cursorLock.cursorLocked);
    }

    public void CloseEquip()
    {
        activeEquip = false;
        //CursorLock(true);
        equipGO.SetActive(false);
    }

    public void CloseShop()
    {
        activeShop = false;
        activeInven = false;
        //CursorLock(true);
        shopGO.SetActive(false);
        invenGO.SetActive(false);
    }

    public void CloseInven()
    {
        activeInven = false;
        //CursorLock(true);
        invenGO.SetActive(false);
    }
    
}