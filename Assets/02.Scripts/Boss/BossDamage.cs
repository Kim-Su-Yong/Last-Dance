using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BossDamage : MonoBehaviour
{
    private readonly string fireBallTag = "FIREBALL";
    private readonly string bulletTag = "BULLET";
    private readonly string foxFireTag = "FOX_FIRE";
    private readonly string punchTag = "PUNCH";
    private readonly string roarTag = "ROAR";

    BossLifeBarScript bossLife;
    BossScript boss;
    public float Offset = 15f;

    // Prefabs
    [SerializeField]
    private GameObject damageUIPrefab;
    private GameObject damageParticlePrefab;

    void Awake()
    {
        bossLife = GetComponent<BossLifeBarScript>();
        boss = GetComponent<BossScript>();
        damageUIPrefab = Resources.Load<GameObject>("Effects/DamagePopUp");
        damageParticlePrefab = Resources.Load<GameObject>("Effects/HitEffect_A");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (bossLife.isDie == true) return;
        if (other.CompareTag(fireBallTag)) // 파이어볼에 맞았다면
        { 
            int _damage = (int)(other.GetComponent<FireBall>().damage + Random.Range(0f, 3f));
            bossLife.beforeHp = bossLife.maxLife;
            bossLife.life -= _damage;
            bossLife.life = Mathf.Clamp(bossLife.life, 0, bossLife.maxLife);

            bossLife.bossAnim.SetTrigger("TakeDamage");
            bossLife.isDamaged = true;

            if (!bossLife.isDie)
                ShowDamageEffect(_damage);
        }

        if (other.CompareTag(bulletTag)) // 독수리 공격에 맞았을 때
        {
            int _damage = (int)(other.GetComponent<BezierMissile>().damage);
            bossLife.beforeHp = bossLife.maxLife;
            bossLife.life -= _damage;
            bossLife.life = Mathf.Clamp(bossLife.life, 0, bossLife.maxLife);

            bossLife.bossAnim.SetTrigger("TakeDamage");
            bossLife.isDamaged = true;

            if (!bossLife.isDie)
                ShowDamageEffect(_damage);
        }

        if (other.CompareTag(foxFireTag)) // 여우불에 맞았을 때
        {
            int _damage = (int)(other.GetComponent<FoxFire>().damage + Random.Range(0f, 5f));
            bossLife.beforeHp = bossLife.maxLife;
            bossLife.life -= _damage;
            bossLife.life = Mathf.Clamp(bossLife.life, 0, bossLife.maxLife);

            bossLife.bossAnim.SetTrigger("TakeDamage");
            bossLife.isDamaged = true;

            if (!bossLife.isDie)
                ShowDamageEffect(_damage);
        }

        if (other.CompareTag(punchTag)) // 호랑이 공격에 맞았을 때
        {
            int _damage = (int)(other.GetComponent<PunchCollider>().damage + Random.Range(0f, 5f));
            bossLife.beforeHp = bossLife.maxLife;
            bossLife.life -= _damage;
            bossLife.life = Mathf.Clamp(bossLife.life, 0, bossLife.maxLife);

            bossLife.bossAnim.SetTrigger("TakeDamage");
            bossLife.isDamaged = true;

            if (!bossLife.isDie)
                ShowDamageEffect(_damage);
        }

        if (other.CompareTag(roarTag))
        {
            int _damage = (int)(other.GetComponent<RoarCollider>().damage + Random.Range(0f, 3f));
            bossLife.beforeHp = bossLife.maxLife;
            bossLife.life -= _damage;
            bossLife.life = Mathf.Clamp(bossLife.life, 0, bossLife.maxLife);

            bossLife.bossAnim.SetTrigger("TakeDamage");
            bossLife.isDamaged = true;

            if (!bossLife.isDie)
                ShowDamageEffect(_damage);
        }
    }

    void ShowDamageEffect(int _damage)
    {
        Vector3 MonsterHeader = new Vector3(transform.position.x + Random.Range(-0.5f, 0.5f),
                                            transform.position.y + Offset,
                                            transform.position.z + Random.Range(-0.5f, 0.5f));
        GameObject Effect_M_DamageAmount = Instantiate(damageUIPrefab,
                                           MonsterHeader,
                                           Quaternion.identity,
                                           transform);
        Effect_M_DamageAmount.GetComponent<TextMeshPro>().text = _damage.ToString();
    }
}