using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerAttack : MonoBehaviour
{
    // ��ũ��Ʈ
    ChangeForm changeForm;
    ThirdPersonCtrl playerCtrl;
    Shooter shooter;
    PlayerState playerState;

    // ������Ʈ
    Animator animator;
    CharacterController controller;

    // ����
    public GameObject target;

    [Header("���� �� ��ų")]

    
    // ���� ����
    public GameObject[] FoxFires;   // �����ϴ� ����� �迭 
    public SkillData fireData;      // ����� ��ų ������
    public SkillData healData;      // �� ��ų ������

    // ������Ʈ Ǯ��
    public List<GameObject> fireBallPool = new List<GameObject>();
    int maxCount = 5;    // ������Ʈ Ǯ���� ����

    public Transform FirePos;       // ���̾ ������ �߻� ��ġ
    GameObject FireBall;            // ���̾ ������Ʈ

    // ȣ���� ��ų
    public BoxCollider[] punchCollider; // ��ġ �浹 �ݶ��̴� �迭
    public ParticleSystem thirdEffect;  // ȣ���� ����° ���� ����Ʈ
    public TrailRenderer rHandTrail;    // ȣ���� ����° ���� ����Ʈ2

    public SkillData roarData;      // ��ȿ ��ų ������
    public SphereCollider roarCollider; // ��ȿ �浹 �ݶ��̴�
    public Transform roarTr;        // ��ȿ ����Ʈ ��ġ

    // ������ ��ų
    public SkillData buffData;

    [Header("���� ����")]
    public bool bIsAttack;          // ���� ������ Ȯ��
    public bool bIsSkill;           // ��ų ��������� Ȯ��
    public bool[] canSkills;        // ��ų ��� ���� ���� ���� �迭

    [SerializeField]
    LayerMask enemyLayer;           // ���ʹ� ������ ���� ���̾�

    [Header("UI")]
    public Image[] coolImg;         // ��ų ��Ÿ�� �̹�����
    public Text[] coolTxt;          // ��ų ��Ÿ�� �ؽ�Ʈ��

    // ����ȭ�� ���� ����
    readonly int hashCombo = Animator.StringToHash("Combo");
    readonly int hashSpeed = Animator.StringToHash("Speed");

    void Awake()
    {
        playerCtrl = GetComponent<ThirdPersonCtrl>();
        shooter = GetComponent<Shooter>();
        changeForm = GetComponent<ChangeForm>();
        playerState = GetComponent<PlayerState>();
        controller = GetComponent<CharacterController>();

        animator = GetComponent<Animator>();

        FireBall = Resources.Load("Player/Magic Fire Ball") as GameObject;

        fireData = Resources.Load("SkillData/FoxFire Data") as SkillData;
        healData = Resources.Load("SkillData/Heal Data") as SkillData;
        roarData = Resources.Load("SkillData/Roar Data") as SkillData;
        buffData = Resources.Load("SkillData/Buff Data") as SkillData;
    }

    private void Start()
    {
        CreateFireBallPool();

        foreach(var img in coolImg)
        {
            img.fillAmount = 0f;
            img.enabled = false;
        }
        foreach(var txt in coolTxt)
        {
            txt.enabled = false;
        }
    }

    private void OnEnable()
    {
        thirdEffect.Stop();
        rHandTrail.enabled = false;
    }
    void Update()
    {
        // ĳ���Ͱ� ���� ���¿����� ���ݺҰ�
        if (playerState.state == PlayerState.State.DIE ||
            playerState.state == PlayerState.State.JUMP ||
            playerState.state == PlayerState.State.HIT)
        {
            return;
        }

        Attack();
    }

    private void Attack()
    {
        if (changeForm.curForm == ChangeForm.FormType.FOX)
        {
            // ���� ���콺 ��ư ������ �⺻ ����(fireRate�� �߻� ��� �ð�)
            if (Input.GetButtonDown("Fire1") && !bIsSkill)
            {
                FoxBaseAttack();
            }
            // ���콺 ������ ��ư ������ ��ų ���(��, ��ų ��� ���� �����϶��� �ߵ��ȴ�)
            else if (Input.GetButtonDown("Fire2") && !bIsAttack && !bIsSkill)
            {
                StartCoroutine(FoxSkill_1());
            }
            else if (Input.GetKeyDown(KeyCode.Q) && !bIsAttack && !bIsSkill)           
            {
                StartCoroutine(FoxSkill_2());
            }
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
            }
            if (Input.GetButtonDown("Fire2") && !bIsAttack && !bIsSkill)
            {
                StartCoroutine(TigerSkill_1());
            }
        }
        else if (changeForm.curForm == ChangeForm.FormType.EAGLE)
        {
            //Shooter ����

            if(Input.GetKeyDown(KeyCode.Q) && !bIsAttack && !bIsSkill)
            {
                StartCoroutine(EagleSkill_1());
            }
        }
    }

    // ����Ű�� �������� Ÿ���� �ٶ󺸰� ���ִ� �Լ�
    void LookAtTarget() 
    {
        if (target != null)
        {
            Vector3 dir = new Vector3(target.transform.position.x, transform.position.y, target.transform.position.z);
            transform.LookAt(dir); // ���� �ٶ󺸱� ������ ���ڿ�������
        }

        //if(target != null)
        //{
        //    Vector3 lookDir = (target.transform.position - transform.position).normalized;

        //    Quaternion from = transform.rotation;
        //    Quaternion to = Quaternion.LookRotation(lookDir);

        //    transform.rotation = Quaternion.Lerp(from, to, Time.deltaTime*5f);
        //}
    }

    #region FireBall ������Ʈ Ǯ��
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
    #endregion

    #region �⺻ ����
    void FoxBaseAttack()
    {
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
            if (count < 3) // �������� �ƴѰ��
            {
                BoxCollider punch = punchCollider[0].GetComponent<BoxCollider>();
                StartCoroutine(ColOff(punch));
            }
            else if (count == 3)
                rHandTrail.enabled = true;
            Invoke("TrailOff", 2f);
        }

        moveStop();
    }

    IEnumerator ColOff(Collider col)
    {
        col.enabled = true;
        yield return new WaitForSeconds(0.3f);
        col.enabled = false;
    }

    void TrailOff()
    {
        rHandTrail.enabled = false;
    }

    void OnAttackEnd(int count)
    {
        bIsAttack = false;
        if (!animator.GetBool("Combo"))
            playerState.state = PlayerState.State.IDLE;
    }
    #endregion

    #region ��ų
    IEnumerator FoxSkill_1()
    {
        if (canSkills[0])
        {
            canSkills[0] = false;
            coolImg[0].enabled = true;
            coolTxt[0].enabled = true;
            animator.SetInteger("SkillState", 1);
            GameObject Effect = Instantiate(fireData.skillEffect, transform.position, Quaternion.identity);
            Destroy(Effect, 1f);

            StartCoroutine(CoolTimeImg(fireData.f_skillCoolTime, coolImg[0], coolTxt[0]));
            yield return new WaitForSeconds(fireData.f_skillCoolTime);
            canSkills[0] = true;
        }
        else
        {
            // �̺κ� UI�� ǥ���ϸ� ������
            Debug.Log("��ų�� ��Ÿ�� ���Դϴ�.");
        }
    }

    IEnumerator FoxSkill_2()
    {
        if (canSkills[1])
        {
            // ü���� ������ ������ ��ų�� ������� ����
            if (GetComponent<PlayerDamage>().HpBar.fillAmount == 1f)
            {
                Debug.Log("ü���� ������ �ֽ��ϴ�.");
                yield return null;
            }
            else
            {
                canSkills[1] = false;
                coolImg[1].enabled = true;
                coolTxt[1].enabled = true;
                animator.SetInteger("SkillState", 2);
                StartCoroutine(CoolTimeImg(healData.f_skillCoolTime, coolImg[1], coolTxt[1]));

                yield return new WaitForSeconds(healData.f_skillCoolTime);
                canSkills[1] = true;
            }
        }
        else
        {
            Debug.Log("��ų�� ��Ÿ�� ���Դϴ�.");
        }
    }

    IEnumerator TigerSkill_1()
    {
        if (canSkills[2])
        {
            canSkills[2] = false;
            coolImg[2].enabled = true;
            coolTxt[2].enabled = true;
            animator.SetInteger("SkillState", 1);

            StartCoroutine(CoolTimeImg(roarData.f_skillCoolTime, coolImg[2], coolTxt[2]));

            yield return new WaitForSeconds(roarData.f_skillCoolTime);
            canSkills[2] = true;
        }
        else
        {
            // �̺κ� UI�� ǥ���ϸ� ������
            Debug.Log("��ų�� ��Ÿ�� ���Դϴ�.");
        }
    }

    IEnumerator EagleSkill_1()
    {
        if(canSkills[3])
        {
            canSkills[3] = false;
            coolImg[3].enabled = true;
            coolTxt[3].enabled = true;
            animator.SetInteger("SkillState", 1);

            StartCoroutine(CoolTimeImg(buffData.f_skillCoolTime, coolImg[3], coolTxt[3]));

            yield return new WaitForSeconds(buffData.f_skillCoolTime);
            canSkills[3] = true;
        }
        else
        {
            // �̺κ� UI�� ǥ���ϸ� ������
            Debug.Log("��ų�� ��Ÿ�� ���Դϴ�.");
        }
    }
    void OnSkillStart()
    {
        bIsSkill = true;
        playerState.state = PlayerState.State.ATTACK;
        moveStop();
    }
    void OnSkillEnd()
    {
        bIsSkill = false;
        playerState.state = PlayerState.State.IDLE;
        animator.SetInteger("SkillState", 0);
    }

    void OnFireGuard()
    {
        foreach (GameObject fire in FoxFires)
        {
            fire.SetActive(true);
        }
    }

    void OnHeal()
    {
        int newHP = (int)(healData.f_skillDamage * PlayerStat.instance.maxHP);    // �ִ�ü���� 50%�� ȸ��
        GameObject Effect = Instantiate(healData.skillEffect, transform.position, Quaternion.identity);
        Destroy(Effect, 1.5f);
        PlayerDamage playerDamage = GetComponent<PlayerDamage>();
        playerDamage.RestoreHp(newHP);
    }
    void OnClaw()
    {
        thirdEffect.Play();
        BoxCollider claw = punchCollider[1].GetComponent<BoxCollider>();
        StartCoroutine(ColOff(claw));
    }
    void OnRoar()
    {
        // ����Ʈ ��ȯ
        GameObject Effect = Instantiate(roarData.skillEffect, roarTr.position, roarTr.rotation);
        Destroy(Effect, 1f);

        StartCoroutine(ColOff(roarCollider));
    }

    IEnumerator DownSpeed(GameObject target)
    {
        // ���� ���ʹ� �����ͷ� �ӵ� ���� ���Ѿ���
        // *���� �� ����*
        target.GetComponent<EnemyDamage>().testSpeed -= 2f;
        yield return new WaitForSeconds(2f);
        target.GetComponent<EnemyDamage>().testSpeed += 2f;
    }

    void OnBuff()
    {
        GameObject Effect = Instantiate(buffData.skillEffect, transform.position, Quaternion.identity);
        Destroy(Effect, 1.5f);
        Debug.Log("���ݷ� ���");    // ĳ���� �����Ϳ� �����Ͽ� ���ݷ� ����ϰ� �� ����
        StartCoroutine(AttackBuff());
    }  

    IEnumerator AttackBuff()
    {
        // ���ݷ� ����(20% + (����/5)), 10���϶� 22���� ����
        int increaseAtk = (int)(PlayerStat.instance.atk * 
            (buffData.f_skillDamage + (int)PlayerStat.instance.character_Lv / 10));
        PlayerStat.instance.atk += increaseAtk;      
        yield return new WaitForSeconds(buffData.f_skillRange); // ��ų ���ӽð����ȸ� ���ݷ� ����
        PlayerStat.instance.atk -= increaseAtk;
    }
    #endregion

    IEnumerator CoolTimeImg(float cool, Image coolImg, Text coolTxt)
    {
        float cooltime = cool;
        while (cooltime > 0)
        {
            cooltime -= Time.deltaTime;
            coolImg.fillAmount = cooltime / cool;
            coolTxt.text = cooltime.ToString("0.0");
            yield return new WaitForFixedUpdate();
        }
        coolTxt.enabled = false;
        coolImg.enabled = false;
        coolImg.fillAmount = 0f;
    }

    // ����, ��ȭ, ��ų ���� �ִϸ����Ϳ��� Speed ���� 0�� ���� �ʱ⶧���� �ִϸ��̼��� ����ؼ� �۵��Ǵ� ���װ� �߻�
    // �̸� ���� ���� �̵��ӵ� ���� ���� 0���� ������ִ� �Լ�
    void moveStop()
    {
        animator.SetFloat(hashSpeed, 0f);
    }
}
