using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MoveAgent : MonoBehaviour
{
    // ���� ���� ���� List Ÿ�� ����
    public List<Transform> wayPoints;
    // ���� �������� �迭 Index
    public int nextIdx;

    private readonly float patrolSpeed = 1.5f;
    private readonly float traceSpeed = 4.0f;
    private float damping = 1.0f;

    private NavMeshAgent agent;
    private Transform monsterTr;

    private bool _patrolling;
    // patrolling ������Ƽ ���� (getter, setter)
    public bool patrolling
    {
        get { return _patrolling; }
        set {
            _patrolling = value;
            if (_patrolling)
            {
                agent.speed = patrolSpeed;
                damping = 1.0f; // ���� ���� ȸ�����
                MoveWayPoint();
            }
        }
    }
    private Vector3 _traceTarget;
    public Vector3 traceTarget
    {
        get { return _traceTarget; }
        set {
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

    void Start()
    {
        monsterTr = GetComponent<Transform>();
        agent = GetComponent<NavMeshAgent>();
        agent.autoBraking = false;
        agent.updateRotation = false;
        agent.speed = patrolSpeed;

        // ���̾��Ű���� WayPointGroup ������Ʈ ����
        var group = GameObject.Find("WayPointGroup");
        if (group != null)
        {
            // WayPointGroup ������ �ִ� ��� Transform ������Ʈ�� ������ ��, List Ÿ���� wayPoints �迭�� �߰�
            group.GetComponentsInChildren<Transform>(wayPoints);
            // �迭 ù ��° �׸� ����(�θ�)
            wayPoints.RemoveAt(0);
        }

        MoveWayPoint();
    }

    void MoveWayPoint()
    {
        if (agent.isPathStale) return;

        agent.destination = wayPoints[nextIdx].position;
        agent.isStopped = false;
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
            nextIdx = ++nextIdx % wayPoints.Count;
            MoveWayPoint();
        }
    }
}
