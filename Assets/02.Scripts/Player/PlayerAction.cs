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

    void Interaction()
    {
        if (Time.timeScale == 0)
            return;
        if (Input.GetKeyDown(KeyCode.F))                // fŰ�� ������ ��ȣ�ۿ� �߻�
        {
            if (nearObject == null) return;             // ��ó�� ������Ʈ�� ������ ����
                                                        //theSound.Play(call_sound);                  // ȣ�� �Ҹ� ���

            //QuestUIManager.uiManager.NPCPanelActivation();
            if (g_manager.isAction)
                playerState.state = PlayerState.State.TALK;
            //else
            //{
            //    playerState.state = PlayerState.State.IDLE;
            //    GameManager.instance.ActionEnd();
            //}
            //g_manager.Action(nearObject);                         // ��ȣ�ۿ� ����
            g_manager.OpenShop();
        }

    }
    private void Awake()
    {
        playerState = GetComponent<PlayerState>();
    }
    private void Update()
    {
        Interaction();
    }
    private void OnEnable()
    {
        //StartCoroutine(FindNearObject());
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
