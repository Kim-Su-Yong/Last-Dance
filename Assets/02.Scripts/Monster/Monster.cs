using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Monster : MonoBehaviour
{
    // 몬스터 타입
    public enum Type {  A, B, C };
    public Type MonsterType;

    public int maxHealth;
    public int curHealth;

    public Transform target;
    public BoxCollider meleeArea;
    public GameObject bullet;

    public bool isChase;
    public bool isAttack;

    Rigidbody rigid;
    BoxCollider boxCollider;
    [SerializeField]
    Material mat;
    NavMeshAgent nav;
    Animator anim;
    [SerializeField]
    Rigidbody rigidBullet;

    void Awake()
    {
        curHealth = 100;
        rigid = GetComponent<Rigidbody>();
        boxCollider = GetComponent<BoxCollider>();

        //bullet = Resources.Load<GameObject>("Missile");
        //meleeArea = transform.GetChild(1).GetComponent<BoxCollider>();
        // └ Type.C에서 문제 발생하여 끌어다 놓는 것으로 대체함
        //meleeArea.enabled = false;

        mat = GetComponentInChildren<MeshRenderer>().material;
        mat.color = Color.white;
        nav = GetComponent<NavMeshAgent>();
        anim = GetComponentInChildren<Animator>();

        Invoke("ChaseStart", 2);
    }
    void Start()
    {
    }
    void ChaseStart()
    {
        isChase = true;
        anim.SetBool("isWalk", true);
    }
    void Update()
    {
        if (nav.enabled)
        {
            nav.SetDestination(target.position);
            nav.isStopped = !isChase;
        }
    }
    void FreezeVelocity()
    {
        if (isChase)
        {
            // 물리력이 Nav agent의 이동을 방해하지 않도록 로직 추가
            rigid.velocity = Vector3.zero;
            rigid.angularVelocity = Vector3.zero;
        }
    }
    void Targeting()
    {
        float targetRadius = 0f;
        float targetRange = 3f;

        switch (MonsterType)
        {
            case Type.A:
                targetRadius = 1.5f;
                targetRange = 3.0f;
                break;
            case Type.B:
                targetRadius = 1.0f;
                targetRange = 10.0f;
                break;
            case Type.C:
                targetRadius = 0.5f;
                targetRange = 25.0f;
                break;
        }
        RaycastHit[] rayHits =
            Physics.SphereCastAll(transform.position,
                                  targetRadius,
                                  transform.forward,
                                  targetRange,
                                  LayerMask.GetMask("Player"));
        if (rayHits.Length > 0 && !isAttack)
        {
            StartCoroutine(Attack());
        }
    }
    IEnumerator Attack()
    {
        isChase = false;
        isAttack = true;
        anim.SetBool("isAttack", true);

        switch (MonsterType)
        {
            case Type.A:
                yield return new WaitForSeconds(0.2f);
                meleeArea.enabled = true;

                yield return new WaitForSeconds(1f);
                meleeArea.enabled = false;

                yield return new WaitForSeconds(1f);
                break;

            case Type.B:
                yield return new WaitForSeconds(0.1f);
                rigid.AddForce(transform.forward * 20, ForceMode.Impulse);
                meleeArea.enabled = true;

                yield return new WaitForSeconds(0.5f);
                rigid.velocity = Vector3.zero;
                meleeArea.enabled = false;

                yield return new WaitForSeconds(2f);
                break;
            case Type.C:
                yield return new WaitForSeconds(0.5f);
                GameObject instantBullet = Instantiate<GameObject>(bullet, transform.position, transform.rotation);
                rigidBullet = instantBullet.GetComponent<Rigidbody>();
                rigidBullet.velocity = transform.forward * 20;

                yield return new WaitForSeconds(2.0f);
                break;
        }

        isChase = true;
        isAttack = false;
        anim.SetBool("isAttack", false);
        
    }
    void FixedUpdate()
    {
        Targeting();
        FreezeVelocity(); 
    }
    
    private void OnTriggerEnter(Collider other)
    {
        //if (other.tag == "Melee")
        //{
        //    Weapon weapon = other.GetComponent<Weapon>();
        //    curHealth -= weapon.damage;
        //    Vector3 reactVec = transform.position - other.transform.position;

        //    StartCoroutine(OnDamage(reactVec, false));
        //    Debug.Log("Melee : " + curHealth);
        //}
        //else if (other.tag == "Bullet")
        //{
        //    Bullet bullet = other.GetComponent<Bullet>();
        //    curHealth -= bullet.damage;

        //    Debug.Log("Range : " + curHealth);
        //}
    }

    IEnumerator OnDamage(Vector3 reactVec, bool isGrenade)
    {
        mat.color = Color.red; // new Color(R, G, B, Alpha) 난듕에 바꾸쟈
        yield return new WaitForSeconds(0.1f);

        if(curHealth > 0)
        {
            mat.color = Color.white;
        }
        else // (curHealth <= 0)
        {
            mat.color = Color.gray;
            gameObject.layer = 9;
            isChase = false;
            nav.enabled = false;
            anim.SetTrigger("doDie");
            //Destroy(gameObject, 4f);

            if (isGrenade)
            {
                reactVec = reactVec.normalized;
                reactVec += Vector3.up * 3;

                rigid.freezeRotation = false;
                rigid.AddForce(reactVec * 5, ForceMode.Impulse);
                rigid.AddTorque(reactVec * 15, ForceMode.Impulse);
                // └ Torque(토크) : 회전력
            }
            else
            {
                reactVec = reactVec.normalized;
                reactVec += Vector3.up;
                rigid.AddForce(reactVec * 5, ForceMode.Impulse);
            }
            Destroy(gameObject, 4);
        }
    }
}
