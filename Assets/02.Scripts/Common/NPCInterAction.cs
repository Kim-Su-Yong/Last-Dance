using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCInterAction : MonoBehaviour
{
    Animator animator;
    LayerMask playerLayer;
    readonly int hashQuest = Animator.StringToHash("Quest");
    readonly int hashTalk = Animator.StringToHash("Talk");
    void Start()
    {
        animator = GetComponent<Animator>();
        playerLayer = LayerMask.NameToLayer("Player");
    }

    void Update()
    {
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            transform.LookAt(other.transform);
        }
    }
}
