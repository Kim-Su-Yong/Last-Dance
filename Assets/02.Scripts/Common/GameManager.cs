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

    public CinemachineVirtualCamera followCam;  // ĳ���͸� �����ϴ� Virtual ī�޶�
    public CinemachineVirtualCamera npcCam;     // NPC�� Ȯ�����ִ� Virtual ī�޶�

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
            followCam.gameObject.SetActive(false);          // �ȷο� ī�޶� ��Ȱ��ȭ
            npcCam.gameObject.gameObject.SetActive(true);   // NPC Ȯ�� ī�޶� Ȱ��ȭ

            talkImage.gameObject.SetActive(true);
            canvasUI.gameObject.SetActive(false);
            talkText.text = "�糪NPC�Դϴ�. ������ ���͵帱���?";
        }
        else
        {
            followCam.gameObject.SetActive(true);          // �ȷο� ī�޶� Ȱ��ȭ
            npcCam.gameObject.gameObject.SetActive(false);   // NPC Ȯ�� ī�޶� ��Ȱ��ȭ

            talkImage.gameObject.SetActive(false);
            canvasUI.gameObject.SetActive(true);
        }
    }
}
