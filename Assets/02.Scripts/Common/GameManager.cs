using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;
public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public TalkManager talkManager;
    public int talkIndex;

    private PlayerAction thePlayer;
    public GameObject talkImage;
    public Text talkText;
    public GameObject canvasUI;

    public CinemachineVirtualCamera followCam;  // 캐릭터를 추적하는 Virtual 카메라
    public CinemachineVirtualCamera npcCam;     // NPC를 확대해주는 Virtual 카메라

    public bool isAction;           // 현재 상호작용 중인지 확인

    void Start()
    {
        instance = this;

        talkImage = GameObject.Find("Canvas_Conversation").transform.GetChild(2).gameObject;
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
    public void Action(GameObject nearObject)
    {            
        ObjData objData = nearObject.GetComponent<ObjData>();

        Talk(objData.id);

    }

    void Talk(int id)
    {
        string talkData = talkManager.GetTalk(id, talkIndex);

        if (talkData == null)    // 더이상 대화할 문장이 없다면 대화 종료
        {
            isAction = false;
            followCam.gameObject.SetActive(true);          // 팔로우 카메라 활성화
            npcCam.gameObject.SetActive(false);   // NPC 확대 카메라 비활성화

            talkImage.gameObject.SetActive(false);
            canvasUI.gameObject.SetActive(true);

            talkIndex = 0;
            return;
        }
            
        talkText.text = talkData;

        isAction = true;
        followCam.gameObject.SetActive(false);          // 팔로우 카메라 비활성화
        npcCam.gameObject.SetActive(true);   // NPC 확대 카메라 활성화

        talkImage.gameObject.SetActive(true);
        canvasUI.gameObject.SetActive(false);
        talkIndex++;        // 다음 문장으로 넘어감
    }
}
