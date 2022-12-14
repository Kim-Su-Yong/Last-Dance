using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoxFire : MonoBehaviour
{
    [SerializeField]
    SkillData foxFireData;          // ����� ��ų������
    GameObject expEffect;           // ���� ����Ʈ
    //SphereCollider sphereCollider;

    //Rigidbody rb;
    //[SerializeField]
    //Transform target = null;

    //[SerializeField]
    //float speed = 0f;
    //[SerializeField]
    //float currentSpeed = 0f;
    //[SerializeField]
    //float delayTime = 0.3f;        // ���� �����ϴµ� ��� �ð�
    //[SerializeField]
    //LayerMask layerMask = 0;

    void Start()
    {
        //expEffect = Resources.Load()
        //rb = GetComponent<Rigidbody>();
        foxFireData =  Resources.Load("SkillData/FoxFire Data") as SkillData;   // ��ų ������ ��������
        gameObject.SetActive(false);                                            // ó������ ��Ȱ��ȭ
    }

    private void OnEnable()
    {
        StartCoroutine(Delay());    // 5�ʵ� �ڵ����� ��Ȱ��ȭ ���ִ� �ڷ�ƾ �Լ�
    }

    private void OnDisable()
    {
        StopCoroutine(Delay());     // ��Ȱ��ȭ�Ǹ�(������ �ε�����) Delay �ڷ�ƾ ���
    }

    //void Update()
    //{
    //    SeachEnemy();
    //    // ǥ���� �ִٸ� ������ �߻�
    //    if (target != null)
    //    {
    //        if (currentSpeed <= speed)
    //            currentSpeed += speed * Time.deltaTime;

    //        transform.position += transform.forward * currentSpeed * Time.deltaTime;

    //        Vector3 dir = (target.position - transform.position).normalized;

    //        // ���� ������ ���� �ڿ������� ���� �����ϸ� ���ư�
    //        transform.forward = Vector3.Lerp(transform.forward, dir, 0.25f);
    //    }
    //}

    //// ���� ���� ���� ã�� �Լ�
    //void SeachEnemy()
    //{
    //    // ��ü 20�ݰ��� ���� �ȿ� ���� ���� �� ��� �������� ������ ���ư�
    //    Collider[] cols = Physics.OverlapSphere(transform.position, 20f, layerMask);

    //    if (cols.Length > 0)
    //    {
    //        target = cols[Random.Range(0, cols.Length)].transform;
    //        if (!(target.CompareTag("ENEMY")))
    //            target = null;
    //    }
    //}

    IEnumerator Delay()
    {
        //// 0.3���Ŀ� ���� ����
        //yield return new WaitForSeconds(delayTime);
        //SeachEnemy();

        // 5�ʵڿ� ������Ʈ ��Ȱ��ȭ
        yield return new WaitForSeconds(5f);    
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        // ������ ������ Explosion�ڷ�ƾ �Լ� ����
        if (other.gameObject.CompareTag("ENEMY"))
        {
            StartCoroutine(Explosion(0f));
        }
    }

    IEnumerator Explosion(float time)
    {
        yield return new WaitForSeconds(time);
        // ������ ���� �� ����Ʈ �߰� ����
        //GameObject explosion = Instantiate(, transform.position, Quaternion.identity);
        //Destroy(explosion, 1f);
        gameObject.SetActive(false);
    }
}
