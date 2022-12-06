using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAI : MonoBehaviour
{
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

    private Animator animator;

    public float attackDist = 5.0f;
    public float traceDist = 10.0f;

    public bool isDie = false;

    private WaitForSeconds ws;
    private MoveAgent moveAgent;

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

        ws = new WaitForSeconds(0.3f);
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
}
