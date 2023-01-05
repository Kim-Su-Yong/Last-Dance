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

    public CinemachineVirtualCamera followCam;  // ĳ���͸� �����ϴ� Virtual ī�޶�
    public CinemachineVirtualCamera npcCam;     // NPC�� Ȯ�����ִ� Virtual ī�޶�

    public bool isAction;           // ���� ��ȣ�ۿ� ������ Ȯ��

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

        if (talkData == null)    // ���̻� ��ȭ�� ������ ���ٸ� ��ȭ ����
        {
            isAction = false;
            followCam.gameObject.SetActive(true);          // �ȷο� ī�޶� Ȱ��ȭ
            npcCam.gameObject.SetActive(false);   // NPC Ȯ�� ī�޶� ��Ȱ��ȭ

            talkImage.gameObject.SetActive(false);
            canvasUI.gameObject.SetActive(true);

            talkIndex = 0;
            return;
        }
            
        talkText.text = talkData;

        isAction = true;
        followCam.gameObject.SetActive(false);          // �ȷο� ī�޶� ��Ȱ��ȭ
        npcCam.gameObject.SetActive(true);   // NPC Ȯ�� ī�޶� Ȱ��ȭ

        talkImage.gameObject.SetActive(true);
        canvasUI.gameObject.SetActive(false);
        talkIndex++;        // ���� �������� �Ѿ
    }
}
