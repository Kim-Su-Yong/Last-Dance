using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GameManager : MonoBehaviour
{
    private PlayerAction thePlayer;
    //private FadeManager theFade;
    [SerializeField]
    private Image image; //������ ȭ��
    [SerializeField]
    private GameObject button; //���� ����
    [SerializeField]
    private GameObject button1; //���� ����
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
        Debug.Log("���̵���");
        button1.SetActive(false);
        StartCoroutine(FadeInCoroutine());
    }
    IEnumerator FadeInCoroutine()
    {
        float fadeCount = 1; //ó�� ���İ�
        while (fadeCount > 0) //���� �ִ밪 1.0���� �ݺ�
        {
            fadeCount -= 0.02f;
            yield return new WaitForSeconds(0.005f); //n�ʸ��� ����(�������� ���� ����)
            image.color = new Color(0, 0, 0, fadeCount); //�ش� ���������� ���İ� ����
        }
    }
    public void FadeOut()
    {
        Debug.Log("���̵�ƿ�");
        button.SetActive(false);
        StartCoroutine(FadeOutCoroutine());
    }
    IEnumerator FadeOutCoroutine()
    {
        float fadeCount = 0; //ó�� ���İ�
        while(fadeCount < 1) //���� �ִ밪 1.0���� �ݺ�
        {
            fadeCount += 0.02f;
            yield return new WaitForSeconds(0.005f); //n�ʸ��� ����(�������� ���� �ƿ���)
            image.color = new Color(0, 0, 0, fadeCount); //�ش� ���������� ���İ� ����
        }
    }
}
