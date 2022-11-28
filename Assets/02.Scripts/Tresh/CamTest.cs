using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamTest : MonoBehaviour
{
    public GameObject target;       // Player(목표)    
    Vector3 offset;                 // 오프셋 벡터
    public float xMove = 0f;        // x축의 누적 이동량
    public float yMove = 0f;        // y축의 누적 이동량
    public float distance = 8f;     // 캐릭터와 카메라의 거리

    readonly string playerTag = "Player";

    void Start()
    {
        target = GameObject.FindGameObjectWithTag(playerTag);
        offset = transform.position - target.transform.position;
    }

    void LateUpdate()
    {
        // 카메라가 캐릭터를 따라갈 때 캐릭터가 움직인 후에 움직이는 형태로 만들어야함
        transform.position = target.transform.position + offset;
    }

    void Update()
    {
        xMove += Input.GetAxis("Mouse X");  // 마우스 좌우 이동량 누적
        yMove -= Input.GetAxis("Mouse Y");  // 마우스 상하 이동량 누적
        // 마우스를 위로 올리면 위를 올려봐야 하기때문에 더해주는 것이 아닌 뺴주는 것

        transform.rotation = Quaternion.Euler(yMove, xMove, 0f);    // 이동량에 따라 카메라가 바라보는 방향을 조정
        Vector3 reversDistance = new Vector3(0f, 0f, distance);     // 카메라가 바라보는 앞방향은 Z축
        transform.position = target.transform.position - transform.rotation * reversDistance;
    }
}
