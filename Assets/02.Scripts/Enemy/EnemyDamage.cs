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
    public float hp = 0f;
    public float hpMax = 100f;

    PlayerAction playerAction;
    CapsuleCollider capsuleCollider;

    private PlayerStat thePlayerStat;
    void Start()
    {
        hp = hpMax;
        rb = GetComponent<Rigidbody>();
        renderer = GetComponent<MeshRenderer>();
        playerAction = GetComponent<PlayerAction>();
        capsuleCollider = GetComponent<CapsuleCollider>();

        thePlayerStat = FindObjectOfType<PlayerStat>();
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag(fireBallTag))
        {
            renderer.material.color = Color.red;
            hp -= 10f;
            //thePlayerStat.Hit(thePlayerStat.atk);
            StartCoroutine(ResetColor());
        }
        if (other.CompareTag(bulletTag))
        {
            renderer.material.color = Color.red;
            StartCoroutine(ResetColor());
            BulletAttack();
        }
        if(other.CompareTag(foxFireTag))
        {
            renderer.material.color = Color.red;
            StartCoroutine(ResetColor());
            hp -= 20f;
        }
        if(other.CompareTag(punchTag))
        {
            renderer.material.color = Color.red;
            StartCoroutine(ResetColor());
            hp -= 15f;
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
