using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoxFire : MonoBehaviour
{
    [SerializeField]
    SkillData foxFireData;          // ����� ��ų������
    GameObject expEffect;           // ���� ����Ʈ
    public int damage;
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

    private void Update()
    {
        damage = (int)(foxFireData.f_skillDamage + PlayerStat.instance.atk * 3);
    }

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
        if (other.CompareTag("ENEMY") || other.CompareTag("Boss"))
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
