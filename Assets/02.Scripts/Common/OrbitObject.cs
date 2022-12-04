using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitObject : MonoBehaviour
{
    [SerializeField]
    Transform Target;           // 중심축 위치
    Transform tr;               // 오브젝트 본인 Transform
    public float orbitSpeed;    // 공전 속도
    Vector3 offSet;             // 수류탄과 캐릭터 간의 거리

    readonly string playerTag = "Player";

    private void Awake()
    {
        tr = GetComponent<Transform>();
        
    }
    void Start()
    {
        Target = GameObject.FindGameObjectWithTag(playerTag).GetComponent<Transform>();
        offSet = tr.position - Target.position;
    }

    void Update()
    {
        // 공전하는 물체는 플레이어와의 거리만큼 계속 유지
        tr.position = Target.position + offSet;
        // 공전
        tr.RotateAround(Target.position,
            Vector3.up, orbitSpeed * Time.deltaTime);
        // offSet 값 변경
        offSet = tr.position - Target.position;
    }
}
