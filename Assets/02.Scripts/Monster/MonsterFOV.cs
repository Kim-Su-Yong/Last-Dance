using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// FOV : Field Of View
public class MonsterFOV : MonoBehaviour
{
    // 적 캐릭터의 추적 사정 거리의 범위
    public float viewRange = 15.0f;
    [Range(0, 360)]
    public float viewAngle = 120f;  //시야각

    private Transform monsterTr;
    private Transform playerTr;
    private int playerLayer;
    private int obstacleLayer;
    private int layerMask;

    void Start()
    {
        // 컴포넌트 추출
        monsterTr = GetComponent<Transform>();
        playerTr = GameObject.FindGameObjectWithTag("Player").transform;

        // 레이어 마스크 값 계산
        playerLayer = LayerMask.NameToLayer("Player");
        obstacleLayer = LayerMask.NameToLayer("OBSTACLE");
        layerMask = 1 << playerLayer | 1 << obstacleLayer;
    }
    
    public Vector3 CirclePoint(float angle) // 주어진 각도에 의해 원주 위의 점의 좌표값을 계산하는 함수
    {
        // 로컬 좌표계를 기준으로 설정하기 위해 적 캐릭터의 y 회전값을 더함
        angle += transform.eulerAngles.y;
        return new Vector3(Mathf.Sign(angle * Mathf.Rad2Deg), 0, Mathf.Cos(angle * Mathf.Deg2Rad));
    }

    public bool isTracePlayer()
    {
        bool isTrace = false;
        // 추적 반경 범위 안에서 플레이어를 추출
        Collider[] colls = Physics.OverlapSphere(monsterTr.position, viewRange, 1 << playerLayer);
        // 배열의 개수가 1일 때 주인공이 범위 안에 있다고 판단
        if (colls.Length == 1)
        {
            // 적 캐릭터와 주인공 사이의 방향 벡터를 계산
            Vector3 dir = (playerTr.position - monsterTr.position).normalized;

            // 적 캐릭터의 시야각에 들어왔는지를 판단
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

        // 적 캐릭터와 주인공 사이의 방향 벡터를 계산
        Vector3 dir = (playerTr.position - monsterTr.position).normalized;

        // 레이캐스트를 투사해서 장애물이 있는지 여부를 판단
        if (Physics.Raycast(monsterTr.position, dir, out hit, viewRange, layerMask))
        {
            isView = (hit.collider.CompareTag("PLAYER"));
        }
        return isView;
    }
}