using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAction : MonoBehaviour
{
    [SerializeField]
    GameObject nearObject;  // 캐릭터와 가장 가까운 오브젝트를 저장
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
            if(nearObject.CompareTag("NPC"))
            {
                NPCInterAction npc = nearObject.GetComponent<NPCInterAction>();
                npc.IsTalk = true;
                // 플레이어 이동, 공격 등 액션 제한 구현 필요
            }
        }
    }

    void Start()
    {
        theSaveNLoad = FindObjectOfType<SaveNLoad>();
    }
    void Update()
    {
        Interaction();
        if (Input.GetKeyDown(KeyCode.F9)) //불러오기
        {
            theSaveNLoad.CallLoad();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.CompareTag("NPC"))
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
