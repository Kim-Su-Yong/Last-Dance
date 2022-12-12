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

    // �ؾ� �� �� : �������� ���� �� �ݶ��̴� enabled = false;�� ����
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
    private float AwayTime = 5f;    // ���� 20
    private float GoneTime = 10f;   // ���� 40
    private float countTime = 0f;
    public Color HpColor = new Color(0f, 188f, 195f, 255f);
    

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

        // Hp UI ��Ȱ��ȭ ��ƾ
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
            monsterType = MonsterType.C_Slime;
        }
    }

    private void Attack()
    {
        // �ִϸ��̼� ���� ���
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
        // ���� ��� ������
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
