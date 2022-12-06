using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MoveAgent : MonoBehaviour
{
    // 순찰 지점 저장 List 타입 변수
    public List<Transform> wayPoints;
    // 다음 순찰지점 배열 Index
    public int nextIdx;

    private readonly float patrolSpeed = 1.5f;
    private readonly float traceSpeed = 4.0f;
    private float damping = 1.0f;

    private NavMeshAgent agent;
    private Transform monsterTr;

    private bool _patrolling;
    // patrolling 프로퍼티 정의 (getter, setter)
    public bool patrolling
    {
        get { return _patrolling; }
        set {
            _patrolling = value;
            if (_patrolling)
            {
                agent.speed = patrolSpeed;
                damping = 1.0f; // 순찰 상태 회전계수
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
            damping = 7.0f; // 추적 상태 회전계수
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

        // 하이어라키에서 WayPointGroup 오브젝트 추출
        var group = GameObject.Find("WayPointGroup");
        if (group != null)
        {
            // WayPointGroup 하위에 있는 모든 Transform 컴포넌트를 추출한 후, List 타입의 wayPoints 배열에 추가
            group.GetComponentsInChildren<Transform>(wayPoints);
            // 배열 첫 번째 항목 삭제(부모)
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
        // 순찰 모드가 아닐 경우 이후 로직을 실행하지 않음
        if (!_patrolling) return;

        // 목적지 도착 여부 계산
        if (agent.velocity.sqrMagnitude >= 0.2f * 0.2f
            && agent.remainingDistance <= 0.5f)
        {
            nextIdx = ++nextIdx % wayPoints.Count;
            MoveWayPoint();
        }
    }
}
