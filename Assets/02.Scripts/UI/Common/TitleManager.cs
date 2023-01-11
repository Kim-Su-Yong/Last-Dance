using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class TitleManager : MonoBehaviour
{
    private SoundManager theSound;
    private SaveNLoad theSave;
    private GameObject themenu;
    private GameObject thetitle;

    public string click_sound;
    void Start()
    {
        theSound = FindObjectOfType<SoundManager>();
        theSave = FindObjectOfType<SaveNLoad>();
        themenu = GameObject.Find("Canvas_Menu").transform.GetChild(0).gameObject;
        thetitle = GameObject.Find("Canvas_Title").transform.GetChild(0).gameObject;
    }
    private void OnEnable()
    {
        Cursor.lockState = CursorLockMode.None;
        //thetitle.gameObject.SetActive(true);
    }
    public void StartGame()
    {
        theSound.Play(click_sound);
        themenu.gameObject.SetActive(false);
        thetitle.gameObject.SetActive(false);
        SceneLoader.Instance.LoadScene("MainScene");
    }
    public void SaveGame()
    {
        theSound.Play(click_sound);
        theSave.CallSave();
    }
    public void LoadGame()
    {
        theSound.Play(click_sound);
        themenu.gameObject.SetActive(false);
        SceneLoader.Instance.LoadScene("MainScene");
    }
    public void ExitGame()
    {
        theSound.Play(click_sound);
        Application.Quit();
    }
}
