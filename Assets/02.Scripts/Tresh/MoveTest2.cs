using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTest2 : MonoBehaviour
{
    // ����, ����, ȸ��
    float h, v, r;
    public float moveSpeed = 3.5f;      // ĳ���� �̵� �ӵ�
    public float runSpeed = 7f;         // ĳ���� �޸��� �ӵ�
    public float turnSpeed = 80f;       // ĳ���� ȸ�� �ӵ�
    Vector3 moveDir = Vector3.zero;

    // ���콺 ����
    public float xSensitivity = 100f;
    public float ySensitivity = 100f;

    // ȸ�� ����
    public float yMinLimit = -45f;
    public float yMaxLimit = 45f;
    public float xMinLimit = -360f;
    public float xMaxLimit = 360f;

    public float yRot = 0f;             // y�� ȸ�� ����
    public float xRot = 0f;             // x�� ȸ�� ����

    public float jumpForce = 10f;       // ���� �Ŀ�
    bool isJump;                        // ���� ���� Ȯ��

    bool isRun;                         // �޸��� ���� Ȯ��

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
            h = Input.GetAxis("Horizontal");    // A, D�� ������ ��
            v = Input.GetAxis("Vertical");      // W, S�� ������ ��

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

    void RotateLimit() // ĳ���� ȸ���� �� �ٸ�
    {
        // ���콺 �¿�(y��)�� ȸ���� ��
        xRot += Input.GetAxis("Mouse X") * xSensitivity * Time.deltaTime;
        // ���콺 ���Ʒ�(x��)�� ȸ���� ��
        yRot += Input.GetAxis("Mouse Y") * ySensitivity * Time.deltaTime;

        // ȸ�� �� ����
        yRot = Mathf.Clamp(yRot, yMinLimit, yMaxLimit);
        xRot = Mathf.Clamp(xRot, xMinLimit, xMaxLimit);

        // ���� �������� ȸ��
        // �÷��̾� ��ġ�� ���� ���̳� ȸ������ ã�� ���� ���� ȸ���� ���
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
