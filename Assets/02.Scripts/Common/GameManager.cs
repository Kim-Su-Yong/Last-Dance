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

    public CinemachineVirtualCamera followCam;  // ĳ���͸� �����ϴ� Virtual ī�޶�
    public CinemachineVirtualCamera npcCam;     // NPC�� Ȯ�����ִ� Virtual ī�޶�

    public bool isAction;           // ���� ��ȣ�ۿ� ������ Ȯ��

    public bool master;
    [HideInInspector]
    private bool isObjectsOn; // �ش� ������Ʈ�� �ִ°��� Ȯ��
    public static bool isBossDead; // ���� �׾����� Ȯ��
    public GameObject[] scenaryObjects;
    [HideInInspector]
    public Material objectsMaterial; // �������� ������ �����ϱ� ���� ���׸���
    public GameObject destructibleObjects; // �������� ������ �����ϱ� ���� ������Ʈ ����
    public GreatSwordScript greatSword;

    public bool playerIsDead;

    private void Awake()
    {
        if (gameManager == null)
            gameManager = this;
        else
            Destroy(gameObject);
        DontDestroyOnLoad(this.gameObject);

        CheckForChanges(); // ���� �� ������Ʈ�� ���õ� Ȯ�λ��׵� üũ
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
        followCam.gameObject.SetActive(true); // �ȷο� ī�޶� Ȱ��ȭ
        npcCam.gameObject.SetActive(false);   // NPC Ȯ�� ī�޶� ��Ȱ��ȭ
    }
    public void OpenShop()
    {
        if (isAction) return;
        thePlayer.state = PlayerState.State.TALK;
        theSound.Play(call_sound);
        isAction = true;
        followCam.gameObject.SetActive(false); // �ȷο� ī�޶� ��Ȱ��ȭ
        npcCam.gameObject.SetActive(true);   // NPC Ȯ�� ī�޶� Ȱ��ȭ
        UIManager.instance.Shop();
    }

    void Talk(int id)
    {
        string talkData = talkManager.GetTalk(id, talkIndex);

        if (talkData == null)    // ���̻� ��ȭ�� ������ ���ٸ� ��ȭ ����
        {
            isAction = false;
            followCam.gameObject.SetActive(true); // �ȷο� ī�޶� Ȱ��ȭ
            npcCam.gameObject.SetActive(false);   // NPC Ȯ�� ī�޶� ��Ȱ��ȭ

            talkImage.gameObject.SetActive(false);
            canvasUI.gameObject.SetActive(true);

            talkIndex = 0;

            UIManager.instance.Shop();
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
