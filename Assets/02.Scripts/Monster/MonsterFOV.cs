using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// FOV : Field Of View
public class MonsterFOV : MonoBehaviour
{
    // �� ĳ������ ���� ���� �Ÿ��� ����
    public float viewRange = 15.0f;
    [Range(0, 360)]
    public float viewAngle = 120f;  //�þ߰�

    private Transform monsterTr;
    private Transform playerTr;
    private int playerLayer;
    private int obstacleLayer;
    private int layerMask;

    void Start()
    {
        // ������Ʈ ����
        monsterTr = GetComponent<Transform>();
        playerTr = GameObject.FindGameObjectWithTag("Player").transform;

        // ���̾� ����ũ �� ���
        playerLayer = LayerMask.NameToLayer("Player");
        obstacleLayer = LayerMask.NameToLayer("OBSTACLE");
        layerMask = 1 << playerLayer | 1 << obstacleLayer;
    }
    
    public Vector3 CirclePoint(float angle) // �־��� ������ ���� ���� ���� ���� ��ǥ���� ����ϴ� �Լ�
    {
        // ���� ��ǥ�踦 �������� �����ϱ� ���� �� ĳ������ y ȸ������ ����
        angle += transform.eulerAngles.y;
        return new Vector3(Mathf.Sign(angle * Mathf.Rad2Deg), 0, Mathf.Cos(angle * Mathf.Deg2Rad));
    }

    public bool isTracePlayer()
    {
        bool isTrace = false;
        // ���� �ݰ� ���� �ȿ��� �÷��̾ ����
        Collider[] colls = Physics.OverlapSphere(monsterTr.position, viewRange, 1 << playerLayer);
        // �迭�� ������ 1�� �� ���ΰ��� ���� �ȿ� �ִٰ� �Ǵ�
        if (colls.Length == 1)
        {
            // �� ĳ���Ϳ� ���ΰ� ������ ���� ���͸� ���
            Vector3 dir = (playerTr.position - monsterTr.position).normalized;

            // �� ĳ������ �þ߰��� ���Դ����� �Ǵ�
            if (Vector3.Angle(monsterTr.forward, dir) < viewAngle * 0.5f)
            {
                isTrace = true;
            }
        }
        return isTrace;

    }

    public bool isViewPlayer()
    {
        bool isView = false;
        RaycastHit hit;

        // �� ĳ���Ϳ� ���ΰ� ������ ���� ���͸� ���
        Vector3 dir = (playerTr.position - monsterTr.position).normalized;

        // ����ĳ��Ʈ�� �����ؼ� ��ֹ��� �ִ��� ���θ� �Ǵ�
        if (Physics.Raycast(monsterTr.position, dir, out hit, viewRange, layerMask))
        {
            isView = (hit.collider.CompareTag("PLAYER"));
        }
        return isView;
    }
}