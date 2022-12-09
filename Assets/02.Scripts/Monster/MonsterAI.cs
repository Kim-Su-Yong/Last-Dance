using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �� �� : GotHitTrace ���� �߰��ϱ�(Distance ������� �ٷ� ����), ���¿� ���� HP Ȱ��ȭ ���� �����ϱ�
public class MonsterAI : MonoBehaviour
{
    // �� Monster
    public float M_HP = 0f;
    public float M_MaxHP = 100f;

    public float damage = 20f;
    public float timeBetAttack = 0.5f;  // Between
    private float lastAttackTime;       // ������ ���� ����



    // �� Monster AI Move
    public enum State
    {
        PATROL,
        TRACE,
        ATTACK,
        DIE
    }
    public State state = State.PATROL;
    private Transform playerTr;
    private Transform monsterTr;

    public float attackDist = 5.0f;
    public float traceDist = 10.0f;

    // �� Bool
    public bool isDie = false;

    // �� Components
    private Animator animator;

    // �� Scripts
    MoveAgent moveAgent;
    MonsterAttack monsterAttack;

    // �� Etc.
    private WaitForSeconds ws;
    private Renderer monsterRenderer;

    // �� ReadOnly
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
        moveAgent = GetComponent<MoveAgent>();

        monsterRenderer = GetComponentInChildren<Renderer>();

        ws = new WaitForSeconds(0.3f);
    }

    public void SetUp(MonsterData monsterData)
    {
        M_MaxHP = monsterData.HP;
        damage = monsterData.damage;
        moveAgent.patrolSpeed = monsterData.patrolSpeed;
        moveAgent.traceSpeed = monsterData.traceSpeed;
        monsterRenderer.material.color = monsterData.skinColor; // ���������� ���ų�, �¾��� �� �� ����
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
                    moveAgent.patrolling = true;
                    break;
                case State.TRACE:
                    moveAgent.traceTarget = playerTr.position;
                    break;
                case State.ATTACK:
                    moveAgent.Stop();
                    break;
                case State.DIE:
                    moveAgent.Stop();
                    break;
            }
        }
    }

    void Update()
    {
        animator.SetFloat(hashSpeed, moveAgent.speed);
    }

    public void Die()
    {
        monsterAttack.isAttack = false;
        isDie = true;
        moveAgent.Stop();

        //anim.SetInteger(hashDieIdx, Random.Range(0, 3));
        //anim.SetTrigger(hashDie);

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
