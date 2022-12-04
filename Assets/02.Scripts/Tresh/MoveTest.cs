using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* 
 * 캐릭터 컨트롤러 컴포넌트
 * 캐릭터 움직임을 제어하기 위해 사용되는 컴포넌트
 * 컴포넌트를 사용하면 충돌 체크를 하는 BoxColider는 필요 없다
 */

public class MoveTest : MonoBehaviour
{
    CharacterController controller;  // 캐릭터 컨트롤러 컴포넌트

    [Header("캐릭터의 이동")]
    public float moveSpeed;        // 이동 속도 값
    float gravity;                 // 캐릭터에 적용할 중력 가속도 값
    Vector3 moveDir;               // 캐릭터의 이동 방향 벡터

    [Header("캐릭터의 점프")]
    public float jumpPower;         // 점프 힘 계수
    bool isJumpPressed;             // 점프 키를 눌렀는지 확인

    [Header("카메라")]
    GameObject Cam;

    Animator animator;

    void Awake()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        Cam = Camera.main.gameObject;
    }
    void Start()
    {
        // 기본 값
        moveSpeed = 5f;
        gravity = 9.8f;
        moveDir = Vector3.zero;
        jumpPower = 5f;
        isJumpPressed = false;
    }

    void Update()
    {
        if (controller == null) return;

        Move();
    }

    private void Move()
    {
        if(Input.GetAxis("Horizontal") != 0 ||
            Input.GetAxis("Vertical") != 0)
        {
            var offset = Cam.transform.forward;
            offset.y = 0;
            transform.LookAt(controller.transform.position + offset);
        }
        // 캐릭터가 바닥에 붙어잇는 경우에만 작동(추락 중에는 방향 전환 불가)
        if (controller.isGrounded)
        {
            // 방향 키보드에 따른 이동 방향 결정
            moveDir = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            // 로컬 좌표를 월드 좌표로 변경
            moveDir = controller.transform.TransformDirection(moveDir);
            moveDir *= moveSpeed;
            animator.SetBool("IsRun", true);

            // 점프 키를 누르면 점프함(점프 중인 상태가 아니라면)
            if (Input.GetButton("Jump") && isJumpPressed == false)
            {
                isJumpPressed = true;
                moveDir.y = jumpPower;
            }
        }
        else
        {
            // 추락 중인 경우 중력의 영향을 받아 떨어짐
            moveDir.y -= gravity * Time.deltaTime;
        }

        if (!Input.GetButton("Jump"))
            isJumpPressed = false;
        // 이동
        controller.Move(moveDir * Time.deltaTime);
        
    }

    void Jump()
    {
        
    }
}
