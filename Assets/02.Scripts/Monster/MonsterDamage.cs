using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterDamage : MonoBehaviour
{
    // Component
    //private MeshRenderer renderer;

    // Readonly
    private readonly string fireBallTag = "FIREBALL";
    private readonly string bulletTag = "BULLET";
    private readonly string foxFireTag = "FOX_FIRE";
    private readonly string punchTag = "PUNCH";

    private float D_FireBall = 10f;


    // Scripts
    MonsterAI monsterAI;

    void Awake()
    {
        monsterAI = GetComponent<MonsterAI>();
        //renderer = GetComponent<MeshRenderer>();

    }

    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(fireBallTag))
        {
            monsterAI.M_HP -= 10f;
            monsterAI.animator.SetTrigger("GotHit");
            monsterAI.isDamaged = true;
            monsterAI.HpUpdate();
            monsterAI.DamagedUI();

            //monsterAI.monsterRenderer[0].material.SetColor();
            //StartCoroutine(ResetColor());
        }
        if (other.CompareTag(bulletTag))
        {
            BulletAttack();
            monsterAI.animator.SetTrigger("GotHit");
            monsterAI.isDamaged = true;
            monsterAI.HpUpdate();
            monsterAI.DamagedUI();
            //monsterAI.monsterRenderer.material.color = Color.red;
            //StartCoroutine(ResetColor());
        }
        if (other.CompareTag(foxFireTag))
        {
            monsterAI.M_HP -= 20f;
            monsterAI.animator.SetTrigger("GotHit");
            monsterAI.isDamaged = true;
            monsterAI.HpUpdate();
            monsterAI.DamagedUI();
            //monsterAI.monsterRenderer.material.color = Color.red;
            //StartCoroutine(ResetColor());
        }
        if (other.CompareTag(punchTag))
        {
            monsterAI.M_HP -= 15f;
            monsterAI.animator.SetTrigger("GotHit");
            monsterAI.isDamaged = true;
            monsterAI.HpUpdate();
            monsterAI.DamagedUI();
            //monsterAI.monsterRenderer.material.color = Color.red;
            //StartCoroutine(ResetColor());
        }
    }

    void OnDamage()
    {
        //capsuleCollider.enabled = true;
    }

    void BulletAttack()
    {
        monsterAI.M_HP -= 2f;
    }
    void FlyAttack()
    {
        monsterAI.M_HP -= 10f;
        //capsuleCollider.enabled = false;
        //Invoke("OnDamage", 5.0f);
    }
}