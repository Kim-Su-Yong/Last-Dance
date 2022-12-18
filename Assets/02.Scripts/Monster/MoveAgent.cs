using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MoveAgent : MonoBehaviour
{
    // 순찰 지점 저장 List 타입 변수
    public List<Transform> wayPoints_A;
    public List<Transform> wayPoints_B;
    public List<Transform> wayPoints_C;

    // 순찰 타입
    public enum PatrolType { Order, Random }
    public PatrolType patrolType;

    // 다음 순찰지점 배열 Index
    public int nextIdx;

    public float patrolSpeed = 1.5f;
    public float traceSpeed = 4.0f;
    private float damping = 1.0f;

    private NavMeshAgent agent;
    private Transform monsterTr;

    MonsterAI monsterAI;
    MonsterSpawner monsterSpawner;

    private bool _patrolling;
    // patrolling 프로퍼티 정의 (getter, setter)
    public bool patrolling
    {
        get { return _patrolling; }
        set
        {
            _patrolling = value;
            if (_patrolling)
            {
                agent.speed = patrolSpeed;
                damping = 1.0f; // 순찰 상태 회전계수

                // 몬스터 타입별 MoveWayPoint() 실행

                switch (monsterAI.monsterType)
                {
                    case MonsterAI.MonsterType.A_Skeleton:
                        MoveWayPoint_A();
                        break;
                    case MonsterAI.MonsterType.B_Fishman:
                        MoveWayPoint_B();
                        break;
                    case MonsterAI.MonsterType.C_Mushroom:
                        MoveWayPoint_C();
                        break;
                }
            }
        }
    }
    private Vector3 _traceTarget;
    public Vector3 traceTarget
    {
        get { return _traceTarget; }
        set
        {
            _traceTarget = value;
            agent.speed = traceSpeed;
            damping = 7.0f; // 추적 상태 회전계수
            TraceTarget(_traceTarget);
        }
    }
    public float speed
    {
        get { return agent.velocity.magnitude; }
    }

    void Awake()
    {
        monsterTr = GetComponent<Transform>();
        agent = GetComponent<NavMeshAgent>();
        agent.autoBraking = false;
        agent.updateRotation = false;
        agent.speed = patrolSpeed;

        monsterAI = GetComponent<MonsterAI>();
        monsterSpawner = GameObject.Find("MonsterA_Spawner").GetComponent<MonsterSpawner>();

        // 순찰 타입
        patrolType = PatrolType.Random;

        #region < 하이어라키에서 WayPointGroup_A, B, C 오브젝트 추출 >
        var group_A = GameObject.Find("WayPointGroup_A");
        if (group_A != null)
        {
            // List 타입의 wayPoints 배열에 추가
            group_A.GetComponentsInChildren<Transform>(wayPoints_A);
            wayPoints_A.RemoveAt(0);
        }
        var group_B = GameObject.Find("WayPointGroup_B");
        if (group_B != null)
        {
            group_B.GetComponentsInChildren<Transform>(wayPoints_B);
            wayPoints_B.RemoveAt(0);
        }
        var group_C = GameObject.Find("WayPointGroup_C");
        if (group_C != null)
        {
            group_C.GetComponentsInChildren<Transform>(wayPoints_C);
            wayPoints_C.RemoveAt(0);
        }
        #endregion

        PatrolTypeNextIdx();
        // 몬스터 타입별 MoveWayPoint() 실행
        switch (monsterAI.monsterType)
        {
            case MonsterAI.MonsterType.A_Skeleton:
                MoveWayPoint_A();
                break;
            case MonsterAI.MonsterType.B_Fishman:
                MoveWayPoint_B();
                break;
            case MonsterAI.MonsterType.C_Mushroom:
                MoveWayPoint_C();
                break;
        }
    }
    void MoveWayPoint_A()
    {
        if (agent.isPathStale) return;

        agent.destination = wayPoints_A[nextIdx].position;
        agent.isStopped = false;
    }

    void MoveWayPoint_B()
    {
        if (agent.isPathStale) return;

        agent.destination = wayPoints_B[nextIdx].position;
        agent.isStopped = false;
    }

    void MoveWayPoint_C()
    {
        if (agent.isPathStale) return;

        agent.destination = wayPoints_B[nextIdx].position;
        agent.isStopped = false;
    }

    // 패트롤 타입 선택
    void PatrolTypeNextIdx()
    {
        switch (monsterAI.monsterType)
        {
            case MonsterAI.MonsterType.A_Skeleton:
                if (patrolType == PatrolType.Order)
                    nextIdx = ++nextIdx % wayPoints_A.Count;
                else if (patrolType == PatrolType.Random)
                    nextIdx = UnityEngine.Random.Range(0, wayPoints_A.Count);
                break;
            case MonsterAI.MonsterType.B_Fishman:
                if (patrolType == PatrolType.Order)
                    nextIdx = ++nextIdx % wayPoints_B.Count;
                else if (patrolType == PatrolType.Random)
                    nextIdx = UnityEngine.Random.Range(0, wayPoints_B.Count);
                break;
            case MonsterAI.MonsterType.C_Mushroom:
                if (patrolType == PatrolType.Order)
                    nextIdx = ++nextIdx % wayPoints_C.Count;
                else if (patrolType == PatrolType.Random)
                    nextIdx = UnityEngine.Random.Range(0, wayPoints_C.Count);
                break;
        }
    }

    void TraceTarget(Vector3 pos)
    {
        if (agent.isPathStale) return;
        agent.destination = pos;
        agent.isStopped = false;
    }

    public void Stop()
    {
        agent.isStopped = true;
        agent.velocity = Vector3.zero;
        _patrolling = false;
    }

    void Update()
    {
        if (agent.isStopped == false)
        {
            Quaternion rot = Quaternion.LookRotation(agent.desiredVelocity);
            monsterTr.rotation = Quaternion.Slerp(monsterTr.rotation, rot, Time.deltaTime * damping);
        }
        // 순찰 모드가 아닐 경우 이후 로직을 실행하지 않음
        if (!_patrolling) return;

        // 목적지 도착 여부 계산
        if (agent.velocity.sqrMagnitude >= 0.2f * 0.2f
            && agent.remainingDistance <= 0.5f)
        {
            PatrolTypeNextIdx();
            // 몬스터 타입별 MoveWayPoint() 실행
            switch (monsterAI.monsterType)
            {
                case MonsterAI.MonsterType.A_Skeleton:
                    MoveWayPoint_A();
                    break;
                case MonsterAI.MonsterType.B_Fishman:
                    MoveWayPoint_B();
                    break;
                case MonsterAI.MonsterType.C_Mushroom:
                    MoveWayPoint_C();
                    break;
            }
        }
    }
}
