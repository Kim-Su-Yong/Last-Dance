using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    Animator animator;
    Rigidbody rb;

    [Header("�̵� ���� ����")]
    public float moveSpeed = 6f;         // �÷��̾� �̵��ӵ�
    Vector3 moveVec;
    [SerializeField]
    float InitSpeed = 6f;           // �÷��̾� �ʱ� �̵��ӵ�
    public float turnSpeed = 80f;    // �÷��̾� ȸ�� �ӵ�
    public bool isWalk;             // �÷��̾� �޸��� ����
    float h, v, r;

    [Header("���� ���� ����")]
    public float jumpPower = 10f;   // �÷��̾� ������ �������� ��
    [SerializeField]
    bool isJump = false;            // ���� ����
    [SerializeField]
    bool isGrounded = false;        // ���� ��� �ִ� ����


    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody>();
        //moveSpeed = InitSpeed;
    }

    void Update()
    {
        Move();
        Turn();
        // ���� ���� ���°� �ƴϸ� ���� ������� ��쿡�� ���� ��� ����
        if (Input.GetKeyDown(KeyCode.Space) && isJump == false && isGrounded == true)   
            Jump();
    }

    void Move()
    {
        h = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");
        r = Input.GetAxis("Mouse X");

        moveVec = new Vector3(h, 0, v).normalized;

        if (h != 0 || v != 0)
        {
            transform.position += moveVec * moveSpeed * Time.deltaTime;      // �̵�
            animator.SetBool("IsRun", true);

            Walk();
        }
        else
        {
            animator.SetBool("IsRun", false);
        }
        
        transform.Rotate(Vector3.up * r * Time.deltaTime * turnSpeed);
        transform.LookAt(transform.position);
    }

    void Walk()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            isWalk = true;
            moveSpeed = 4f;
            animator.SetBool("IsWalk", true);
        }
        else if(Input.GetKeyUp(KeyCode.LeftShift))
        {
            isWalk = false;
            moveSpeed = InitSpeed;
            animator.SetBool("IsWalk", false);
        }
    }

    private void Turn()
    {
        transform.LookAt(transform.position + moveVec);
    }

    void Jump()
    {
        Debug.Log("����");
        rb.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
    }

    private void OnCollisionEnter(Collision col)
    {
        if(col.collider.CompareTag("Ground"))
        {
            isGrounded = true;
            isJump = false;
        }
    }

    private void OnCollisionExit(Collision col)
    {
        if (col.collider.CompareTag("Ground"))
        {
            isGrounded = false;
            isJump = true;
        }
    }
}
