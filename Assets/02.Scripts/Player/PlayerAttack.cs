using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    ChangeForm changeForm;
    ThirdPersonCtrl playerCtrl;
    Shooter shooter;
    PlayerState playerState;

    Animator animator;
    CharacterController controller;

    // ĳ���� ��ó�� ���� ������ �ִ� ���� �ڵ����� Ÿ������ �����Ͽ�
    // ���ݽ� Ÿ���� �ٶ󺸸� ���� ����� ����
    public GameObject target;

    [Header("������Ʈ Ǯ��")]
    public List<GameObject> fireBallPool = new List<GameObject>();
    public int maxCount = 5;    // ������Ʈ Ǯ���� ����


    [Header("���̾ �߻�")]
    public Transform FirePos;       // ���̾ ������ �߻� ��ġ
    [SerializeField]
    GameObject FireBall;            // ���̾ ������Ʈ

    [Header("����� ����(��ų1)")]
    public GameObject[] FoxFires;   // �����ϴ� ����� �迭 
    public bool canSkill = true;    // ��ų ��� ���� ���� ����
    public SkillData fireData;      // ����� ��ų ������
    float skill1_CoolTime = 10f;
    float skill_CoolTimer;

    [Header("ȣ���� ��ġ")]
    public BoxCollider[] punchCollider; // ��ġ �浹 �ݶ��̴� �迭
    int punchCount = 0;
    readonly int hashCombo = Animator.StringToHash("Combo");
    public ParticleSystem thirdEffect;  // ȣ���� ����° ���� ����Ʈ

    [Header("ȣ���� ��ȿ(��ų1)")]
    public SkillData roarData;      // ��ȿ ��ų ������
    public Transform roarTr;        // ��ȿ ����Ʈ ��ġ
        
    [Header("���� ����")]
    public bool bIsAttack;          //  ���� ������ Ȯ��
    public bool bIsSkill;           // ��ų ��������� Ȯ��
    [SerializeField]
    LayerMask enemyLayer;           // ���ʹ� ������ ���� ���̾�

    void Awake()
    {
        playerCtrl = GetComponent<ThirdPersonCtrl>();
        shooter = GetComponent<Shooter>();
        changeForm = GetComponent<ChangeForm>();
        playerState = GetComponent<PlayerState>();
        controller = GetComponent<CharacterController>();
        FireBall = Resources.Load("Magic fire") as GameObject;
        animator = GetComponent<Animator>();

        fireData = Resources.Load("SkillData/FoxFire Data") as SkillData;
        roarData = Resources.Load("SkillData/Roar Data") as SkillData;

        thirdEffect.Stop();
    }

    private void Start()
    {
        //enemyLayer = LayerMask.NameToLayer("ENEMY");
        CreateFireBallPool();
    }

    private void OnEnable()
    {
        // ȣ���� �����ϴ� �ð� �ܿ��� �ݶ��̴��� �����־�� ��
        foreach(var col in punchCollider)
            col.gameObject.SetActive(false);
    }

    void Update()
    {
        // ĳ���Ͱ� ���� ���¿����� ���ݺҰ�
        if (playerState.state == PlayerState.State.DIE||
            playerState.state == PlayerState.State.JUMP||
            playerState.state == PlayerState.State.HIT)
        {
            return;
        }

        if (changeForm.curForm == ChangeForm.FormType.FOX)
        {
            // ���� ���콺 ��ư ������ �⺻ ����(fireRate�� �߻� ��� �ð�)
            if (Input.GetButtonDown("Fire1")&& !bIsSkill)
            {
                FoxBaseAttack();
            }
            // ���콺 ������ ��ư ������ ��ų ���(��, ��ų ��� ���� �����϶��� �ߵ��ȴ�)
            else if (Input.GetButtonDown("Fire2") && !bIsAttack)
            {
                //FoxSkill_1();
                StartCoroutine(FoxSkill_1());
            }
            CoolDown(); // ��ų ��Ÿ�� ���� �ڵ� �߰� �ʿ�
        }
        else if (changeForm.curForm == ChangeForm.FormType.TIGER)
        {
            if (Input.GetButtonDown("Fire1") && !bIsSkill)
            {
                TigerBaseAttack(true);
            }
            else if (Input.GetButtonUp("Fire1"))
            {
                TigerBaseAttack(false);
                punchCount = 0;
            }
            if (Input.GetButtonDown("Fire2") && !bIsAttack)
            {
                StartCoroutine(TigerSkill_1());
            }
            CoolDown(); // ��ų ��Ÿ�� ���� �ڵ� �߰� �ʿ�
        }
        else if (changeForm.curForm == ChangeForm.FormType.EAGLE)
        {

        }
    }

    void CreateFireBallPool()   // ������Ʈ Ǯ���� ������Ʈ ����(FireBall)
    {
        GameObject fireBallPools = new GameObject("FireBallPool");
        for(int i=0; i<maxCount; i++)
        {
            GameObject _fireBall = Instantiate(FireBall, fireBallPools.transform);
            _fireBall.name = "FireBall" + i.ToString("00");
            _fireBall.SetActive(false);
            fireBallPool.Add(_fireBall);
        }
    }

    public GameObject GetFireBall() // ������Ʈ Ǯ���� FireBall ������ �������� �Լ�
    {
        for(int i=0; i<fireBallPool.Count; i++)
        {
            if (fireBallPool[i].activeSelf == false)
            {
                return fireBallPool[i];
            }
        }
        return null;
    }

    #region �⺻ ����
    void FoxBaseAttack()
    {
        //nextFire = Time.time + fireRate;
        animator.SetTrigger("Attack");
    }

    private void TigerBaseAttack(bool isPunching)
    {
        animator.SetBool(hashCombo, isPunching);
    }

    void OnFire()
    {
        // ������Ʈ Ǯ�� ���
        GameObject _fireBall = GetFireBall();
        if(_fireBall != null)
        {
            _fireBall.transform.position = FirePos.position;
            _fireBall.transform.rotation = FirePos.rotation;
            _fireBall.SetActive(true);
        }
    }
    void OnAttackStart(int count)
    {
        bIsAttack = true;
        LookAtTarget();
        playerState.state = PlayerState.State.ATTACK;
        if(count != 0) // ��ġ����
        {
            if (count < 3)
            {
                punchCollider[0].gameObject.SetActive(true);
            }
            punchCount++;
        }
        
        animator.SetFloat("Speed", 0f);
    }
    void OnHitAttack()
    {
        Debug.Log("ȣ���� ����° Ÿ��");
        thirdEffect.Play();
        punchCollider[1].gameObject.SetActive(true);
    }
    void OnAttackEnd(int count)
    {
        bIsAttack = false;
        if (!animator.GetBool("Combo"))
            playerState.state = PlayerState.State.IDLE;
        if (count != 0) // ��ġ����
        {
            punchCount = 0;
        }
    }
    #endregion
    IEnumerator FoxSkill_1()
    {
        if (canSkill)
        {
            animator.SetInteger("SkillState", 1);
            GameObject Effect = Instantiate(fireData.skillEffect, transform.position , Quaternion.identity);
            Destroy(Effect, 1f);
            float animTime = animator.GetCurrentAnimatorClipInfo(0).Length;
            yield return new WaitForSeconds(animTime);
        }
    }

    IEnumerator TigerSkill_1()
    {
        if (canSkill)
        {
            animator.SetInteger("SkillState", 1);
            float animTime = animator.GetCurrentAnimatorClipInfo(0).Length;
            yield return new WaitForSeconds(animTime);
        }
    }
    void OnSkillStart()
    {
        bIsSkill = true;
        playerState.state = PlayerState.State.ATTACK;
        animator.SetFloat("Speed", 0f);
    }

    void OnFireGuard()
    {
        foreach (GameObject fire in FoxFires)
        {
            fire.SetActive(true);
        }
    }

    void OnRoar()
    {
        // ����Ʈ ��ȯ
        GameObject Effect = Instantiate(roarData.skillEffect, roarTr.position, roarTr.rotation);
        Destroy(Effect, 1f);

        // ��ȿ ��ų �ݰ��� ���ʹ� ���̾� �浹ü�� ��� ����
        Collider[] Cols = Physics.OverlapSphere(transform.position, roarData.f_skillRange, enemyLayer);
        
        // �ݶ��̴��� ���ʹ̵��̶�� 
        foreach(Collider col in Cols)
        {
            Rigidbody rb = col.GetComponent<Rigidbody>();
            if(rb != null)
            {
                col.GetComponent<EnemyDamage>().OnHitSkill((int)roarData.f_skillDamage, roarData.skillName);
                StartCoroutine(DownSpeed(col.gameObject));
            }
        }
    }

    void OnSkillEnd()
    {
        bIsSkill = false;
        playerState.state = PlayerState.State.IDLE;
        canSkill = false;
        animator.SetInteger("SkillState", 0);
    }

    void CoolDown()
    {
        if (!canSkill)
        {
            skill1_CoolTime += Time.deltaTime;
            canSkill = true;
            //if(skill_CoolTimer)
        }
    }

    // ����Ű�� �������� Ÿ���� �ٶ󺸰� ���ִ� �Լ�
    void LookAtTarget()
    {
        if(target != null)
        {
            Vector3 dir = new Vector3(target.transform.position.x, transform.position.y, target.transform.position.z);
            transform.LookAt(dir); // ���� �ٶ󺸱� ������ ���ڿ�������
        }
    }

    IEnumerator DownSpeed(GameObject target)
    {
        // ���� ���ʹ� �����ͷ� �ӵ� ���� ���Ѿ���
        // *���� �� ����*
        target.GetComponent<EnemyDamage>().testSpeed -= 2f;
        yield return new WaitForSeconds(2f);
        target.GetComponent<EnemyDamage>().testSpeed += 2f;
    }
}
