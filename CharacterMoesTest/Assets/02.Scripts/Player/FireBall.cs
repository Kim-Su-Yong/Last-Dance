using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : MonoBehaviour
{
    Rigidbody rb;

    public float speed = 1000f;

    [Header("���� ����")]
    SphereCollider sphereCollider;
    [SerializeField]
    GameObject ExpEffect;
    

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        ExpEffect = Resources.Load("Explosion") as GameObject;
        sphereCollider = GetComponent<SphereCollider>();
        rb.AddForce(transform.forward * speed);

        StartCoroutine(Explosion(5f));
    }

    private void OnEnable()
    {
        //StartCoroutine(Delay());
        //Destroy(gameObject, 5f);
    }

    void Update()
    {
        //TraceEnemy();
    }

    //private void TraceEnemy()
    //{
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

    // ���� ���� ���� ã�� �Լ�
    //void SearchEnemy()
    //{
    //    // ��ü 100�ݰ��� ���� �ȿ� ���� ���� �� ��� �������� ������ ���ư�
    //    Collider[] cols = Physics.OverlapSphere(transform.position, 100f, layerMask);

    //    if(cols.Length > 0)
    //    {
    //        target = cols[Random.Range(0, cols.Length)].transform;
    //        if (!(target.CompareTag("ENEMY")))
    //            target = null;
    //    }
    //}

    //IEnumerator Delay()
    //{
    //    // 0.3���Ŀ� ���� ����
    //    yield return new WaitForSeconds(delayTime);
    //    SearchEnemy();

    //    yield return new WaitForSeconds(5f);
    //    Destroy(gameObject);
    //}

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            StartCoroutine(Explosion(0f));
        }

        if (other.gameObject.CompareTag("ENEMY"))
        {
            StartCoroutine(Explosion(0f));
        }
            
            
    }

    IEnumerator Explosion(float time)
    {
        yield return new WaitForSeconds(time);
        sphereCollider.enabled = false;
        rb.isKinematic = true;
        GameObject explosion = Instantiate(ExpEffect, transform.position, Quaternion.identity);
        Destroy(explosion, 1f);
        Destroy(gameObject);
    }
}
