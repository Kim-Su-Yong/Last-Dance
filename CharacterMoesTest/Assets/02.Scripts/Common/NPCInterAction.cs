using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCInterAction : MonoBehaviour
{
    Animator animator;

    public bool IsTalk;

    LayerMask playerLayer;
    readonly int hashQuest = Animator.StringToHash("Quest");
    readonly int hashTalk = Animator.StringToHash("Talk");
    float rotSpeed = 10f;
    void Start()
    {
        animator = GetComponent<Animator>();
        playerLayer = LayerMask.NameToLayer("Player");
    }

    void Update()
    {
        if (IsTalk)
        {
            animator.SetBool(hashQuest, true);
            animator.SetBool(hashTalk, false);
        }
            
    }

    private void OnTriggerStay(Collider other)
    {
        // 일정 범위 안에 캐릭터가 감지되면 캐릭터를 바라봄
        if (other.CompareTag("Player"))
        {
            //transform.LookAt(other.transform);
            Vector3 dir = new Vector3(other.transform.position.x, transform.position.y, other.transform.position.z);
            //transform.rotation = Quaternion.Lerp(transform.rotation,
            //  Quaternion.LookRotation(dir), Time.deltaTime*rotSpeed);
            transform.LookAt(dir);
            animator.SetBool(hashTalk, true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        animator.SetBool(hashTalk, false);
    }
}
