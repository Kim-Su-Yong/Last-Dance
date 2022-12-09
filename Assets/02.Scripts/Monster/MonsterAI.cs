using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 할 것 : GotHitTrace 상태 추가하기(Distance 상관없이 바로 추적), 상태에 따라 HP 활성화 여부 구현하기
public class MonsterAI : MonoBehaviour
{
    // ▶ Monster
    public float M_HP = 0f;
    public float M_MaxHP = 100f;

    public float damage = 20f;
    public float timeBetAttack = 0.5f;  // Between
    private float lastAttackTime;       // 마지막 공격 시점



    // ▶ Monster AI Move
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

    // ▶ Bool
    public bool isDie = false;

    // ▶ Components
    private Animator animator;

    // ▶ Scripts
    MoveAgent moveAgent;
    MonsterAttack monsterAttack;

    // ▶ Etc.
    private WaitForSeconds ws;
    private Renderer monsterRenderer;

    // ▶ ReadOnly
    // 프리팹 특성 상 BlendTree > Locomotion 이외에는 Any State -> Trigger 
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
        monsterRenderer.material.color = monsterData.skinColor; // 슬라임한테 쓰거나, 맞았을 때 쓸 예정
    }

    void OnEnable()
    {
        StartCoroutine(CheckState());
        StartCoroutine(Action());
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

        //Invoke("DieAnim", 2.0f); //함수를 f초 이후에 호출한다.
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
