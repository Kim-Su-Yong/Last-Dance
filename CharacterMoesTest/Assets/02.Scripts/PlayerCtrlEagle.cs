using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCtrlEagle : MonoBehaviour
{
    [SerializeField]
    private Animation _animation;
    private Rigidbody rd;
    float h = 0f, v = 0f, r = 0f; //Horizontal Vertical Mouse X ����� �޴� ����
    public float moveSpeed = 3.5f; //�̵� �ӵ�
    public float turnSpeed = 80f;  //ȸ�� �ӵ�
    public float JumpForce = 10.0f;
    private bool isJump; //���������� üũ
    private bool CanFly; //�� �� �ִ��� üũ
    private bool isFly; //���� �ִ������� üũ
    [HideInInspector]
    public bool isGround;
    private bool CanFlyAttack; //���� ���� �������� üũ
    private bool isFlyAttack; //���� ���������� üũ
    private bool CanJump = true; //���߿��� ���� �� �� �ִ��� üũ
    private const string groundTag = "Ground";
    
    private bool PowerJump = true;
    public float PowerJump_CoolTime = 10.0f; //������ ��Ÿ��
    private float PowerJump_Timer;

    private GameObject flyattackEffect;
    private GameObject flyattackrange;
    private AudioSource audio;
    private AudioClip flyattackSound;

    private float Jump_Timer;
    private void Start()
    {
        rd = GetComponent<Rigidbody>();
        audio = GetComponent<AudioSource>();
        flyattackrange = GameObject.Find("Eagle").transform.GetChild(3).gameObject;
        flyattackEffect = Resources.Load<GameObject>("BigExplosionEffect"); //���ϰ��ݽ� ������ ����Ʈ(�ӽ�)
        flyattackSound = Resources.Load<AudioClip>("Metal impact 5"); //���ϰ��ݽ� ������ ����(�ӽ�)
    }
    void Update()
    {
        Jump();
        Move();
        Fly();
        //Test();
    }
    private void Move() //�̵� ���� �Լ�
    {
        h = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical"); 
        r = Input.GetAxis("Mouse X");

        Vector3 moveDir = (h * Vector3.right) + (v * Vector3.forward);
        transform.Translate(moveDir.normalized * moveSpeed * Time.deltaTime);
        transform.Rotate(Vector3.up * r * turnSpeed * Time.deltaTime);
    }
    void Jump() //���� ���� �Լ�
    {
        if (Input.GetKey(KeyCode.Space) && !isJump) //�Ϲ� ����
        {
            Jump_Timer += Time.deltaTime;
        }
        if (Input.GetKeyUp(KeyCode.Space)) //������
        {
            if (Jump_Timer < 2.0f && !isJump && CanJump)
            {
                Jumping();
            }
            else if(Jump_Timer > 2.0f && !isJump && CanJump)
            {
                if(PowerJump)
                {
                    JumpForce = 20.0f;
                    Jumping();
                    CanFly = true;
                    CanFlyAttack = true;
                }
                else if(!PowerJump)
                {
                    Jumping();
                }
                PowerJump = false;
            }
        }
        if (!PowerJump) //������ ��Ÿ�� �ֱ� ���� ���ǹ�
        {
            PowerJump_Timer += Time.deltaTime;
            if (PowerJump_Timer > PowerJump_CoolTime)
            {
                PowerJump_Timer = 0;
                PowerJump = true;
            }
        }
    }
    void Jumping()
    {
        isJump = true;
        isGround = false;
        CanJump = false;
        rd.AddForce(Vector3.up * JumpForce, ForceMode.Impulse);
        Jump_Timer = 0;
    }
    void Fly() //���� ���� �Լ�
    {
        if (isGround == false)
        {
            if (Input.GetKeyDown(KeyCode.E) && CanFly && !isFly)
            {
                rd.drag = 10.0f;
                isJump = false;
                isFly = true;
                CanJump = false;
                CanFlyAttack = true;
            }
            else if (Input.GetKeyDown(KeyCode.E) && CanFly && isFly)
            {
                rd.drag = 0;
                isJump = false;
                isFly = false;
            }
            else if (Input.GetMouseButtonDown(0) && CanFlyAttack && !CanJump)
            {
                rd.drag = 0;
                isFlyAttack = true;
                FlyAttackActive();
            }
        }
    }
    void FlyAttackActive() //���ϰ��� ���� �Լ�
    {
        flyattackrange.SetActive(true);
    }
    public void DeActive()
    {
        flyattackrange.SetActive(false);
    }
    private void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.CompareTag(groundTag))
        {
            isGround = true;
            isJump = false;
            CanFly = false;
            CanJump = true;
            JumpForce = 10.0f;
            rd.drag = 0;
            if(isFlyAttack)
            {
                GameObject effect = Instantiate(flyattackEffect, transform.position, Quaternion.identity);
                Destroy(effect, 1.0f);
                audio.PlayOneShot(flyattackSound, 1.0f);
            }
            isFlyAttack = false;
            CanFlyAttack = false;
            Invoke("DeActive", 0.5f);
        }
    } 
}