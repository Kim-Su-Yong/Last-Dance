using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class TitleManager : MonoBehaviour
{
    private SoundManager theSound;
    private PlayerAction thePlayer;
    private GameManager theGM;

    public string click_sound;
    void Start()
    {
        theSound = FindObjectOfType<SoundManager>();
        thePlayer = FindObjectOfType<PlayerAction>();
        theGM = FindObjectOfType<GameManager>();
    }
    public void NewGame()
    {
        //넣어야 하나?
    }
    public void LoadGame()
    {
        //StartCoroutine(GameStartCoroutine());
        theSound.Play(click_sound);
        
        SceneManager.LoadScene("MainScene");
        theGM.FadeIn();
    }
    IEnumerator GameStartCoroutine()
    {
        //theFade.FadeOut();
        theSound.Play(click_sound);
        yield return new WaitForSeconds(2f);
        //Color color = thePlayer.GetComponent<SpriteRenderer>().color;
        //color.a = 1f;
        //thePlayer.GetComponent<SpriteRenderer>().color = color;
        //thePlayer.currentSceneName = "MainScene";
        theGM.LoadStart();
        SceneManager.LoadScene("MainScene");
    }
    public void ExitGame()
    {
        theSound.Play(click_sound);
        Application.Quit();
    }
}
