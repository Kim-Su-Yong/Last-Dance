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

    public Image coolImg; //��Ÿ�� �̹���
    public Text cooltxt;  //��Ÿ�� �ð� �ؽ�Ʈ
    public bool isCool;   //��Ÿ������ �Ǵ�

    public GameObject invenGO; // �κ��丮 ������Ʈ
    public GameObject equipGO; // ���â ������Ʈ
    public GameObject shopGO;
    public GameObject prefab_floating_text;
    public Transform messageTr;

    public bool activeInven; //�κ��丮�� ������ true
    public bool activeEquip; //���â�� ������ true
    public bool activeShop;

    void Start()
    {
        equipmentslots = GameObject.FindGameObjectsWithTag("Equipment");
        playerDamage = FindObjectOfType<PlayerDamage>();
        cursorLock = FindObjectOfType<StandardInput>();
        equipGO.SetActive(false);
        invenGO.SetActive(false);
        coolImg.fillAmount = 0f;
        cooltxt.enabled = false;
        coolImg.enabled = false;
        isCool = true;
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

        if (Input.GetKeyDown(KeyCode.X) && isCool)
        {
            isCool = false;
            cooltxt.enabled = true;
            coolImg.enabled = true;
            StartCoroutine(CoolTime(3f));
        }
        ShowInven();
        ShowEquip();
        ShowShop();
        if (isUse)
        {
            itemInfo = selectedItem.GetComponent<ItemInfo>();
            isUse = false;
        }
    }
    IEnumerator CoolTime(float cool)
    {
        float cTxt = cool;
        while (cTxt > 0)
        {
            cTxt -= Time.deltaTime;
            coolImg.fillAmount = cTxt / cool;
            cooltxt.text = cTxt.ToString("0.0");
            yield return new WaitForFixedUpdate();
        }
        coolImg.fillAmount = 0f;
        cooltxt.enabled = false;
        coolImg.enabled = false;
        isCool = true;
    }
    void ShowInven()    // �κ��丮â�� ������ ���� 2����(x��ư ������, iŰ ������)
    {                   // x��ư ������� Ŀ������ �������� �ʴ� ���� �����ؾ���
        if (Input.GetKeyDown(KeyCode.I))
        {
            activeInven = !activeInven;
            if (activeInven)
            {
                cursorLock.cursorLocked = false;
                cursorLock.SetCursorState(cursorLock.cursorLocked);
                invenGO.SetActive(true);
            }
            else
            {
                if (!activeEquip)
                {
                    cursorLock.cursorLocked = true;
                    cursorLock.SetCursorState(cursorLock.cursorLocked);
                }
                invenGO.SetActive(false);
            }
        }
    }
    void ShowEquip()
    {
        if (Input.GetKeyDown(KeyCode.E) && !activeShop) //������ ���������� ���â�� ������ �ʵ���
        {
            activeEquip = !activeEquip;
            if (activeEquip)
            {
                cursorLock.cursorLocked = false;
                cursorLock.SetCursorState(cursorLock.cursorLocked);
                equipGO.SetActive(true);
            }
            else
            {
                if (!activeInven)
                {
                    cursorLock.cursorLocked = true;
                    cursorLock.SetCursorState(cursorLock.cursorLocked);
                }
                equipGO.SetActive(false);
            }
        }
    }
    void ShowShop()
    {
        if (Input.GetKeyDown(KeyCode.G) && !activeEquip) //���â�� ���������� ������ ������ �ʵ���
        {
            activeShop = !activeShop;
            if (activeShop)
            {
                shopGO.SetActive(true);
                invenGO.SetActive(true);
            }
            else
            {
                shopGO.SetActive(false);
                invenGO.SetActive(false);
            }
        }
    }
    public void CloseShop()
    {
        shopGO.SetActive(false);
        invenGO.SetActive(false);
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
}