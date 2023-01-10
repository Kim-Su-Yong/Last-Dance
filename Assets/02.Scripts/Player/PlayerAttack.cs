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
    AudioSource source; 

    // ����
    public GameObject target;       // �ڵ����� Ÿ������ ��ǥ(���ʹ�)

    [Header("���� �� ��ų")]
    // ���� ����
    public GameObject[] FoxFires;   // �����ϴ� ����� �迭 
    public SkillData fireData;      // ����� ��ų ������
    public SkillData healData;      // �� ��ų ������

    // ������Ʈ Ǯ��
    public List<GameObject> fireBallPool = new List<GameObject>();
    int maxCount = 10;    // ������Ʈ Ǯ���� ����

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
    public SkillData buffData;      // ������ ��ų ������

    [Header("���� ����")]
    public bool bIsAttack;          // ���� ������ Ȯ��
    public bool bIsSkill;           // ��ų ��������� Ȯ��
    public bool[] canSkills;        // ��ų ��� ���� ���� ���� �迭

    [SerializeField]
    LayerMask enemyLayer;           // ���ʹ� ������ ���� ���̾�

    [SerializeField]
    LayerMask bossLayer;            // ���� ������ ���� ���̾�

    [Header("UI")]
    public Image[] coolImg;         // ��ų ��Ÿ�� �̹�����
    public Text[] coolTxt;          // ��ų ��Ÿ�� �ؽ�Ʈ��

    [Header("Sounds")]
    public AudioClip fireBallClip;  // ���� �⺻ ���� ����
    public AudioClip foxFireClip;   // ����� ��ų ����
    public AudioClip healClip;      // ���� �� ��ų ����

    public AudioClip punchClip;     // ȣ���� �⺻ ���� ����
    public AudioClip thirdPunchClip;// ȣ���� 3��° ��ġ ����    
    public AudioClip roarClip;      // ȣ���� ��ȿ ��ų ����
    
    public AudioClip bezierClip;    // ������ �⺻ ���� ����
    public AudioClip buffClip;      // ������ ���� ��ų ����

    // ����ȭ�� ���� ����
    readonly int hashAttack = Animator.StringToHash("Attack");
    readonly int hashComoboAttack = Animator.StringToHash("ComboAttack");
    readonly int hashSpeed = Animator.StringToHash("Speed");
    readonly int hashSkillState = Animator.StringToHash("SkillState");

    StandardInput input;

    void Awake()
    {
        playerCtrl = GetComponent<ThirdPersonCtrl>();
        shooter = GetComponent<Shooter>();
        changeForm = GetComponent<ChangeForm>();
        playerState = GetComponent<PlayerState>();
        controller = GetComponent<CharacterController>();

        animator = GetComponent<Animator>();
        source = GetComponent<AudioSource>();

        FireBall = Resources.Load("Player/Magic Fire Ball") as GameObject;

        fireData = Resources.Load("SkillData/FoxFire Data") as SkillData;
        healData = Resources.Load("SkillData/Heal Data") as SkillData;
        roarData = Resources.Load("SkillData/Roar Data") as SkillData;
        buffData = Resources.Load("SkillData/Buff Data") as SkillData;

        input = GetComponent<StandardInput>();
    }

    private void Start()
    {
        CreateFireBallPool();       // ���̾ ������Ʈ Ǯ ����

        foreach(var img in coolImg) // ��Ÿ�� �̹��� �ʱ�ȭ
        {
            img.fillAmount = 0f;
            img.enabled = false;
        }
        foreach(var txt in coolTxt) // ��Ÿ�� �ؽ�Ʈ �ʱ�ȭ
        {
            txt.enabled = false;
        }
    }

    private void OnEnable()
    {
        thirdEffect.Stop();         // ����Ʈ ��ƼŬ ����
        rHandTrail.enabled = false; // ���η����� ��Ȱ��ȭ
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
        if (MouseHover.instance.isUIHover) return;  // UI�� ���콺 ȣ���� �������� ����
        if (input.cursorLocked == false) return;
        Attack();
    }

    private void Attack()
    {
        if (changeForm.curForm == ChangeForm.FormType.FOX)  // ���� ��
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
        else if (changeForm.curForm == ChangeForm.FormType.TIGER)   // ȣ���� ��
        {
            if (Input.GetButtonDown("Fire1") && !bIsSkill)
            {
                TigerBaseAttack();
            }
            if (Input.GetButtonDown("Fire2") && !bIsAttack && !bIsSkill)
            {
                StartCoroutine(TigerSkill_1());
            }
        }
        else if (changeForm.curForm == ChangeForm.FormType.EAGLE)   // ������ ��
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
        animator.SetTrigger(hashAttack);
    }

    //private void TigerBaseAttack(bool isPunching)
    //{
    //    animator.SetBool(hashCombo, isPunching);
    //}

    void TigerBaseAttack()
    {
        animator.SetTrigger(hashComoboAttack);
    }

    void OnFire()       // ���� �⺻ ���� �ִϸ��̼� �̺�Ʈ
    {
        // ������Ʈ Ǯ�� ���
        GameObject _fireBall = GetFireBall();
        if(_fireBall != null)
        {
            _fireBall.transform.position = FirePos.position;
            _fireBall.transform.rotation = FirePos.rotation;
            _fireBall.SetActive(true);
            source.PlayOneShot(fireBallClip);
        }
    }
    void OnAttackStart(int count)   // �⺻ ���� �ִϸ��̼� �̺�Ʈ
    {
        bIsAttack = true;
        LookAtTarget();             // Ÿ���� �ٶ󺸸� ����
        playerState.state = PlayerState.State.ATTACK;   // �÷��̾� ���� = ���� ����
        if(count != 0) // ��ġ����(����, ������ �⺻������ count ���� 0)
        {
            if (count < 3) // �������� �ƴѰ��
            {
                BoxCollider punch = punchCollider[0].GetComponent<BoxCollider>();
                source.PlayOneShot(punchClip);
                StartCoroutine(ColOff(punch));
            }
            else if (count == 3)
                rHandTrail.enabled = true;  // ���� ������ Ȱ��ȭ
            Invoke("TrailOff", 2f);         // 2���� ���� ������ ��Ȱ��ȭ
        }

        moveStop();                         // �����߿� �̵����� �ʵ��� �ϴ� �Լ�
    }

    IEnumerator ColOff(Collider col)        // Ÿ�� �ݶ��̴� ��Ȱ��ȭ �ڷ�ƾ(0.3��)
    {
        col.enabled = true;
        yield return new WaitForSeconds(0.3f);
        col.enabled = false;
    }

    void TrailOff()                         // ���� ������ ��Ȱ��ȭ �Լ�
    {
        rHandTrail.enabled = false;
    }

    void OnAttackEnd(int count)             // �⺻ ���� ���� �Լ�
    {
        bIsAttack = false;
        //if (!animator.GetBool("Combo"))
        playerState.state = PlayerState.State.IDLE; // ���� ���°� ������ IDLE�� ���ƿ�
    }
    #endregion

    #region ��ų
    IEnumerator FoxSkill_1()
    {
        if (canSkills[0])           // ����� ��ų�� ��� ������ ���¶��
        {
            canSkills[0] = false;   // ����� ��ų ��� �Ұ��� ����
            // ��Ÿ�� ǥ�� Ȱ��ȭ
            coolImg[0].enabled = true;      
            coolTxt[0].enabled = true;
            animator.SetInteger(hashSkillState, 1);   // �ִϸ��̼� ����
            // ����Ʈ ���� �� 1���� ����
            GameObject Effect = Instantiate(fireData.skillEffect, transform.position, Quaternion.identity);
            Destroy(Effect, 1f);

            // ��Ÿ�� �ڷ�ƾ ����
            StartCoroutine(CoolTimeImg(fireData.f_skillCoolTime, coolImg[0], coolTxt[0]));
            // ��ų ��Ÿ�� ��ŭ ��ٸ�
            yield return new WaitForSeconds(fireData.f_skillCoolTime);
            // ����� ��ų ���� ���·� ����
            canSkills[0] = true;
        }
        else    // �ش� ��ų�� ��Ÿ�� ���̶��
        {
            // �̺κ� UI�� ǥ���ϸ� ������
            Debug.Log("��ų�� ��Ÿ�� ���Դϴ�.");
        }
    }

    IEnumerator FoxSkill_2()
    {
        if (canSkills[1])   // �� ��ų ��� ������ ���¶��
        {
            // ü���� ������ ������ ��ų�� ������� ����
            if (GetComponent<PlayerDamage>().HpBar.fillAmount == 1f)
            {
                Debug.Log("ü���� ������ �ֽ��ϴ�.");
                yield return null;
            }
            else    // ü���� �������� �ʴٸ�
            {
                canSkills[1] = false;       // �� ��ų �Ұ��� ����
                // ��Ÿ�� ǥ�� Ȱ��ȭ
                coolImg[1].enabled = true;  
                coolTxt[1].enabled = true;
                animator.SetInteger(hashSkillState, 2);   // �ִϸ��̼� ����
                // ��Ÿ�� �ڷ�ƾ ����
                StartCoroutine(CoolTimeImg(healData.f_skillCoolTime, coolImg[1], coolTxt[1]));

                yield return new WaitForSeconds(healData.f_skillCoolTime);
                canSkills[1] = true;    // ��Ÿ���� �� �Ǹ� �� ��ų ��밡���� ����
            }
        }
        else    // �ش� ��ų�� ��Ÿ�� ���̶��
        {
            Debug.Log("��ų�� ��Ÿ�� ���Դϴ�.");
        }
    }

    IEnumerator TigerSkill_1()
    {
        if (canSkills[2])       // ��ȿ ��ų�� ��� ������ ���¶��
        {
            canSkills[2] = false;   // ��ȿ ��ų �Ұ��� ����
            // ��Ÿ�� ǥ�� Ȱ��ȭ
            coolImg[2].enabled = true;
            coolTxt[2].enabled = true;
            animator.SetInteger(hashSkillState, 1);   // �ִϸ��̼� ����
            // ��Ÿ�� �ڷ�ƾ ����
            StartCoroutine(CoolTimeImg(roarData.f_skillCoolTime, coolImg[2], coolTxt[2]));

            yield return new WaitForSeconds(roarData.f_skillCoolTime);
            canSkills[2] = true;    // ��Ÿ���� �� �Ǹ� ��ȿ ��ų ��밡���� ����
        }
        else    // �ش� ��ų�� ��Ÿ�� ���̶��
        {
            // �̺κ� UI�� ǥ���ϸ� ������
            Debug.Log("��ų�� ��Ÿ�� ���Դϴ�.");
        }
    }

    IEnumerator EagleSkill_1()
    {   
        if(canSkills[3])        // ���� ��ų ��밡���� ���¶��
        {
            canSkills[3] = false;   // ���� ��ų �Ұ��� ����
            // ��Ÿ�� ǥ�� Ȱ��ȭ
            coolImg[3].enabled = true;
            coolTxt[3].enabled = true;
            animator.SetInteger(hashSkillState, 1);   // �ִϸ��̼� ����
            // ��Ÿ�� �ڷ�ƾ ����
            StartCoroutine(CoolTimeImg(buffData.f_skillCoolTime, coolImg[3], coolTxt[3]));

            yield return new WaitForSeconds(buffData.f_skillCoolTime);
            canSkills[3] = true;    // ��Ÿ���� �� �Ǹ� ���� ��ų ��밡���� ����
        }
        else    // �ش� ��ų�� ��Ÿ�� ���̶��
        {
            // �̺κ� UI�� ǥ���ϸ� ������
            Debug.Log("��ų�� ��Ÿ�� ���Դϴ�.");
        }
    }
    void OnSkillStart()     // ��ų ���� �ִϸ��̼� �̺�Ʈ
    {
        bIsSkill = true;
        playerState.state = PlayerState.State.ATTACK;   // ��ų ���� ���ݻ��·� ����(�ٸ� ������ ���� ����)
        moveStop(); // ��ų ����� �̵� �Ұ�
    }
    void OnSkillEnd()       // ��ų ���� �ִϸ��̼� �̺�Ʈ
    {
        bIsSkill = false;
        playerState.state = PlayerState.State.IDLE;     // �÷��̾� => IDLE�� ����
        animator.SetInteger(hashSkillState, 0);           // �ִϸ��̼� ���� IDLE
    }

    void OnFireGuard()  // ����� ��ų �ߵ� �̺�Ʈ
    {
        foreach (GameObject fire in FoxFires)   // ����� Ȱ��ȭ(�ִϸ��̼ǰ� Ÿ�̹��� �°� �ϱ� ���� �̺�Ʈ)
        {
            fire.SetActive(true);
            source.PlayOneShot(foxFireClip);
        }
    }

    void OnHeal()       // �� ��ų �ߵ� �̺�Ʈ
    {
        int newHP = (int)(healData.f_skillDamage * PlayerStat.instance.maxHP);    // �ִ�ü���� 50%�� ȸ��
        source.PlayOneShot(healClip);
        GameObject Effect = Instantiate(healData.skillEffect, transform.position, Quaternion.identity); // ����Ʈ Instantiate �� 1.5�� �� ����
        Destroy(Effect, 1.5f);
        PlayerDamage playerDamage = GetComponent<PlayerDamage>();
        playerDamage.RestoreHp(newHP);  // ü�� ȸ��
    }
    void OnClaw()       // ȣ���� ����ġ ��ų �̺�Ʈ
    {
        thirdEffect.Play(); // ����Ʈ ��ƼŬ ����
        BoxCollider claw = punchCollider[1].GetComponent<BoxCollider>();
        source.PlayOneShot(thirdPunchClip);
        StartCoroutine(ColOff(claw)); // ����ġ �ݶ��̴� ��Ȱ��ȭ �ڷ�ƾ ����
    }
    void OnRoar()       // ȣ���� ��ȿ ��ų �̺�Ʈ
    {
        // ����Ʈ Instantiate �� 1�� �� ����
        GameObject Effect = Instantiate(roarData.skillEffect, roarTr.position, roarTr.rotation);
        Destroy(Effect, 1f);
        source.PlayOneShot(roarClip);

        // �ݶ��̴� ��Ȱ��ȭ �ڷ�ƾ ����
        StartCoroutine(ColOff(roarCollider));
    }

    //IEnumerator DownSpeed(GameObject target)    // �׽�Ʈ�Լ�(����ȭ�� ������ ����)
    //{
    //    // ���� ���ʹ� �����ͷ� �ӵ� ���� ���Ѿ���
    //    // *���� �� ����*
    //    target.GetComponent<EnemyDamage>().testSpeed -= 2f;
    //    yield return new WaitForSeconds(2f);
    //    target.GetComponent<EnemyDamage>().testSpeed += 2f;
    //}

    void OnBuff()   // ������ ���� ��ų �̺�Ʈ
    {
        // ����Ʈ Instantiate �� 1.5�� �� ����
        GameObject Effect = Instantiate(buffData.skillEffect, transform.position, Quaternion.identity);
        Destroy(Effect, 1.5f);
        source.PlayOneShot(buffClip);
        // ���ݷ� ���� �ڷ�ƾ ����
        StartCoroutine(AttackBuff());
    }  

    IEnumerator AttackBuff()    // ���ݷ� ���� ���� �ڷ�ƾ
    {
        // ���ݷ� ����(20% + (����/5)), 10���϶� 22���� ����
        int increaseAtk = (int)(PlayerStat.instance.atk * 
            (buffData.f_skillDamage + (int)PlayerStat.instance.character_Lv / 10));
        PlayerStat.instance.atk += increaseAtk;     // �÷��̾� ����(���ݷ�) ����
        yield return new WaitForSeconds(buffData.f_skillRange); // ��ų ���ӽð����ȸ� ���ݷ� ����
        PlayerStat.instance.atk -= increaseAtk;     // ������ ��ŭ ����
    }
    #endregion

    // ��Ÿ�� ǥ�� �ڷ�ƾ
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
