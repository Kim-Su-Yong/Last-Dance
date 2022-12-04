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
    float delayTime = 0.3f;        // 적을 추적하는데 대기 시간
    [SerializeField]
    LayerMask layerMask = 0;


    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        StartCoroutine(Delay());
    }

    void Update()
    {
        //SeachEnemy();
        //// 표적이 있다면 앞으로 발사
        //if (target != null)
        //{
        //    if (currentSpeed <= speed)
        //        currentSpeed += speed * Time.deltaTime;

        //    transform.position += transform.forward * currentSpeed * Time.deltaTime;

        //    Vector3 dir = (target.position - transform.position).normalized;

        //    // 선형 보간을 통해 자연스럽게 적을 추적하며 날아감
        //    transform.forward = Vector3.Lerp(transform.forward, dir, 0.25f);
        //}
    }

    // 범위 내에 적들 찾는 함수
    void SeachEnemy()
    {
        // 구체 20반경의 범위 안에 적이 검출 된 경우 랜덤으로 적에게 날아감
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
        // 0.3초후에 적을 추적
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
            gameObject.SetActive(false);
        }

        //Destroy(gameObject);
    }
}
