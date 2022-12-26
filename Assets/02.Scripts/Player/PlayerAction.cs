using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAction : MonoBehaviour
{
    [SerializeField]
    GameObject nearObject;  // 캐릭터와 가장 가까운 오브젝트를 저장
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
        if (Input.GetKeyDown(KeyCode.F))                // f키를 누르면 상호작용 발생
        {
            if (nearObject == null) return;             // 근처에 오브젝트가 없으면 종료
            theSound.Play(call_sound);                  // 호출 소리 재생
            if (playerState.state != PlayerState.State.TALK)
                playerState.state = PlayerState.State.TALK; // 플레이어 상태 전환 : talk(이동 및 공격 불가)
            else
                playerState.state = PlayerState.State.IDLE;

            g_manager.Action(nearObject);                         // 상호작용 실행
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
