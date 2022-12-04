using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove2 : MonoBehaviour
{
    Transform tr;
    Rigidbody rb;
    Animator animator;
    AudioSource source;

    [Header("�÷��̾� �̵� ����")]
    public float f_moveSpeed = 6f;     // ĳ������ �̵� �ӵ�
    public float f_jumpPower = 10f;     // ĳ���Ͱ� ������ �������� ��
    public float f_turnSpeed = 100f;    // ĳ������ ȸ�� �ӵ�

    [Header("�̵� ���Ͱ� ����")]
    [SerializeField]
    float f_hAxis;
    [SerializeField]
    float f_vAxis;
    [SerializeField]
    float f_rAxis;      // ȸ�� ���
    [SerializeField]
    Vector3 v_moveVec;  // ���� �̵� ����

    [Header("Bool�� ����")]
    bool b_isJump;      // ���� �������� Ȯ���� ����
    bool b_isGround;    // ���� ���� �������� Ȯ���� ����

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
        if (!b_isGround) return;    // ���� ���� �Ұ���
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
