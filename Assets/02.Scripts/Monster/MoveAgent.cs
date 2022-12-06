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

    private NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.autoBraking = false;

        // ���̾��Ű���� WayPointGroup ������Ʈ ����
        var group = GameObject.Find("WayPointGroup");
        if (group != null)
        {
            // WayPointGroup ������ �ִ� ��� Transform ������Ʈ�� ������ ��, List Ÿ���� wayPoints �迭�� �߰�
            group.GetComponentsInChildren<Transform>(wayPoints);
            // �迭 ù ��° �׸� ����(�θ�)
            wayPoints.RemoveAt(0);
        }
    }

    void Update()
    {
        
    }
}
