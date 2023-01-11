using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    private StandardInput cursorLock;

    public Button UseButton;
    public Text UseText; //장비일 경우 장착, 소비일 경우 사용
    public GameObject EquipSlot;
    public GameObject ConsumeSlot;
    public GameObject selectedItem; //장착, 사용하려고 선택된 아이템
    public GameObject[] equipmentslots; //장비창의 슬롯들
    private string equipType;
    private string equipType2;
    public bool isUse = false;
    private ItemInfo itemInfo;
    private PlayerDamage playerDamage;
    PlayerState playerState;

    public GameObject invenGO; // 인벤토리 오브젝트
    public GameObject equipGO; // 장비창 오브젝트
    public GameObject shopGO;
    public GameObject prefab_floating_text;
    public Transform messageTr;

    public bool activeInven; //인벤토리가 켜지면 true
    public bool activeEquip; //장비창이 켜지면 true
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
            UseText.text = "장착";
        }
        else if (ConsumeSlot.activeSelf == true)
        {
            UseButton.gameObject.SetActive(true);
            UseText.text = "사용";
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

    void ShowInven()    // 인벤토리창이 닫히는 경우는 2가지(x버튼 누르기, i키 누르기)
    {                   // x버튼 누를경우 커서락이 변동되지 않는 버그 수정해야함
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
        if (Input.GetKeyDown(KeyCode.E) && !activeShop && !activeMenu && !isEnd) //상점이 열려있으면 장비창이 열리지 않도록
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
        if (Input.GetKeyDown(KeyCode.G) && !activeEquip && !activeMenu) //장비창이 열려있으면 상점이 열리지 않도록
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
        if (EquipSlot.activeSelf == true) //장비창 슬롯이 활성화 되어있을 경우
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
        else if (ConsumeSlot.activeSelf == true) //소비창 슬롯이 활성화 되어있을 경우
        {
            if (playerDamage.curHp == PlayerStat.instance.maxHP)
            {
                GameObject clone = Instantiate(prefab_floating_text, messageTr.position, Quaternion.Euler(Vector3.zero));
                clone.GetComponent<FloatingText>().text.text = "포션을 사용할 수 없습니다.";
                clone.transform.SetParent(messageTr.transform);
                return;
            }
            if (itemInfo.itemCount > 1)
            {
                GameObject clone = Instantiate(prefab_floating_text, messageTr.position, Quaternion.Euler(Vector3.zero));
                //clone.GetComponent<FloatingText>().text.text = "체력을 " + itemInfo.AddHp.ToString() + " 만큼 회복합니다.";
                clone.GetComponent<FloatingText>().text.text = "포션을 사용합니다.";
                clone.transform.SetParent(messageTr.transform);
                playerDamage.RestoreHp(itemInfo.AddHp);
                playerDamage.UsePotionEffect();
                itemInfo.itemCount--;
                itemInfo.GetComponent<HoverTip>().itemCount--;
                itemInfo.GetComponent<HoverTip>().countToShow =
                    "수량 : " + itemInfo.GetComponent<HoverTip>().itemCount.ToString() + "개";
                itemInfo.GetComponent<InventorySlot>().itemCount--;
                itemInfo.GetComponent<InventorySlot>().itemCount_Text.text =
                    itemInfo.GetComponent<InventorySlot>().itemCount.ToString();
            }
            else if (itemInfo.itemCount == 1)
            {
                GameObject clone = Instantiate(prefab_floating_text, messageTr.position, Quaternion.Euler(Vector3.zero));
                clone.GetComponent<FloatingText>().text.text = "포션을 사용합니다.";
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