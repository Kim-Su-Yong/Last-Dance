using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// �� �� : GotHitTrace ���� �߰��ϱ�(Distance ������� �ٷ� ����), ���¿� ���� HP Ȱ��ȭ ���� �����ϱ�
// GotHit = true -> Time.time �� = 0~ ����, Time.time �� ���� ������ ���� �ð� �����ϰ�, Patrol�� �� GotHit = false; 1. Hp SetActive false �ð� �� ������ 2. Hp �ʱ�ȭ
// �� ������ GotHit 
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
    public int M_EXP = 0;      // �� ���͸�Ÿ ����ġ�� �ٸ��Ƿ� ����

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
    private float AwayTime = 20f;    // ���� 20
    private float GoneTime = 40f;   // ���� 40
    private float _countTime = 0f;
    [HideInInspector] public Color HpColor = new Color(0f, 188f, 195f, 255f);

    // Damage
    public float _beforeHP = 0f;    // MonsterDamage.cs���� �� �����

    // ReadOnly
    // ������ Ư�� �� BlendTree > Locomotion �̿ܿ��� Any State -> Trigger 
    //private readonly int hashMove = Animator.StringToHash("IsMove");
    private readonly int hashSpeed = Animator.StringToHash("Locomotion");
    private readonly string playerTag = "Player";

    private readonly float damping = 10.0f; // Monster LookAt Player ȸ�� ���

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
        // [Hp ��Ȱ��ȭ]
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
        // [������ �г�] �� isDamaged == ture�� �� TraceDistance += 5f �ǰ�,
        //                    GoneTime ���� isDamaged == false �Ǹ� �ʱⰪ���� ����
        if (isDamaged == true)
        {
            //Debug.Log("Did it work?");
            traceDist = MadTraceDist;
        }
    }

    public void SetUp(MonsterData monsterData)
    {
        M_MaxHP = monsterData.HP;
        M_EXP = monsterData.EXP;    // ����ġ ���� �߰�
        damage = monsterData.damage;
        attackSpeed = monsterData.attackSpeed;
        moveAgent.patrolSpeed = monsterData.patrolSpeed;
        moveAgent.traceSpeed = monsterData.traceSpeed;
        attackDist = monsterData.attackDist;
        // TraceDistance
        traceDist = monsterData.traceDist;
        traceDistMemory = traceDist;
        MadTraceDist = monsterData.MadTraceDist;
        //monsterRenderer.material.color = monsterData.skinColor; // ���������� ���ų�, �¾��� �� �� ����
    }

    // ���� ���� �� ���� Ÿ�� ������ �޾Ƽ� �����Ѵ�.
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
        // �ִϸ��̼� ���� ���
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
        // ���� ��� ������
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

                    // ���� ����� �÷��̾� ����ġ ����
                    PlayerStat.instance.GainExp(M_EXP);
                    PlayerStat.instance.ChangeMoney(M_EXP*10);
                    
                    // ����� ���̻� �ڷ�ƾ�� �������� ����
                    StopAllCoroutines();

                    StartCoroutine(PushPool());
                    break;
            }
        }
    }

    public void HpUpdate()
    {
        // ���� ���� �Լ��� ������ �ε巴�� �پ�鵵�� ����
        //Hp_Bar.fillAmount = Mathf.Lerp(Hp_Bar.fillAmount, M_HP / M_MaxHP, Time.deltaTime * 100f);
        Hp_Bar.fillAmount = M_HP / M_MaxHP;
        Hp_Bar_Before.fillAmount = _beforeHP / M_MaxHP;

        //Debug.Log("Hp_Bar.fillAmount �� : " + Hp_Bar.fillAmount * M_MaxHP);
        Hp_Text.text = ((int)M_HP).ToString() + " / " + ((int)M_MaxHP).ToString();

        // HP ���� ���� ����

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