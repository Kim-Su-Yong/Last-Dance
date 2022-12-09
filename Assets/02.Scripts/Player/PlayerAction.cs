using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAction : MonoBehaviour
{
    [SerializeField]
    GameObject nearObject;  // 캐릭터와 가장 가까운 오브젝트를 저장
    PlayerState playerState;
    public static PlayerAction instance;
    public string currentMapName;
    public string currentSceneName;
    private SaveNLoad theSaveNLoad;
    void Interaction()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (nearObject == null) return;
            /*
             * 충분히 가까워진 상태에서 e키를 누르면 해당 상호작용 팝업창 출력
             */
            if (nearObject.CompareTag("NPC"))
            {
                NPCInterAction npc = nearObject.GetComponent<NPCInterAction>();
                npc.IsTalk = true;
                // 플레이어 이동, 공격 등 액션 제한 구현 필요
            }
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

        if (Input.GetKeyDown(KeyCode.F5)) //저장
        {

        }
        if (Input.GetKeyDown(KeyCode.F9)) //불러오기
        {

        }
    }

    IEnumerator FindNearObject()
    {
        //0.3초마다 
        while(playerState.state != PlayerState.State.DIE)
        {
            yield return new WaitForSeconds(0.3f);
            // 0.3초마다 20범위 안에 있는 콜라이더들을 찾는다
            Collider[] Cols = Physics.OverlapSphere(transform.position, 20f);

            foreach (Collider col in Cols)
            {
                if (col.CompareTag("ENEMY"))
                    nearObject = col.gameObject;
            }
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
