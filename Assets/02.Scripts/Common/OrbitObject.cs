using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitObject : MonoBehaviour
{
    [SerializeField]
    Transform Target;           // �߽��� ��ġ
    Transform tr;               // ������Ʈ ���� Transform
    public float orbitSpeed;    // ���� �ӵ�
    Vector3 offSet;             // ����ź�� ĳ���� ���� �Ÿ�

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
        // �����ϴ� ��ü�� �÷��̾���� �Ÿ���ŭ ��� ����
        tr.position = Target.position + offSet;
        // ����
        tr.RotateAround(Target.position,
            Vector3.up, orbitSpeed * Time.deltaTime);
        // offSet �� ����
        offSet = tr.position - Target.position;
    }
}
