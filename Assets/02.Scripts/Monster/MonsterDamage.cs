using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MonsterDamage : MonoBehaviour
{
    // Component

    // Readonly
    private readonly string fireBallTag = "FIREBALL";
    private readonly string bulletTag = "BULLET";
    private readonly string foxFireTag = "FOX_FIRE";
    private readonly string punchTag = "PUNCH";

    private float fireBall_Damage = 20f;
    private float bullet_Damage = 20f;
    private float foxFire_Damage = 20f;
    private float punch_Damage = 20f;

    // Scripts
    MonsterAI monsterAI;

    // Prefabs
    [SerializeField]
    private GameObject damageUIPrefab;
    private GameObject damageParticlePrefab;

    // Pre
    private Color M_DamageColor = new Color(255f, 110f, 0f);
    private float Offset = 0f;

    void Awake()
    {
        monsterAI = GetComponent<MonsterAI>();
        damageUIPrefab = Resources.Load<GameObject>("Effects/DamagePopUp");
        damageParticlePrefab = Resources.Load<GameObject>("Effects/HitEffect_A");

    }

    void Update()
    {


    }

    private void OnTriggerEnter(Collider other)
    {
        if (monsterAI.isDie == true) return;
        if (other.CompareTag(fireBallTag))
        {
            int _damage = (int) (fireBall_Damage + Random.Range(0f, 9f));
            monsterAI.M_HP -= _damage;
            monsterAI.animator.SetTrigger("GotHit");
            monsterAI.isDamaged = true;
            monsterAI.HpUpdate();
            monsterAI.DamagedUI();

            if (!monsterAI.isDie)
                ShowDamageEffect(other, _damage);
        }
        if (other.CompareTag(bulletTag))
        {
            int _damage = (int) (bullet_Damage + Random.Range(0f, 9f));
            monsterAI.M_HP -= _damage;
            monsterAI.animator.SetTrigger("GotHit");
            monsterAI.isDamaged = true;
            monsterAI.HpUpdate();
            monsterAI.DamagedUI();

            if (!monsterAI.isDie)
                ShowDamageEffect(other, _damage);
        }
        if (other.CompareTag(foxFireTag))
        {
            int _damage = (int) (foxFire_Damage + Random.Range(0f, 9f));
            monsterAI.M_HP -= _damage;
            monsterAI.animator.SetTrigger("GotHit");
            monsterAI.isDamaged = true;
            monsterAI.HpUpdate();
            monsterAI.DamagedUI();

            if (!monsterAI.isDie)
                ShowDamageEffect(other, _damage);
        }
        if (other.CompareTag(punchTag))
        {
            int _damage = (int) (punch_Damage + Random.Range(0f, 9f));
            monsterAI.M_HP -= _damage;
            monsterAI.animator.SetTrigger("GotHit");
            monsterAI.isDamaged = true;
            monsterAI.HpUpdate();
            monsterAI.DamagedUI();

            if (!monsterAI.isDie)
                ShowDamageEffect(other, _damage);
        }
    }

    void FlyAttack()
    {
        monsterAI.M_HP -= 10f;
        //capsuleCollider.enabled = false;
    }

    void ShowDamageEffect(Collider col, int _damage)
    {
        // Monster GotHit Particle Effect
        Vector3 pos = col.ClosestPoint(transform.position);
        Vector3 _normal = transform.position - pos;
        //GameObject Effect_M_GotHit = Instantiate(damageParticlePrefab, pos, Quaternion.identity);
        //Destroy(Effect_M_GotHit, 1f);

        // Monster GotHit Damage Amount Effect
        // MonsterHeader Offset Setting
        switch (monsterAI.monsterType)
        {
            case MonsterAI.MonsterType.A_Skeleton:
                Offset = 2.2f;
                break;
            case MonsterAI.MonsterType.B_Fishman:
                Offset = 1.9f;
                break;
            case MonsterAI.MonsterType.C_Slime:
                Offset = 1.5f;
                break;
        }
        Vector3 MonsterHeader = new Vector3(transform.position.x + Random.Range(-0.5f, 0.5f), 
                                            transform.position.y + Offset, 
                                            transform.position.z + Random.Range(-0.5f, 0.5f));
        GameObject Effect_M_DamageAmount = Instantiate(damageUIPrefab, MonsterHeader, Quaternion.identity, transform);
        Effect_M_DamageAmount.GetComponent<TextMeshPro>().text = _damage.ToString();
        Effect_M_DamageAmount.GetComponent<TextMeshPro>().color = M_DamageColor;
    }
}