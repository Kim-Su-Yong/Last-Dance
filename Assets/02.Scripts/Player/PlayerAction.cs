using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAction : MonoBehaviour
{
    [SerializeField]
    GameObject nearObject;  // ĳ���Ϳ� ���� ����� ������Ʈ�� ����
    public PlayerState playerState;
    public GameManager g_manager;
    public static PlayerAction instance;
    GameObject scanObject;
    public SoundManager theSound;
    public string call_sound;
    private SaveNLoad theSaveNLoad;

    void Interaction()
    {    
        if (Input.GetKeyDown(KeyCode.F))                // fŰ�� ������ ��ȣ�ۿ� �߻�
        {
            if (nearObject == null) return;             // ��ó�� ������Ʈ�� ������ ����
            theSound.Play(call_sound);                  // ȣ�� �Ҹ� ���

            g_manager.Action(nearObject);                         // ��ȣ�ۿ� ����
            if (g_manager.isAction)
                playerState.state = PlayerState.State.TALK;
            else
                playerState.state = PlayerState.State.IDLE;
        }

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
        theSaveNLoad = FindObjectOfType<SaveNLoad>();
    }
    void Update()
    {
        Interaction();
        if (Input.GetKeyDown(KeyCode.F5))
        {
            theSaveNLoad.CallSave();
        }
        if (Input.GetKeyDown(KeyCode.F9))
        {
            theSaveNLoad.CallLoad();
        }
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
