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
    public bool activeMenu;
    GameObject menu;

    public GameObject deathPanel;

    public SoundManager theSound;
    public string call_sound;

    public bool isEnd;
    void Start()
    {
        instance = this;

        equipmentslots = GameObject.FindGameObjectsWithTag("Equipment");
        playerDamage = FindObjectOfType<PlayerDamage>();
        playerState = FindObjectOfType<PlayerState>();
        cursorLock = FindObjectOfType<StandardInput>();
        equipGO.SetActive(false);
        invenGO.SetActive(false);

        menu = GameObject.Find("Canvas_Menu").transform.GetChild(0).gameObject;

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
            //ShowShop();
            deathPanel.SetActive(false);
        }
        
        if (isUse)
        {
            itemInfo = selectedItem.GetComponent<ItemInfo>();
            isUse = false;
        }

        if (menu.activeSelf == true)
            activeMenu = true;
        else
            activeMenu = false;

        if (!activeEquip && !activeInven && !activeShop && !activeMenu)
            CursorLock(true);

        StartCoroutine(deathPanelShow());

        //if(Input.GetKeyDown(KeyCode.K))
        //{
        //    ChangeMoney(100);
        //}
        //if (Input.GetKeyDown(KeyCode.L))
        //{
        //    ChangeMoney(-100);
        //}
    }

    void ShowInven()    // �κ��丮â�� ������ ���� 2����(x��ư ������, iŰ ������)
    {                   // x��ư ������� Ŀ������ �������� �ʴ� ���� �����ؾ���
        if (Input.GetKeyDown(KeyCode.I) && !activeMenu && !isEnd)
        {
            //activeInven = !activeInven;
            if (!activeInven)
            {
                theSound.Play(call_sound);
                activeInven = true;
                CursorLock(false);
                invenGO.SetActive(true);
                playerState.state = PlayerState.State.TALK;
            }
            else
            {
                CloseInven();
            }
        }
    }
    void ShowEquip()
    {
        if (Input.GetKeyDown(KeyCode.E) && !activeShop && !activeMenu && !isEnd) //������ ���������� ���â�� ������ �ʵ���
        {
            //activeEquip = !activeEquip;
            if (!activeEquip)
            {
                theSound.Play(call_sound);
                activeEquip = true;
                CursorLock(false);
                equipGO.SetActive(true);
                playerState.state = PlayerState.State.TALK;
            }
            else
            {
                CloseEquip();
            }
        }
    }
    void ShowShop()
    {
        if (Input.GetKeyDown(KeyCode.G) && !activeEquip && !activeMenu) //���â�� ���������� ������ ������ �ʵ���
        {
            //activeShop = !activeShop;
            if (!activeShop)
            {
                theSound.Play(call_sound);
                activeShop = true;
                activeInven = true;
                CursorLock(false);                
                shopGO.SetActive(true);
                invenGO.SetActive(true);
                //playerState.state = PlayerState.State.TALK;
            }
            else
            {
                CloseShop();
            }
        }
    }

    public void Shop()
    {
        if (!activeShop)
        {
            theSound.Play(call_sound);
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
        theSound.Play(call_sound);
        activeEquip = false;
        //CursorLock(true);
        equipGO.SetActive(false);
        playerState.state = PlayerState.State.IDLE;
    }

    public void CloseShop()
    {
        theSound.Play(call_sound);
        activeShop = false;
        activeInven = false;
        //CursorLock(true);
        shopGO.SetActive(false);
        invenGO.SetActive(false);
        GameManager.instance.isAction = false;
        GameManager.instance.ActionEnd();
        playerState.state = PlayerState.State.IDLE;
    }

    public void CloseInven()
    {
        theSound.Play(call_sound);
        activeInven = false;
        //CursorLock(true);
        invenGO.SetActive(false);
        playerState.state = PlayerState.State.IDLE;
    }

    public void CloseUI()
    {
        CloseShop();
        CloseEquip();
        CloseInven();
    }
}