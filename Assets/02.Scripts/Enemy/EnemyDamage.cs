using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    Rigidbody rb;
    MeshRenderer renderer;

    readonly string fireBallTag = "FIREBALL";
    readonly string bulletTag = "BULLET";
    readonly string foxFireTag = "FOX_FIRE";
    readonly string punchTag = "PUNCH";
    readonly string roarTag = "ROAR";
    public float hp = 0f;
    public float hpMax = 100f;

    public float testSpeed = 5f;

    PlayerAttack playerAction;
    CapsuleCollider capsuleCollider;

    void Start()
    {
        hp = hpMax;
        rb = GetComponent<Rigidbody>();
        renderer = GetComponent<MeshRenderer>();
        playerAction = GetComponent<PlayerAttack>();
        capsuleCollider = GetComponent<CapsuleCollider>();
    }

    public void OnHitSkill(int newDamage, string skillName)
    {
        hp -= newDamage;
        if(skillName == "Roar")
        {
            // 스피드 감소
            renderer.material.color = Color.red;
            StartCoroutine(ResetColor());
            Debug.Log(newDamage);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag(fireBallTag))
        {
            renderer.material.color = Color.red;
            hp -= other.GetComponent<FireBall>().damage;
            StartCoroutine(ResetColor());
            Debug.Log(other.GetComponent<FireBall>().damage);
        }
        if (other.CompareTag(bulletTag))
        {
            renderer.material.color = Color.red;
            StartCoroutine(ResetColor());
            BulletAttack();
            //Debug.Log(other.GetComponent<Bullet>().damage);
        }
        if(other.CompareTag(foxFireTag))
        {
            renderer.material.color = Color.red;
            hp -= other.GetComponent<FoxFire>().damage;
            StartCoroutine(ResetColor());
            Debug.Log(other.GetComponent<FoxFire>().damage);
        }
        if(other.CompareTag(punchTag))
        {
            renderer.material.color = Color.red;
            
            hp -= other.GetComponent<PunchCollider>().damage;
            StartCoroutine(ResetColor());
            Debug.Log(other.GetComponent<PunchCollider>().damage);
        }
        if(other.CompareTag(roarTag))
        {
            renderer.material.color = Color.red;

            hp -= other.GetComponent<RoarCollider>().damage;
            StartCoroutine(ResetColor());
            Debug.Log(other.GetComponent<RoarCollider>().damage);
        }       
    }

    IEnumerator ResetColor()
    {
        yield return new WaitForSeconds(0.5f);
        renderer.material.color = Color.white;
    }

    void OnDamage()
    {
        capsuleCollider.enabled = true;
    }

    void BulletAttack()
    {
        hp -= 2f;
    }
    void FlyAttack()
    {
        hp -= 10f;
        capsuleCollider.enabled = false;
        Invoke("OnDamage", 5.0f);
    }
}
