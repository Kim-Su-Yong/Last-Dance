using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCtrl : MonoBehaviour
{
    Movement3D movement3D;
    Animator animator;

    void Start()
    {
        movement3D = GetComponent<Movement3D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        float x = Input.GetAxis("Horizontal");  // 방향키 좌우
        float z = Input.GetAxis("Vertical");    // 방향키 상하

        movement3D.MoveTo(new Vector3(x, 0f, z));
        if(x == 0 && z == 0)
            animator.SetBool("IsRun", false);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            movement3D.JumpTo();
        }    
    }
}
