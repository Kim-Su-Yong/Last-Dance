using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAction : MonoBehaviour
{
    [SerializeField]
    GameObject nearObject;  // 캐릭터와 가장 가까운 오브젝트를 저장
    public PlayerState playerState;
    public GameManager g_manager;
    public static PlayerAction instance;
    GameObject scanObject;
    public SoundManager theSound;
    public string call_sound;
    private SaveNLoad theSaveNLoad;

    void Interaction()
    {    
        if (Input.GetKeyDown(KeyCode.F))                // f키를 누르면 상호작용 발생
        {
            if (nearObject == null) return;             // 근처에 오브젝트가 없으면 종료
            theSound.Play(call_sound);                  // 호출 소리 재생

            g_manager.Action(nearObject);                         // 상호작용 실행
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
