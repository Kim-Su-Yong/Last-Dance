using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class TitleManager : MonoBehaviour
{
    [SerializeField]
    private SoundManager theSound;
    [SerializeField]
    private SaveNLoad theSave;
    [SerializeField]
    private Menu themenu;

    public string click_sound;
    void Start()
    {
        theSound = FindObjectOfType<SoundManager>();
        theSave = FindObjectOfType<SaveNLoad>();
        //themenu = FindObjectOfType<Menu>();
    }
    public void SaveGame()
    {
        theSound.Play(click_sound);
        theSave.CallSave();
    }
    public void LoadGame()
    {
        //themenu.Close();
        theSound.Play(click_sound);
        SceneLoader.Instance.LoadScene("MainScene");
    }
    public void ExitGame()
    {
        theSound.Play(click_sound);
        Application.Quit();
    }
}
