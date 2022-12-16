using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;
public class GameManager : MonoBehaviour
{
    private PlayerAction thePlayer;
    public GameObject talkImage;
    public Text talkText;
    public GameObject canvasUI;

    public CinemachineVirtualCamera followCam;  // 캐릭터를 추적하는 Virtual 카메라
    public CinemachineVirtualCamera npcCam;     // NPC를 확대해주는 Virtual 카메라

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
        if (talkImage.gameObject.activeInHierarchy == false)
        {
            followCam.gameObject.SetActive(false);          // 팔로우 카메라 비활성화
            npcCam.gameObject.gameObject.SetActive(true);   // NPC 확대 카메라 활성화

            talkImage.gameObject.SetActive(true);
            canvasUI.gameObject.SetActive(false);
            talkText.text = "루나NPC입니다. 무엇을 도와드릴까요?";
        }
        else
        {
            followCam.gameObject.SetActive(true);          // 팔로우 카메라 활성화
            npcCam.gameObject.gameObject.SetActive(false);   // NPC 확대 카메라 비활성화

            talkImage.gameObject.SetActive(false);
            canvasUI.gameObject.SetActive(true);
        }
    }
}
