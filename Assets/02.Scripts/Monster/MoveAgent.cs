using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MoveAgent : MonoBehaviour
{
    // ���� ���� ���� List Ÿ�� ����
    public List<Transform> wayPoints_A;
    public List<Transform> wayPoints_B;
    public List<Transform> wayPoints_C;

    // ���� Ÿ��
    public enum PatrolType { Order, Random }
    public PatrolType patrolType;

    // ���� �������� �迭 Index
    public int nextIdx;

    public float patrolSpeed = 1.5f;
    public float traceSpeed = 4.0f;
    private float damping = 1.0f;

    private NavMeshAgent agent;
    private Transform monsterTr;

    MonsterAI monsterAI;
    MonsterSpawner monsterSpawner;

    private bool _patrolling;
    // patrolling ������Ƽ ���� (getter, setter)
    public bool patrolling
    {
        get { return _patrolling; }
        set
        {
            _patrolling = value;
            if (_patrolling)
            {
                agent.speed = patrolSpeed;
                damping = 1.0f; // ���� ���� ȸ�����

                // ���� Ÿ�Ժ� MoveWayPoint() ����

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
            damping = 7.0f; // ���� ���� ȸ�����
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

        // ���� Ÿ��
        patrolType = PatrolType.Random;

        #region < ���̾��Ű���� WayPointGroup_A, B, C ������Ʈ ���� >
        var group_A = GameObject.Find("WayPointGroup_A");
        if (group_A != null)
        {
            // List Ÿ���� wayPoints �迭�� �߰�
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
        // ���� Ÿ�Ժ� MoveWayPoint() ����
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

    // ��Ʈ�� Ÿ�� ����
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
        // ���� ��尡 �ƴ� ��� ���� ������ �������� ����
        if (!_patrolling) return;

        // ������ ���� ���� ���
        if (agent.velocity.sqrMagnitude >= 0.2f * 0.2f
            && agent.remainingDistance <= 0.5f)
        {
            PatrolTypeNextIdx();
            // ���� Ÿ�Ժ� MoveWayPoint() ����
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
