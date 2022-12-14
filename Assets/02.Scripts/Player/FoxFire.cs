using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoxFire : MonoBehaviour
{
    [SerializeField]
    SkillData foxFireData;          // 여우불 스킬데이터
    GameObject expEffect;           // 폭발 이펙트
    //SphereCollider sphereCollider;

    //Rigidbody rb;
    //[SerializeField]
    //Transform target = null;

    //[SerializeField]
    //float speed = 0f;
    //[SerializeField]
    //float currentSpeed = 0f;
    //[SerializeField]
    //float delayTime = 0.3f;        // 적을 추적하는데 대기 시간
    //[SerializeField]
    //LayerMask layerMask = 0;

    void Start()
    {
        //expEffect = Resources.Load()
        //rb = GetComponent<Rigidbody>();
        foxFireData =  Resources.Load("SkillData/FoxFire Data") as SkillData;   // 스킬 데이터 가져오기
        gameObject.SetActive(false);                                            // 처음에는 비활성화
    }

    private void OnEnable()
    {
        StartCoroutine(Delay());    // 5초뒤 자동으로 비활성화 해주는 코루틴 함수
    }

    private void OnDisable()
    {
        StopCoroutine(Delay());     // 비활성화되면(적에게 부딪히면) Delay 코루틴 취소
    }

    //void Update()
    //{
    //    SeachEnemy();
    //    // 표적이 있다면 앞으로 발사
    //    if (target != null)
    //    {
    //        if (currentSpeed <= speed)
    //            currentSpeed += speed * Time.deltaTime;

    //        transform.position += transform.forward * currentSpeed * Time.deltaTime;

    //        Vector3 dir = (target.position - transform.position).normalized;

    //        // 선형 보간을 통해 자연스럽게 적을 추적하며 날아감
    //        transform.forward = Vector3.Lerp(transform.forward, dir, 0.25f);
    //    }
    //}

    //// 범위 내에 적들 찾는 함수
    //void SeachEnemy()
    //{
    //    // 구체 20반경의 범위 안에 적이 검출 된 경우 랜덤으로 적에게 날아감
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
        //// 0.3초후에 적을 추적
        //yield return new WaitForSeconds(delayTime);
        //SeachEnemy();

        // 5초뒤에 오브젝트 비활성화
        yield return new WaitForSeconds(5f);    
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        // 적에게 닿으면 Explosion코루틴 함수 실행
        if (other.gameObject.CompareTag("ENEMY"))
        {
            StartCoroutine(Explosion(0f));
        }
    }

    IEnumerator Explosion(float time)
    {
        yield return new WaitForSeconds(time);
        // 적에게 닿을 시 이펙트 추가 예정
        //GameObject explosion = Instantiate(, transform.position, Quaternion.identity);
        //Destroy(explosion, 1f);
        gameObject.SetActive(false);
    }
}
