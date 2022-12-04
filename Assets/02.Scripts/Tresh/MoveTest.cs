using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* 
 * ĳ���� ��Ʈ�ѷ� ������Ʈ
 * ĳ���� �������� �����ϱ� ���� ���Ǵ� ������Ʈ
 * ������Ʈ�� ����ϸ� �浹 üũ�� �ϴ� BoxColider�� �ʿ� ����
 */

public class MoveTest : MonoBehaviour
{
    CharacterController controller;  // ĳ���� ��Ʈ�ѷ� ������Ʈ

    [Header("ĳ������ �̵�")]
    public float moveSpeed;        // �̵� �ӵ� ��
    float gravity;                 // ĳ���Ϳ� ������ �߷� ���ӵ� ��
    Vector3 moveDir;               // ĳ������ �̵� ���� ����

    [Header("ĳ������ ����")]
    public float jumpPower;         // ���� �� ���
    bool isJumpPressed;             // ���� Ű�� �������� Ȯ��

    [Header("ī�޶�")]
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
        // �⺻ ��
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
        // ĳ���Ͱ� �ٴڿ� �پ��մ� ��쿡�� �۵�(�߶� �߿��� ���� ��ȯ �Ұ�)
        if (controller.isGrounded)
        {
            // ���� Ű���忡 ���� �̵� ���� ����
            moveDir = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            // ���� ��ǥ�� ���� ��ǥ�� ����
            moveDir = controller.transform.TransformDirection(moveDir);
            moveDir *= moveSpeed;
            animator.SetBool("IsRun", true);

            // ���� Ű�� ������ ������(���� ���� ���°� �ƴ϶��)
            if (Input.GetButton("Jump") && isJumpPressed == false)
            {
                isJumpPressed = true;
                moveDir.y = jumpPower;
            }
        }
        else
        {
            // �߶� ���� ��� �߷��� ������ �޾� ������
            moveDir.y -= gravity * Time.deltaTime;
        }

        if (!Input.GetButton("Jump"))
            isJumpPressed = false;
        // �̵�
        controller.Move(moveDir * Time.deltaTime);
        
    }

    void Jump()
    {
        
    }
}
