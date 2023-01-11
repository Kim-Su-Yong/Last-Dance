using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public static GameManager gameManager;

    public TalkManager talkManager;
    public int talkIndex;

    private PlayerState thePlayer;
    public SoundManager theSound;
    public string call_sound;
    public GameObject talkImage;
    public Text talkText;
    public GameObject canvasUI;

    public CinemachineVirtualCamera followCam;  // 캐릭터를 추적하는 Virtual 카메라
    public CinemachineVirtualCamera npcCam;     // NPC를 확대해주는 Virtual 카메라

    public bool isAction;           // 현재 상호작용 중인지 확인

    public bool master;
    [HideInInspector]
    private bool isObjectsOn; // 해당 오브젝트가 있는건지 확인
    public static bool isBossDead; // 보스 죽었는지 확인
    public GameObject[] scenaryObjects;
    [HideInInspector]
    public Material objectsMaterial; // 무너지는 바위를 구현하기 위한 메테리얼
    public GameObject destructibleObjects; // 무너지는 바위를 구현하기 위한 오브젝트 선언
    public GreatSwordScript greatSword;

    public bool playerIsDead;

    private void Awake()
    {
        if (gameManager == null)
            gameManager = this;
        else
            Destroy(gameObject);
        DontDestroyOnLoad(this.gameObject);

        CheckForChanges(); // 보스 및 오브젝트에 관련된 확인사항들 체크
    }
    void Start()
    {
        instance = this;
        thePlayer = FindObjectOfType<PlayerState>();

        talkImage = GameObject.Find("Canvas_Conversation").transform.GetChild(0).gameObject;
        canvasUI = GameObject.Find("Canvas_UI");
    }
    void Update()
    {
    }
    public void Action(GameObject nearObject)
    {            
        ObjData objData = nearObject.GetComponent<ObjData>();

        Talk(objData.id);
    }

    public void ActionEnd()
    {
        followCam.gameObject.SetActive(true); // 팔로우 카메라 활성화
        npcCam.gameObject.SetActive(false);   // NPC 확대 카메라 비활성화
    }
    public void OpenShop()
    {
        if (isAction) return;
        thePlayer.state = PlayerState.State.TALK;
        theSound.Play(call_sound);
        isAction = true;
        followCam.gameObject.SetActive(false); // 팔로우 카메라 비활성화
        npcCam.gameObject.SetActive(true);   // NPC 확대 카메라 활성화
        UIManager.instance.Shop();
    }

    void Talk(int id)
    {
        string talkData = talkManager.GetTalk(id, talkIndex);

        if (talkData == null)    // 더이상 대화할 문장이 없다면 대화 종료
        {
            isAction = false;
            followCam.gameObject.SetActive(true); // 팔로우 카메라 활성화
            npcCam.gameObject.SetActive(false);   // NPC 확대 카메라 비활성화

            talkImage.gameObject.SetActive(false);
            canvasUI.gameObject.SetActive(true);

            talkIndex = 0;

            UIManager.instance.Shop();
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

    public void CheckForChanges()
    {
        if (!PlayerPrefs.HasKey("IsObjectsOn")) PlayerPrefs.SetInt("IsObjectsOn", 1);
        if (!PlayerPrefs.HasKey("BetterColliders")) PlayerPrefs.SetInt("BetterColliders", 1);
        isObjectsOn = PlayerPrefs.GetInt("IsObjectsOn") == 1 ? true : false;

        CheckDesctructibleObjetcsState();
        //CheckSwordColliders();
    }

    public void CheckDesctructibleObjetcsState()
    {
        if (!Application.isPlaying) return;
        if (PlayerPrefs.GetInt("IsObjectsOn") == 1)
        {
            destructibleObjects.SetActive(true);
            isObjectsOn = true;
        }
        else
        {
            isObjectsOn = false;
            destructibleObjects.SetActive(false);
        }
    }

    public void CheckSwordColliders()
    {
        if (!Application.isPlaying) return;
        if (PlayerPrefs.GetInt("BetterColliders") == 1)
        {
            greatSword.betterColliders = true;
        }
        else
        {
            greatSword.betterColliders = false;
        }
    }
}
