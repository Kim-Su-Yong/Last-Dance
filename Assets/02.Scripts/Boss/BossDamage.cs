using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class BossDamage : MonoBehaviour
{
    private readonly string fireBallTag = "FIREBALL";
    private readonly string bulletTag = "BULLET";
    private readonly string foxFireTag = "FOX_FIRE";
    private readonly string punchTag = "PUNCH";
    private readonly string roarTag = "ROAR";

    BossAI bossAI;
    BossScript boss;
    public float Offset = 15f;

    // Prefabs
    [SerializeField]
    private GameObject damageUIPrefab;
    private GameObject damageParticlePrefab;

    public GameObject BossHpPanel;
    public Image BossHpBar;


    void Awake()
    {
        bossAI = GetComponent<BossAI>();
        boss = GetComponent<BossScript>();
        damageUIPrefab = Resources.Load<GameObject>("Effects/DamagePopUp");
        damageParticlePrefab = Resources.Load<GameObject>("Effects/HitEffect_A");
    }

    private void Start()
    {
        BossHpBar = BossHpPanel.transform.GetChild(0).GetComponent<Image>();
        BossHpBar.fillAmount = 1f;
        //BossHpPanel.SetActive(false);
    }

    private void Update()
    {
        if (bossAI.isDie)
            BossHpBar.gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (bossAI.isDie == true) return;

        if (other.CompareTag(fireBallTag)) // 파이어볼에 맞았다면
        {
            int _damage = (int)(other.GetComponent<FireBall>().damage + Random.Range(10f, 13f));
            bossAI.beforeHp = bossAI.maxHp;
            bossAI.curHp -= _damage;
            bossAI.curHp = Mathf.Clamp(bossAI.curHp, 0, bossAI.maxHp);

            bossAI.isDamaged = true;
            bossAI.HpUpdate();

            if (!bossAI.isDie)
                ShowDamageEffect(_damage);
        }

        if (other.CompareTag(bulletTag)) // 독수리 공격에 맞았을 때
        {
            int _damage = (int)(other.GetComponent<BezierMissile>().damage + Random.Range(10f, 13f));
            bossAI.beforeHp = bossAI.maxHp;
            bossAI.curHp -= _damage;
            bossAI.curHp = Mathf.Clamp(bossAI.curHp, 0, bossAI.maxHp);

            bossAI.isDamaged = true;
            bossAI.HpUpdate();

            if (!bossAI.isDie)
                ShowDamageEffect(_damage);
        }

        if (other.CompareTag(foxFireTag)) // 여우불에 맞았을 때
        {
            int _damage = (int)(other.GetComponent<FoxFire>().damage + Random.Range(10f, 13f));
            bossAI.beforeHp = bossAI.maxHp;
            bossAI.curHp -= _damage;
            bossAI.curHp = Mathf.Clamp(bossAI.curHp, 0, bossAI.maxHp);

            bossAI.isDamaged = true;
            bossAI.HpUpdate();

            if (!bossAI.isDie)
                ShowDamageEffect(_damage);
        }

        if (other.CompareTag(punchTag)) // 호랑이 공격에 맞았을 때
        {
            int _damage = (int)(other.GetComponent<PunchCollider>().damage + Random.Range(10f, 13f));
            bossAI.beforeHp = bossAI.maxHp;
            bossAI.curHp -= _damage;
            bossAI.curHp = Mathf.Clamp(bossAI.curHp, 0, bossAI.maxHp);

            bossAI.animator.SetBool("TakeDamage", true);
            bossAI.isDamaged = true;
            bossAI.HpUpdate();

            if (!bossAI.isDie)
                ShowDamageEffect(_damage);
        }

        if (other.CompareTag(roarTag))
        {
            int _damage = (int)(other.GetComponent<RoarCollider>().damage + Random.Range(10f, 13f));
            bossAI.beforeHp = bossAI.maxHp;
            bossAI.curHp -= _damage;
            bossAI.curHp = Mathf.Clamp(bossAI.curHp, 0, bossAI.maxHp);

            bossAI.isDamaged = true;
            bossAI.HpUpdate();

            if (!bossAI.isDie)
                ShowDamageEffect(_damage);
        }
        hpUpdate();
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

    void hpUpdate()
    {
        BossHpBar.fillAmount = (float)bossAI.curHp / bossAI.maxHp;
    }
}