using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Title : MonoBehaviour
{
    //private FadeManager theFade; //�����Ҷ� ȭ���� ��ο����ٰ� �����, �ε�â���� ������ ����
    private SoundManager theSound;
    private PlayerAction thePlayer;
    private GameManager theGM;

    public string click_sound;
    void Start()
    {
        //theFade =
        theSound = FindObjectOfType<SoundManager>();
        thePlayer = FindObjectOfType<PlayerAction>();
        theGM = FindObjectOfType<GameManager>();
    }
    public void StartGame()
    {
        StartCoroutine(GameStartCoroutine());
    }
    IEnumerator GameStartCoroutine()
    {
        //theFade.FadeOut();
        theSound.Play(click_sound);
        yield return new WaitForSeconds(2f);
        Color color = thePlayer.GetComponent<SpriteRenderer>().color;
        color.a = 1f;
        thePlayer.GetComponent<SpriteRenderer>().color = color;
        thePlayer.currentSceneName = "MainScene";

        theGM.LoadStart();
    }
}
