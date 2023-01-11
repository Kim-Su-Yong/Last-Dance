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
        C_Mushroom
    }
    public MonsterType monsterType;

    // Monster
    public float M_HP = 0f;
    public float M_MaxHP = 100f;
    public int M_EXP = 0;      // 각 몬스터마타 경험치가 다르므로 선언

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

    [SerializeField]
    private float attackDist = 2.0f;
    [SerializeField]
    private float traceDist = 8.0f;
    private float traceDistMemory = 0f;
    private float MadTraceDist = 15.0f;
    private float tooCloseDist = 1.2f;

    // Attack Value
    public float damage = 20f;
    public float attackSpeed = 2f;  // Between
    private float nextAttackTime = 0;

    public BoxCollider attackCollider;

    // Bool
    public bool isDie = false;
    public bool isAttack = false;
    public bool isDamaged = false;
    public bool isIdle = false;
    public bool isInDetection = false;

    // Animation
    [HideInInspector]
    public Animator animator;
    public float idleBreakTime = 10f;
    public float idleTimeCount = 0f;

    // Audio
    [HideInInspector]
    public AudioSource _audio;

    // Audio Clip
    private AudioClip attackSound;

    // Scripts
    MoveAgent moveAgent;
    MonsterAttack monsterAttack;

    // Etc.
    private WaitForSeconds ws;
    private Rigidbody rbody;

    // UI

    [HideInInspector] public Canvas Hp_Canvas;
    [HideInInspector] public Image Hp_Bar;
    [HideInInspector] public Image Hp_Bar_Before;
    [HideInInspector] public Text Hp_Text;
    private float AwayTime = 20f;    // 최종 20
    private float GoneTime = 40f;   // 최종 40
    private float _countTime = 0f;
    [HideInInspector] public Color HpColor = new Color(0f, 188f, 195f, 255f);

    // Damage
    public float _beforeHP = 0f;    // MonsterDamage.cs에서 값 저장됨

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
        _audio = GetComponent<AudioSource>();
        moveAgent = GetComponent<MoveAgent>();
        monsterAttack = GetComponent<MonsterAttack>();
        attackCollider = transform.GetChild(3).GetComponentInChildren<BoxCollider>();
        rbody = GetComponent<Rigidbody>();

        // UI
        Hp_Canvas = transform.GetChild(2).GetComponent<Canvas>();
        Hp_Bar_Before = transform.GetChild(2).GetChild(0).GetChild(0).GetComponent<Image>();
        Hp_Bar = transform.GetChild(2).GetChild(0).GetChild(1).GetComponent<Image>();
        Hp_Text = transform.GetChild(2).GetChild(0).GetChild(2).GetComponent<Text>();

        // Resources Load
        attackSound = Resources.Load<AudioClip>("Sound/");

        ws = new WaitForSeconds(0.3f);
    }

    private void Start()
    {
        M_HP = M_MaxHP;
        Hp_Bar.fillAmount = 1f;
        Hp_Bar_Before.enabled = false;
        Hp_Canvas.enabled = false;
        attackCollider.enabled = false;

        Hp_Text.text = ((int)M_HP).ToString() + " / " + ((int)M_MaxHP).ToString();

    }

    void Update()
    {
        StartCoroutine(CheckState());
        StartCoroutine(Action());

        // Hp Bar
        if (isDamaged == true)
            _countTime += Time.deltaTime;
        PlayerFarAway();
        //HpUpdate();

        // Attack
        if (isAttack)
        {
            if (Time.time >= nextAttackTime)
            {
                StartCoroutine(Attack());
                nextAttackTime = Time.time + attackSpeed + Random.Range(0.1f, 0.3f);
            }
            Quaternion rot = Quaternion.LookRotation(playerTr.position - monsterTr.position);
            monsterTr.rotation = Quaternion.Slerp(monsterTr.rotation, rot, Time.deltaTime * damping);
        }
    }

    private void PlayerFarAway()
    {
        // [Hp 비활성화]
        if (isInDetection == true) return;
        if (moveAgent.patrolling == true && isDamaged == true)
        {
            if (_countTime >= AwayTime)
            {
                Hp_Canvas.enabled = false;
            }
            if (_countTime >= GoneTime)
            {
                M_HP = M_MaxHP;
                isDamaged = false;
                _countTime = 0f;
                traceDist = traceDistMemory;
            }
        }
        // [몬스터의 분노] → isDamaged == ture일 때 TraceDistance += 5f 되고,
        //                    GoneTime 이후 isDamaged == false 되면 초기값으로 변경
        if (isDamaged == true)
        {
            //Debug.Log("Did it work?");
            traceDist = MadTraceDist;
        }
    }

    public void SetUp(MonsterData monsterData)
    {
        M_MaxHP = monsterData.HP;
        M_EXP = monsterData.EXP;    // 경험치 변수 추가
        damage = monsterData.damage;
        attackSpeed = monsterData.attackSpeed;
        moveAgent.patrolSpeed = monsterData.patrolSpeed;
        moveAgent.traceSpeed = monsterData.traceSpeed;
        attackDist = monsterData.attackDist;
        // TraceDistance
        traceDist = monsterData.traceDist;
        traceDistMemory = traceDist;
        MadTraceDist = monsterData.MadTraceDist;
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
            monsterType = MonsterType.C_Mushroom;
        }
    }

    IEnumerator Attack()
    {
        // 애니메이션 랜덤 재생
        switch (monsterType)
        {
            case MonsterType.A_Skeleton:
                animator.SetTrigger($"Attack {Random.Range(1, 4)}");

                yield return new WaitForSeconds(0.7f);
                attackCollider.enabled = true;
                yield return new WaitForSeconds(0.2f);
                attackCollider.enabled = false;

                break;
            case MonsterType.B_Fishman:
                animator.SetTrigger($"Attack {Random.Range(1, 3)}");

                yield return new WaitForSeconds(0.9f);
                attackCollider.enabled = true;
                yield return new WaitForSeconds(0.2f);
                attackCollider.enabled = false;
                break;
            case MonsterType.C_Mushroom:
                animator.SetTrigger($"Attack {Random.Range(1, 2)}");
                yield return new WaitForSeconds(0.4f);
                attackCollider.enabled = true;
                yield return new WaitForSeconds(0.2f);
                attackCollider.enabled = false;

                break;
        }
        //audio.PlayOneShot(attackSound, 1.0f);
    }

    IEnumerator CheckState()
    {
        // 몬스터 사망 전까지
        while (!isDie)
        {
            if (state == State.DIE) yield break;

            float dist = Vector3.Distance(playerTr.position, monsterTr.position);
            if (dist <= tooCloseDist)
            {
                rbody.AddForce(new Vector3(0f, 0f, -1f), ForceMode.Impulse);
            }
            else if (dist <= attackDist)
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
                    isDie = true;

                    //UIManager.instance.ChangeMoney(M_EXP);
                    GetComponent<Rigidbody>().isKinematic = true;
                    GetComponent<CapsuleCollider>().enabled = false;
                    //StopAllCoroutines();

                    // 몬스터 사망시 플레이어 경험치 증가
                    PlayerStat.instance.GainExp(M_EXP);
                    PlayerStat.instance.ChangeMoney(M_EXP*10);
                    
                    // 사망시 더이상 코루틴을 수행하지 않음
                    StopAllCoroutines();

                    StartCoroutine(PushPool());
                    break;
            }
        }
    }

    public void HpUpdate()
    {
        // 선형 보간 함수로 데미지 부드럽게 줄어들도록 만듦
        //Hp_Bar.fillAmount = Mathf.Lerp(Hp_Bar.fillAmount, M_HP / M_MaxHP, Time.deltaTime * 100f);
        Hp_Bar.fillAmount = M_HP / M_MaxHP;
        Hp_Bar_Before.fillAmount = _beforeHP / M_MaxHP;

        //Debug.Log("Hp_Bar.fillAmount 값 : " + Hp_Bar.fillAmount * M_MaxHP);
        Hp_Text.text = ((int)M_HP).ToString() + " / " + ((int)M_MaxHP).ToString();

        // HP 값의 범위 지정

        if (M_HP <= 0)
        {
            state = State.DIE;
            Hp_Canvas.enabled = false;
        }
    }

    public void DamagedUI()
    {
        Hp_Bar.color = Color.red;
        Hp_Bar_Before.enabled = true;
        Invoke("ReturnUIColor", 0.3f);
    }

    private void ReturnUIColor()
    {
        Hp_Bar.color = HpColor;
        Hp_Bar_Before.enabled = false;
    }

    IEnumerator PushPool()
    {
        yield return new WaitForSeconds(3.0f);
        ReturnUIColor();
        this.gameObject.SetActive(false);
        GetComponent<Rigidbody>().isKinematic = false;
        GetComponent<CapsuleCollider>().enabled = true;
        M_HP = M_MaxHP;
    }
    IEnumerator wait()
    {
        yield return new WaitForSeconds(3.5f);
    }
    private void OnEnable()
    {
        Hp_Bar.fillAmount = 1f;
        Hp_Text.text = ((int)M_HP).ToString() + " / " + ((int)M_MaxHP).ToString();
    }
}