using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GameManager : MonoBehaviour
{
    private PlayerAction thePlayer;
    public GameObject talkImage;
    public Text talkText;
    public GameObject canvasUI;
    void Start()
    {
        talkImage = GameObject.Find("Canvas_Conversation").transform.GetChild(0).gameObject;
        canvasUI = GameObject.Find("Canvas_UI");
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
    public void Action()
    {
            if(talkImage.gameObject.activeInHierarchy == false)
            {
                talkImage.gameObject.SetActive(true);
                Time.timeScale = 0f;
                canvasUI.gameObject.SetActive(false);
                talkText.text = "루나NPC입니다. 무엇을 도와드릴까요?";
            }
            else
            {
                talkImage.gameObject.SetActive(false);
                canvasUI.gameObject.SetActive(true);
                Time.timeScale = 1f;
            }
        }
    }
