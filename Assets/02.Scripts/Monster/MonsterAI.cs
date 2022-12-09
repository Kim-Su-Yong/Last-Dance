using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �� �� : GotHitTrace ���� �߰��ϱ�(Distance ������� �ٷ� ����), ���¿� ���� HP Ȱ��ȭ ���� �����ϱ�
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

    // Bool
    public bool isDie = false;

    // Animation
    public Animator animator;

    // Audio
    public AudioSource audio;

    // Scripts
    MoveAgent moveAgent;
    MonsterAttack monsterAttack;

    // Etc.
    private WaitForSeconds ws;
    private Renderer monsterRenderer;

    // ReadOnly
    // ������ Ư�� �� BlendTree > Locomotion �̿ܿ��� Any State -> Trigger 
    //private readonly int hashMove = Animator.StringToHash("IsMove");
    private readonly int hashSpeed = Animator.StringToHash("Locomotion");
    private readonly string playerTag = "Player";

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

        monsterRenderer = GetComponentInChildren<Renderer>();

        ws = new WaitForSeconds(0.3f);
    }

    public void SetUp(MonsterData monsterData)
    {
        M_MaxHP = monsterData.HP;
        monsterAttack.damage = monsterData.damage;
        monsterAttack.attackSpeed = monsterData.attackSpeed;
        moveAgent.patrolSpeed = monsterData.patrolSpeed;
        moveAgent.traceSpeed = monsterData.traceSpeed;
        attackDist = monsterData.attackDist;
        traceDist = monsterData.traceDist;
        monsterRenderer.material.color = monsterData.skinColor; // ���������� ���ų�, �¾��� �� �� ����
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

    void OnEnable()
    {
        StartCoroutine(CheckState());
        StartCoroutine(Action());
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
                    monsterAttack.isAttack = false;
                    moveAgent.patrolling = true;
                    animator.SetFloat(hashSpeed, moveAgent.speed);
                    break;
                case State.TRACE:
                    monsterAttack.isAttack = false;
                    moveAgent.traceTarget = playerTr.position;
                    animator.SetFloat(hashSpeed, moveAgent.speed);
                    break;
                case State.ATTACK:
                    if (monsterAttack.isAttack == false)
                    {
                        monsterAttack.isAttack = true;
                    }
                        moveAgent.Stop();
                    break;
                case State.DIE:
                    Die();
                    moveAgent.Stop();
                    break;
            }
        }
    }

    void Update()
    {
    }

    public void Die()
    {
        monsterAttack.isAttack = false;
        isDie = true;
        moveAgent.Stop();

        GetComponent<Rigidbody>().isKinematic = true;
        GetComponent<CapsuleCollider>().enabled = false;
        StopAllCoroutines();

        this.gameObject.SetActive(false);
        StartCoroutine(PushPool());

        //Invoke("DieAnim", 2.0f); //�Լ��� f�� ���Ŀ� ȣ���Ѵ�.
    }

    IEnumerator PushPool()
    {
        yield return new WaitForSeconds(3.0f);
        this.gameObject.SetActive(false);
        moveAgent.patrolling = true;
        isDie = false;
        GetComponent<Rigidbody>().isKinematic = false;
        GetComponent<CapsuleCollider>().enabled = true;
        M_HP = M_MaxHP;
    }
}
