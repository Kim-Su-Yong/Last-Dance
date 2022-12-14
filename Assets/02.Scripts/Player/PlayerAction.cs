using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAction : MonoBehaviour
{
    [SerializeField]
    GameObject nearObject;  // ĳ���Ϳ� ���� ����� ������Ʈ�� ����
    PlayerState playerState;
    public GameManager g_manager;
    public static PlayerAction instance;
    public string currentMapName;
    public string currentSceneName;
    GameObject scanObject;
    public SoundManager theSound;
    public string call_sound;
    //private SaveNLoad theSaveNLoad;

    void Interaction()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (nearObject == null) return;
            theSound.Play(call_sound);
            g_manager.Action();
            /*
             * ����� ������� ���¿��� eŰ�� ������ �ش� ��ȣ�ۿ� �˾�â ���
             */
            if (nearObject.CompareTag("NPC"))
            {
                NPCInterAction npc = nearObject.GetComponent<NPCInterAction>();
                npc.IsTalk = true;
                // �÷��̾� �̵�, ���� �� �׼� ���� ���� �ʿ�
            }
        }
        //if (Input.GetKeyDown(KeyCode.F) && CompareTag("NPC"))
            
    }
    private void Awake()
    {
        playerState = GetComponent<PlayerState>();
    }
    private void OnEnable()
    {
        //StartCoroutine(FindNearObject());
    }
    void Start()
    {
        //theSaveNLoad = FindObjectOfType<SaveNLoad>();
    }
    void Update()
    {
        Interaction();
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("NPC"))
        {
            nearObject = other.gameObject;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("NPC"))
        {
            nearObject = null;
        }
    }
}
