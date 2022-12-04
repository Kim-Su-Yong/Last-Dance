using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public Transform target;            // 추적할 대상
    private Transform tr;
    public float height = 4f;           // 카메라의 높이
    public float distance = 5f;         // 카메라와의 거리
    public float moveDamping = 15f;     // 카메라의 이동계수(부드럽게 하는 용도)
    public float rotateDamping = 10f;   // 카메라의 회전계수
    public float targetOffset = 2f;     // 추적 좌표의 오프셋
    private float originHeight;         // 최초의 높이를 저장
    float overDamping = 5f;             // 이동속도 계수

    [Header("Etc Obstacle Setting")]
    public float heightObstacle = 15f;  // 카메라가 장애물에 부딪힐 때 올라갈 높이
    public float castOffset = 1f;       // 주인공에 투사할 Raycast의 높이 오프셋

    void Start()
    {
        tr = GetComponent<Transform>();
        originHeight = height;
    }

    private void Update()
    {
        // 플레이어가 장애물에 가려졌는지 판단할 Raycast의 높낮이를 설정
        Vector3 castTarget = target.position + (target.up * castOffset);
        //castTarget 좌표로의 방향 벡터를 계산
        Vector3 castDir = (castTarget - tr.position).normalized;

        RaycastHit hit;
        // 카메라에서 캐릭터 머리 방향으로 광선을 무한대로 쏴줌
        if (Physics.Raycast(tr.position, castDir, out hit, Mathf.Infinity))
        {
            // 플레이어가 맞지 않았다면(장애물이 있다면)
            if (!hit.collider.CompareTag("Player"))
            {
                height = Mathf.Lerp(height, heightObstacle, Time.deltaTime * overDamping);
            }
            else
            {
                height = Mathf.Lerp(height, originHeight, Time.deltaTime * overDamping);
            }
        }
    }

    void LateUpdate()   // 캐릭터가 이동한 후에 움직여야하므로 LateUpdate 사용
    {
        // 카메라의 위치(백뷰)
        Vector3 camPos = target.position - (target.forward * distance)
                                     + (target.up * height);
        tr.position = Vector3.Lerp(tr.position, camPos, Time.deltaTime * moveDamping);
        tr.rotation = Quaternion.Slerp(tr.rotation, target.rotation, Time.deltaTime * rotateDamping);
        tr.LookAt(target.position + (target.up * targetOffset));  // 카메라를 바라본다
    }

    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.green;
    //    Gizmos.DrawWireSphere(target.position + (target.up * targetOffset), 1f);
    //    Gizmos.DrawLine(target.position + (target.up * targetOffset), transform.position);
    //    Gizmos.color = Color.yellow;
    //    Gizmos.DrawWireSphere(transform.position, 0.5f);
    //}
}
