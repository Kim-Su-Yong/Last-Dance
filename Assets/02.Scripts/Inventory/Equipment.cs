using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Equipment : MonoBehaviour
{
    private OrderManager theOrder;
    private SoundManager theSound;
    private PlayerStat thePlayerStat;
    private Inventory theInven;

    public string key_sound;
    public string enter_sound;
    public string open_sound;
    public string close_sound;
    public string takeoff_sound;

    private const int WEAPON = 0, SHILED = 1, AMULT = 2, LEFT_RING = 3, RIGHT_RING = 4,
                      HELMET = 5, ARMOR = 6, LEFT_GLOVE = 7, RIGHT_GLOVE = 8, BELT = 9,
                      LEFT_BOOTS = 10, RIGHT_BOOTS = 11;

    public GameObject go;
    public Text[] text; //스탯
    public Image[] img_slots; //장비 아이콘들
    public GameObject go_selected_Slot_UI; //선택된 장비 슬롯 UI

    public Item[] equipItemList; //장착된 장비 리스트
    private int selectedSlot; //선택된 장비 슬롯
    public bool activated;
    private bool inputKey;
    void Start()
    {
        theInven = FindObjectOfType<Inventory>();
        theOrder = FindObjectOfType<OrderManager>();
        theSound = FindObjectOfType <SoundManager>();
        thePlayerStat = FindObjectOfType<PlayerStat>();
    }
    public void SelectedSlot()
    {
        go_selected_Slot_UI.transform.position = img_slots[selectedSlot].transform.position;
    }
    public void ClearEquip()
    {
        Color color = img_slots[0].color;
        color.a = 0f;

        for(int i =0; i < img_slots.Length; i++)
        {
            img_slots[i].sprite = null;
            img_slots[i].color = color;
        }
    }
    public void ShowEquip()
    {
        Color color = img_slots[0].color;
        color.a = 1f;

        for(int i = 0; i< img_slots.Length; i++)
        {
            if(equipItemList[i].itemID != 0)
            {
                img_slots[i].sprite = equipItemList[i].itemIcon;
                img_slots[i].color = color;
            }
        }
    }
    void Update()
    {
        if(inputKey)
        {
            if(Input.GetKeyDown(KeyCode.E))
            {
                activated = !activated;

                if(activated)
                {
                    //theOrder.NotMove();
                    theSound.Play(open_sound);
                    go.SetActive(true);
                    selectedSlot = 0;
                    ClearEquip();
                    ShowEquip();
                }
                else
                {
                    //theOrder.NotMove();
                    theSound.Play(close_sound);
                    go.SetActive(false);
                    ClearEquip();
                }
            }

            if(activated)
            {
                if(Input.GetKeyDown(KeyCode.DownArrow))
                {
                    if (selectedSlot < img_slots.Length - 1)
                        selectedSlot++;
                    else
                        selectedSlot = 0;
                    theSound.Play(key_sound);
                    SelectedSlot();
                }
                else if (Input.GetKeyDown(KeyCode.RightArrow))
                {
                    if (selectedSlot < img_slots.Length - 1)
                        selectedSlot++;
                    else
                        selectedSlot = 0;
                    theSound.Play(key_sound);
                    SelectedSlot();
                }
                else if (Input.GetKeyDown(KeyCode.LeftArrow))
                {
                    if (selectedSlot > 0)
                        selectedSlot--;
                    else
                        selectedSlot = img_slots.Length - 1;
                    theSound.Play(key_sound);
                    SelectedSlot();
                }
                else if (Input.GetKeyDown(KeyCode.UpArrow))
                {
                    if (selectedSlot > 0)
                        selectedSlot--;
                    else
                        selectedSlot = img_slots.Length - 1;
                    theSound.Play(key_sound);
                    SelectedSlot();               
                }
                else if (Input.GetKeyDown(KeyCode.Z))
                {
                    theSound.Play(enter_sound);
                    inputKey = false;

                }
            }
        }
    }
}
