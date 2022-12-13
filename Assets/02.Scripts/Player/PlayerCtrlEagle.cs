using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCtrlEagle : MonoBehaviour
{
    [SerializeField]
    private Animation _animation;
    private Rigidbody rd;
    float h = 0f, v = 0f, r = 0f; //Horizontal Vertical Mouse X 명령을 받는 변수
    public float moveSpeed = 3.5f; //이동 속도
    public float turnSpeed = 80f;  //회전 속도
    public float JumpForce = 10.0f;
    private bool isJump; //점프중인지 체크
    private bool CanFly; //날 수 있는지 체크
    private bool isFly; //날고 있는중인지 체크
    [HideInInspector]
    public bool isGround;
    private bool CanFlyAttack; //낙하 공격 가능한지 체크
    private bool isFlyAttack; //공중 공격중인지 체크
    private bool CanJump = true; //공중에서 점프 할 수 있는지 체크
    private const string groundTag = "Ground";
    
    private bool PowerJump = true;
    public float PowerJump_CoolTime = 10.0f; //강점프 쿨타임
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
        flyattackEffect = Resources.Load<GameObject>("BigExplosionEffect"); //낙하공격시 나오는 이펙트(임시)
        flyattackSound = Resources.Load<AudioClip>("Metal impact 5"); //낙하공격시 나오는 사운드(임시)
    }
    void Update()
    {
        Jump();
        Move();
        Fly();
        //Test();
    }
    private void Move() //이동 관련 함수
    {
        h = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical"); 
        r = Input.GetAxis("Mouse X");

        Vector3 moveDir = (h * Vector3.right) + (v * Vector3.forward);
        transform.Translate(moveDir.normalized * moveSpeed * Time.deltaTime);
        transform.Rotate(Vector3.up * r * turnSpeed * Time.deltaTime);
    }
    void Jump() //점프 관련 함수
    {
        if (Input.GetKey(KeyCode.Space) && !isJump) //일반 점프
        {
            Jump_Timer += Time.deltaTime;
        }
        if (Input.GetKeyUp(KeyCode.Space)) //강점프
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
        if (!PowerJump) //강점프 쿨타임 주기 위한 조건문
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
    void Fly() //비행 관련 함수
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
    void FlyAttackActive() //낙하공격 관련 함수
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