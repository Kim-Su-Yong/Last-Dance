using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoxFire : MonoBehaviour
{
    Rigidbody rb;
    [SerializeField]
    Transform target = null;

    [SerializeField]
    float speed = 0f;
    [SerializeField]
    float currentSpeed = 0f;
    [SerializeField]
    float delayTime = 0.3f;        // ���� �����ϴµ� ��� �ð�
    [SerializeField]
    LayerMask layerMask = 0;

    [SerializeField]
    SkillData foxFireData;
    SphereCollider collider;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        foxFireData =  Resources.Load("SkillData/FoxFire Data") as SkillData;
        collider = GetComponent<SphereCollider>();
    }

    private void OnEnable()
    {
        collider.enabled = true;
        rb.isKinematic = false;
        //StartCoroutine(Delay());
    }

    void Update()
    {
        //SeachEnemy();
        //// ǥ���� �ִٸ� ������ �߻�
        //if (target != null)
        //{
        //    if (currentSpeed <= speed)
        //        currentSpeed += speed * Time.deltaTime;

        //    transform.position += transform.forward * currentSpeed * Time.deltaTime;

        //    Vector3 dir = (target.position - transform.position).normalized;

        //    // ���� ������ ���� �ڿ������� ���� �����ϸ� ���ư�
        //    transform.forward = Vector3.Lerp(transform.forward, dir, 0.25f);
        //}
    }

    // ���� ���� ���� ã�� �Լ�
    void SeachEnemy()
    {
        // ��ü 20�ݰ��� ���� �ȿ� ���� ���� �� ��� �������� ������ ���ư�
        Collider[] cols = Physics.OverlapSphere(transform.position, 20f, layerMask);

        if (cols.Length > 0)
        {
            target = cols[Random.Range(0, cols.Length)].transform;
            if (!(target.CompareTag("ENEMY")))
                target = null;
        }
    }

    IEnumerator Delay()
    {
        // 0.3���Ŀ� ���� ����
        yield return new WaitForSeconds(delayTime);
        SeachEnemy();

        yield return new WaitForSeconds(5f);
        //Destroy(gameObject);
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("ENEMY"))
        {
            StartCoroutine(Explosion(0f));
            //gameObject.SetActive(false);
        }

        //Destroy(gameObject);
    }

    IEnumerator Explosion(float time)
    {
        yield return new WaitForSeconds(time);
        collider.enabled = false;
        rb.isKinematic = true;
        GameObject explosion = Instantiate(foxFireData.skillEffect, transform.position, Quaternion.identity);
        Destroy(explosion, 1f);
        //Destroy(gameObject);
        gameObject.SetActive(false);
    }
}
