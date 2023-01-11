using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour
{
    public static Menu instance;

    public SoundManager theSound;

    public string call_sound;
    public string cancel_sound;

    public GameObject pauseImg;
    public RectTransform pauseMenu;
    public GameObject player;
    public RectTransform soundMenu;
    public RectTransform screenMenu;

    StandardInput cursorLock;
    //PlayerAttack attack;
    GameObject theUI;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").gameObject;
        pauseImg = GameObject.Find("Canvas_Menu").transform.GetChild(0).gameObject;
        pauseMenu = pauseImg.transform.GetChild(0).GetComponent<RectTransform>();
        soundMenu = pauseImg.transform.GetChild(1).GetComponent<RectTransform>();
        screenMenu = pauseImg.transform.GetChild(2).GetComponent<RectTransform>();
        theUI = GameObject.Find("Canvas_UI");

        cursorLock = player.GetComponent<StandardInput>();
        //attack = player.GetComponent<PlayerAttack>();
    }
    void Update()
    {
        if (UIManager.instance.isEnd) return;
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if (UIManager.instance.activeShop || UIManager.instance.activeInven || UIManager.instance.activeEquip)
            {
                UIManager.instance.CloseUI();
                return;
            }
            else
            {
                theSound.Play(call_sound);
                Pause();
            }
            
        }
    }
    public void Pause()
    {
        if (pauseImg.gameObject.activeInHierarchy == false) // 캔버스 안에 RectTransform 오브젝트 활성화 / 비활성화
        {
            pauseImg.gameObject.SetActive(true);
            if (pauseMenu.gameObject.activeInHierarchy == false)
            {
                pauseMenu.gameObject.SetActive(true);
            }
            Time.timeScale = 0f; // 게임 정지
            CursorLock(false);
            
        }
        else
        {
            Close();
        }
    }
    public void Close()
    {
        pauseImg.gameObject.SetActive(false);
        screenMenu.gameObject.SetActive(false);
        soundMenu.gameObject.SetActive(false);
        Time.timeScale = 1f;
        CursorLock(true);
    }
    public void Sounds(bool isopen)
    {
        if (isopen == true)
        {
            soundMenu.gameObject.SetActive(true);
            pauseMenu.gameObject.SetActive(false);
        }
        if (!isopen)
        {
            soundMenu.gameObject.SetActive(false);
            pauseMenu.gameObject.SetActive(true);
        }
    }
    public void ScreenSetting(bool isopen)
    {
        if (isopen == true)
        {
            screenMenu.gameObject.SetActive(true);
            soundMenu.gameObject.SetActive(false);
            pauseMenu.gameObject.SetActive(false);
        }
        if (!isopen)
        {
            screenMenu.gameObject.SetActive(false);
            soundMenu.gameObject.SetActive(false);
            pauseMenu.gameObject.SetActive(true);
        }
    }
    public void GoToTitle()
    {
        pauseImg.gameObject.SetActive(false);
        screenMenu.gameObject.SetActive(false);
        soundMenu.gameObject.SetActive(false);
        theUI.gameObject.SetActive(false);
        Time.timeScale = 1f;
        SceneLoader.Instance.LoadScene("TitleScene");
    }
    public void Exit()
    {
        Application.Quit();
    }

    void CursorLock(bool locked)
    {
        cursorLock.cursorLocked = locked;
        cursorLock.cursorInputForLook = locked;
        cursorLock.SetCursorState(cursorLock.cursorLocked);
    }
}
