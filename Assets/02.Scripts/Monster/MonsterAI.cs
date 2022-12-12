using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// 할 것 : GotHitTrace 상태 추가하기(Distance 상관없이 바로 추적), 상태에 따라 HP 활성화 여부 구현하기
// GotHit = true -> Time.time 값 = 0~ 시작, Time.time 값 저장 변수가 일정 시간 도달하고, Patrol일 때 GotHit = false; 1. Hp SetActive false 시간 더 지나면 2. Hp 초기화
// 또 맞으면 GotHit 
//      

public class MonsterAI : MonoBehaviour
{
    public enum MonsterType
    {
        A_Skeleton,
        B_Fishman,
        C_Slime
    }
    public MonsterType monsterType;

    // Monster
    public float M_HP = 0f;
    public float M_MaxHP = 100f;

    // Monster AI Move
    public enum State
    {
        PATROL,
        TRACE,
        ATTACK,
        DIE
    }
    public State state = State.PATROL;
    public Transform playerTr;
    public Transform monsterTr;

    public float attackDist = 5.0f;
    public float traceDist = 10.0f;

    // Attack Value
    public float damage = 20f;
    public float attackSpeed = 0.5f;  // Between
    private float nextAttackTime = 0;

    // 해야 할 것 : 공격하지 않을 땐 콜라이더 enabled = false;로 변경
    // Components
    [HideInInspector]
    public SphereCollider attackCollider;

    // Bool
    public bool isDie = false;
    public bool isAttack = false;
    public bool isDamaged = false;

    // Animation
    [HideInInspector]
    public Animator animator;

    // Audio
    [HideInInspector]
    public AudioSource audio;

    // Audio Clip
    private AudioClip attackSound;

    // Scripts
    MoveAgent moveAgent;
    MonsterAttack monsterAttack;

    // Etc.
    private WaitForSeconds ws;
    //public SkinnedMeshRenderer[] monsterRenderer;

    // UI
    public Canvas Hp_Canvas;
    public Image Hp_Bar;
    public Text Hp_Text;
    private float AwayTime = 5f;    // 최종 20
    private float GoneTime = 10f;   // 최종 40
    private float countTime = 0f;
    public Color HpColor = new Color(0f, 188f, 195f, 255f);
    

    // ReadOnly
    // 프리팹 특성 상 BlendTree > Locomotion 이외에는 Any State -> Trigger 
    //private readonly int hashMove = Animator.StringToHash("IsMove");
    private readonly int hashSpeed = Animator.StringToHash("Locomotion");
    private readonly string playerTag = "Player";

    private readonly float damping = 10.0f; // Monster LookAt Player 회전 계수

    void Awake()
    {
        var player = GameObject.FindGameObjectWithTag(playerTag);
        if (player != null)
        {
            playerTr = player.GetComponent<Transform>();
        }
        monsterTr = GetComponent<Transform>();
        animator = GetComponent<Animator>();
        audio = GetComponent<AudioSource>();
        moveAgent = GetComponent<MoveAgent>();
        monsterAttack = GetComponent<MonsterAttack>();
        switch (monsterType)
        {
            case MonsterAI.MonsterType.A_Skeleton:
                //monsterRenderer = GetComponentsInChildren<SkinnedMeshRenderer>();
                break;
            case MonsterAI.MonsterType.B_Fishman:
                //monsterRenderer = GetComponentsInChildren<SkinnedMeshRenderer>();
                break;
            case MonsterAI.MonsterType.C_Slime:
                break;
        }

        // UI
        Hp_Canvas = transform.GetChild(2).GetComponent<Canvas>();
        Hp_Bar = transform.GetChild(2).GetChild(0).GetChild(0).GetComponent<Image>();
        Hp_Text = transform.GetChild(2).GetChild(0).GetChild(1).GetComponent<Text>();

        // Resources Load
        attackSound = Resources.Load<AudioClip>("Sound/");

        ws = new WaitForSeconds(0.3f);
    }

    private void Start()
    {
        M_HP = M_MaxHP;
        Hp_Canvas.enabled = false;
    }

    void Update()
    {
        StartCoroutine(CheckState());
        StartCoroutine(Action());

        // Attack
        if (isAttack)
        {
            if (Time.time >= nextAttackTime)
            {
                Attack();
                nextAttackTime = Time.time + attackSpeed + Random.Range(0.1f, 0.3f);
            }
            Quaternion rot = Quaternion.LookRotation(playerTr.position - monsterTr.position);
            monsterTr.rotation = Quaternion.Slerp(monsterTr.rotation, rot, Time.deltaTime * damping);
        }

        // Hp UI 비활성화 루틴
        if (isDamaged == true)
        {
            Hp_Canvas.enabled = true;
            if (moveAgent.patrolling)
            {
                float CountTime = 0;
                CountTime += Time.deltaTime;
                if (CountTime >= AwayTime)
                {
                    Hp_Canvas.enabled = false;
                }
                if (CountTime >= GoneTime)
                {
                    M_HP = M_MaxHP;
                    isDamaged = false;
                }
            }
        }
        HpUpdate();
    }

    public void SetUp(MonsterData monsterData)
    {
        M_MaxHP = monsterData.HP;
        damage = monsterData.damage;
        attackSpeed = monsterData.attackSpeed;
        moveAgent.patrolSpeed = monsterData.patrolSpeed;
        moveAgent.traceSpeed = monsterData.traceSpeed;
        attackDist = monsterData.attackDist;
        traceDist = monsterData.traceDist;
        //monsterRenderer.material.color = monsterData.skinColor; // 슬라임한테 쓰거나, 맞았을 때 쓸 예정
    }

    // 몬스터 생성 시 몬스터 타입 정보를 받아서 저장한다.
    public void LetMeKnowMonsterType(int type) // From MonsterSpawner.cs
    {
        if (type == 0)
        {
            monsterType = MonsterType.A_Skeleton;
        }
        else if (type == 1)
        {
            monsterType = MonsterType.B_Fishman;
        }
        else if (type == 2)
        {
            monsterType = MonsterType.C_Slime;
        }
    }

    private void Attack()
    {
        // 애니메이션 랜덤 재생
        switch (monsterType)
        {
            case MonsterAI.MonsterType.A_Skeleton:
                animator.SetTrigger($"Attack {Random.Range(1, 4)}");
                break;
            case MonsterAI.MonsterType.B_Fishman:
                animator.SetTrigger($"Attack {Random.Range(1, 3)}");
                break;
            case MonsterAI.MonsterType.C_Slime:
                break;
        }
        //audio.PlayOneShot(attackSound, 1.0f);
    }

    void OnEnable()
    {
        //StartCoroutine(CheckState());
        //StartCoroutine(Action());
    }

    IEnumerator CheckState()
    {
        // 몬스터 사망 전까지
        while (!isDie)
        {
            if (state == State.DIE) yield break;

            float dist = Vector3.Distance(playerTr.position, monsterTr.position);

            if (dist <= attackDist)
            {
                state = State.ATTACK;
            }
            else if (dist <= traceDist)
            {
                state = State.TRACE;
            }
            else
            {
                state = State.PATROL;
            }
            yield return ws;
        }
        yield return ws;
    }

    IEnumerator Action()
    {
        while (!isDie)
        {
            yield return ws;
            switch (state)
            {
                case State.PATROL:
                    isAttack = false;
                    moveAgent.patrolling = true;
                    animator.SetFloat(hashSpeed, moveAgent.speed);
                    break;
                case State.TRACE:
                    isAttack = false;
                    moveAgent.traceTarget = playerTr.position;
                    animator.SetFloat(hashSpeed, moveAgent.speed);
                    break;
                case State.ATTACK:
                    if (isAttack == false)
                    {
                        isAttack = true;
                    }
                        moveAgent.Stop();
                    break;
                case State.DIE:
                    isAttack = false;
                    moveAgent.Stop();
                    animator.SetTrigger("Death");
                    isAttack = false;
                    isDie = true;

                    GetComponent<Rigidbody>().isKinematic = true;
                    GetComponent<CapsuleCollider>().enabled = false;
                    StopAllCoroutines();

                    StartCoroutine(PushPool());
                    
                    //Invoke("Die", 5.0f);
                    break;
            }
        }
    }

    public void HpUpdate()
    {
        Hp_Bar.fillAmount = M_HP / M_MaxHP;
        Hp_Text.text = ((int)M_HP).ToString() + " / 100";
        if (M_HP <= 0)
        {
            state = State.DIE;
            Hp_Canvas.enabled = false;
        }
        
    }

    public void DamagedUI()
    {
        Hp_Bar.color = Color.red;
        StartCoroutine(ReturnUIColor());
    }

    IEnumerator ReturnUIColor()
    {
        yield return new WaitForSeconds(0.3f);
        Hp_Bar.color = HpColor;
    }

    IEnumerator PushPool()
    {
        yield return new WaitForSeconds(3.0f);
        isDie = false;
        GetComponent<Rigidbody>().isKinematic = false;
        GetComponent<CapsuleCollider>().enabled = true;
        this.gameObject.SetActive(false);
        M_HP = M_MaxHP;
    }


}
