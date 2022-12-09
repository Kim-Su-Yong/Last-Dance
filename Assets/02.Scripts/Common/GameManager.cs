using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GameManager : MonoBehaviour
{
    private PlayerAction thePlayer;
    //private FadeManager theFade;
    [SerializeField]
    private Image image; //검은색 화면
    [SerializeField]
    private GameObject button; //삭제 예정
    [SerializeField]
    private GameObject button1; //삭제 예정
    public GameObject FadePannel;

    void Start()
    {
        
    }
    void Update()
    {
        
    }
    public void LoadStart()
    {
        StartCoroutine(LoadWaitCoroutine());
    }
    IEnumerator LoadWaitCoroutine()
    {
        yield return new WaitForSeconds(0.5f);
        thePlayer = FindObjectOfType<PlayerAction>();
        
    }
    public void FadeIn()
    {
        Debug.Log("페이드인");
        button1.SetActive(false);
        StartCoroutine(FadeInCoroutine());
    }
    IEnumerator FadeInCoroutine()
    {
        float fadeCount = 1; //처음 알파값
        while (fadeCount > 0) //알파 최대값 1.0까지 반복
        {
            fadeCount -= 0.02f;
            yield return new WaitForSeconds(0.005f); //n초마다 실행(작을수록 빨리 인함)
            image.color = new Color(0, 0, 0, fadeCount); //해당 변수값으로 알파값 지정
        }
    }
    public void FadeOut()
    {
        Debug.Log("페이드아웃");
        button.SetActive(false);
        StartCoroutine(FadeOutCoroutine());
    }
    IEnumerator FadeOutCoroutine()
    {
        float fadeCount = 0; //처음 알파값
        while(fadeCount < 1) //알파 최대값 1.0까지 반복
        {
            fadeCount += 0.02f;
            yield return new WaitForSeconds(0.005f); //n초마다 실행(작을수록 빨리 아웃함)
            image.color = new Color(0, 0, 0, fadeCount); //해당 변수값으로 알파값 지정
        }
    }
}
