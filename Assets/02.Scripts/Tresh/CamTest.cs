using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamTest : MonoBehaviour
{
    public GameObject target;       // Player(��ǥ)    
    Vector3 offset;                 // ������ ����
    public float xMove = 0f;        // x���� ���� �̵���
    public float yMove = 0f;        // y���� ���� �̵���
    public float distance = 8f;     // ĳ���Ϳ� ī�޶��� �Ÿ�

    readonly string playerTag = "Player";

    void Start()
    {
        target = GameObject.FindGameObjectWithTag(playerTag);
        offset = transform.position - target.transform.position;
    }

    void LateUpdate()
    {
        // ī�޶� ĳ���͸� ���� �� ĳ���Ͱ� ������ �Ŀ� �����̴� ���·� ��������
        transform.position = target.transform.position + offset;
    }

    void Update()
    {
        xMove += Input.GetAxis("Mouse X");  // ���콺 �¿� �̵��� ����
        yMove -= Input.GetAxis("Mouse Y");  // ���콺 ���� �̵��� ����
        // ���콺�� ���� �ø��� ���� �÷����� �ϱ⶧���� �����ִ� ���� �ƴ� ���ִ� ��

        transform.rotation = Quaternion.Euler(yMove, xMove, 0f);    // �̵����� ���� ī�޶� �ٶ󺸴� ������ ����
        Vector3 reversDistance = new Vector3(0f, 0f, distance);     // ī�޶� �ٶ󺸴� �չ����� Z��
        transform.position = target.transform.position - transform.rotation * reversDistance;
    }
}
