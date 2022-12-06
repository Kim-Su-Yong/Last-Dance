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

    private NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.autoBraking = false;

        // 하이어라키에서 WayPointGroup 오브젝트 추출
        var group = GameObject.Find("WayPointGroup");
        if (group != null)
        {
            // WayPointGroup 하위에 있는 모든 Transform 컴포넌트를 추출한 후, List 타입의 wayPoints 배열에 추가
            group.GetComponentsInChildren<Transform>(wayPoints);
            // 배열 첫 번째 항목 삭제(부모)
            wayPoints.RemoveAt(0);
        }
    }

    void Update()
    {
        
    }
}
