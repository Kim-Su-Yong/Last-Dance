using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTest2 : MonoBehaviour
{
    // 수평, 수직, 회전
    float h, v, r;
    public float moveSpeed = 3.5f;      // 캐릭터 이동 속도
    public float runSpeed = 7f;         // 캐릭터 달리기 속도
    public float turnSpeed = 80f;       // 캐릭터 회전 속도
    Vector3 moveDir = Vector3.zero;

    // 마우스 감도
    public float xSensitivity = 100f;
    public float ySensitivity = 100f;

    // 회전 제한
    public float yMinLimit = -45f;
    public float yMaxLimit = 45f;
    public float xMinLimit = -360f;
    public float xMaxLimit = 360f;

    public float yRot = 0f;             // y축 회전 변수
    public float xRot = 0f;             // x축 회전 변수

    public float jumpForce = 10f;       // 점프 파워
    bool isJump;                        // 점프 상태 확인

    bool isRun;                         // 달리기 상태 확인

    public bool isGronded;
    float gravity = 9.8f;

    Animator animator;
    CharacterController controller;

    void Start()
    {
        animator = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        isGronded = controller.isGrounded;

        
        Move();
        Turn();
        //RotateLimit();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }
    }

    void Move()
    {
        if(controller.isGrounded)
        {
            h = Input.GetAxis("Horizontal");    // A, D를 눌렀을 때
            v = Input.GetAxis("Vertical");      // W, S를 눌렀을 때

            moveDir = Vector3.right * h + Vector3.forward * v;

            if (controller.velocity.magnitude > 0.1f)
            {
                isRun = true;
                animator.SetBool("IsRun", true);
            }

            else
            {
                isRun = false;
                animator.SetBool("IsRun", false);
            }
        }
        else
        {
            moveDir.y -= gravity * Time.deltaTime;
        }
        controller.Move(moveDir.normalized * Time.deltaTime * moveSpeed);
    }
    void Turn()
    {
        if (!isRun) return;
        transform.rotation = Quaternion.Slerp(transform.rotation,
                Quaternion.LookRotation(moveDir), turnSpeed);
    }

    void RotateLimit() // 캐릭터 회전은 좀 다름
    {
        // 마우스 좌우(y축)로 회전할 때
        xRot += Input.GetAxis("Mouse X") * xSensitivity * Time.deltaTime;
        // 마우스 위아래(x축)로 회전할 때
        yRot += Input.GetAxis("Mouse Y") * ySensitivity * Time.deltaTime;

        // 회전 값 제한
        yRot = Mathf.Clamp(yRot, yMinLimit, yMaxLimit);
        xRot = Mathf.Clamp(xRot, xMinLimit, xMaxLimit);

        // 로컬 기준으로 회전
        // 플레이어 위치에 따라 축이나 회전축을 찾기 위해 로컬 회전축 사용
        transform.localEulerAngles = new Vector3(yRot, xRot, 0f);
    }

    void Jump()
    {
        if (isJump) return;
        if (controller.isGrounded)
        {
            //controller.velocity.y = Vector3.up * jumpForce;
            moveDir.y = jumpForce;
            isJump = true;
        }
        
    }
}
