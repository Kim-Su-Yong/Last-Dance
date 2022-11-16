using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove2 : MonoBehaviour
{
    Transform tr;
    Rigidbody rb;
    Animator animator;
    AudioSource source;

    [Header("플레이어 이동 변수")]
    public float f_moveSpeed = 6f;     // 캐릭터의 이동 속도
    public float f_jumpPower = 10f;     // 캐릭터가 점프시 가해지는 힘
    public float f_turnSpeed = 100f;    // 캐릭터의 회전 속도

    [Header("이동 벡터값 변수")]
    [SerializeField]
    float f_hAxis;
    [SerializeField]
    float f_vAxis;
    [SerializeField]
    float f_rAxis;      // 회전 계수
    [SerializeField]
    Vector3 v_moveVec;  // 최종 이동 벡터

    [Header("Bool형 변수")]
    bool b_isJump;      // 점프 상태인지 확인할 변수
    bool b_isGround;    // 땅에 착지 상태인지 확인할 변수

    void Start()
    {
        tr = GetComponent<Transform>();
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        source = GetComponent<AudioSource>();
    }

    private void FixedUpdate()
    {
        Move();
    }

    void Update()
    {
        Move();
        Turn();
        Jump();
    }

    void Move()
    {
        f_hAxis = Input.GetAxis("Horizontal");
        f_vAxis = Input.GetAxis("Vertical");
        f_rAxis = Input.GetAxis("Mouse X");

        v_moveVec = new Vector3(f_hAxis, 0, f_vAxis).normalized;

        transform.Translate(v_moveVec* f_moveSpeed * Time.deltaTime);

        animator.SetBool("IsRun", v_moveVec != Vector3.zero);

        transform.Rotate(Vector3.up * f_rAxis * Time.deltaTime * f_turnSpeed);
        transform.LookAt(tr.position);
    }

    void Turn()
    {
        //transform.LookAt();
        //transform.LookAt(transform.position + v_moveVec);
    }

    void Jump()
    {
        if (!b_isGround) return;    // 연속 점프 불가능
        if (Input.GetKeyDown(KeyCode.Space))
        {
            rb.AddForce(Vector3.up * f_jumpPower, ForceMode.Impulse);
            b_isJump = true;
        }
    }

    private void OnCollisionEnter(Collision col)
    {
        if (col.collider.CompareTag("Ground"))
            b_isGround = true;
    }

    private void OnCollisionExit(Collision col)
    {
        if (col.collider.CompareTag("Ground"))
            b_isGround = false;
    }
}
