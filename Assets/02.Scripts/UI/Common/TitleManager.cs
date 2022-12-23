using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class TitleManager : MonoBehaviour
{
    private SoundManager theSound;

    public string click_sound;
    void Start()
    {
        theSound = FindObjectOfType<SoundManager>();
    }
    public void NewGame()
    {
        //�־�� �ϳ�?
    }
    public void LoadGame()
    {
        theSound.Play(click_sound);
        SceneLoader.Instance.LoadScene("MainScene");
    }
    public void ExitGame()
    {
        theSound.Play(click_sound);
        Application.Quit();
    }
}
