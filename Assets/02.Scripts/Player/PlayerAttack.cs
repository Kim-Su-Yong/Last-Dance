using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    int maxCount = 5;    // ������Ʈ Ǯ���� ����

    [Header("���̾ �߻�")]
    public Transform FirePos;       // ���̾ ������ �߻� ��ġ
    GameObject FireBall;            // ���̾ ������Ʈ

    [Header("���� ��ų")]
    public GameObject[] FoxFires;   // �����ϴ� ����� �迭 
    public bool[] canSkills;
    //public bool canSkill = true;    // ��ų ��� ���� ���� ����
    public SkillData fireData;      // ����� ��ų ������
    public SkillData healData;      // �� ��ų ������

    [Header("ȣ���� ��ġ")]
    public BoxCollider[] punchCollider; // ��ġ �浹 �ݶ��̴� �迭
    int punchCount = 0;
    public ParticleSystem thirdEffect;  // ȣ���� ����° ���� ����Ʈ
    public TrailRenderer rHandTrail;

    [Header("ȣ���� ��ų")]
    public SkillData roarData;      // ��ȿ ��ų ������
    public Transform roarTr;        // ��ȿ ����Ʈ ��ġ
        
    [Header("���� ����")]
    public bool bIsAttack;          //  ���� ������ Ȯ��
    public bool bIsSkill;           // ��ų ��������� Ȯ��
    [SerializeField]
    LayerMask enemyLayer;           // ���ʹ� ������ ���� ���̾�

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
        FireBall = Resources.Load("Magic fire") as GameObject;
        animator = GetComponent<Animator>();

        fireData = Resources.Load("SkillData/FoxFire Data") as SkillData;
        roarData = Resources.Load("SkillData/Roar Data") as SkillData;
        healData = Resources.Load("SkillData/Heal Data") as SkillData; 
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
            else if (Input.GetButtonDown("Fire2") && !bIsAttack)
            {
                StartCoroutine(FoxSkill_1());
            }
            else if (Input.GetKeyDown(KeyCode.Q) && !bIsAttack)
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
                punchCount = 0;
            }
            if (Input.GetButtonDown("Fire2") && !bIsAttack)
            {
                StartCoroutine(TigerSkill_1());
            }
        }
        else if (changeForm.curForm == ChangeForm.FormType.EAGLE)
        {
            //Shooter ����
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
            if (count < 3)
            {
                punchCollider[0].gameObject.SetActive(true);
            }
            else if (count == 3)
                rHandTrail.enabled = true;
            Invoke("TrailOff", 2f);
            punchCount++;
        }

        moveStop();
    }

    void TrailOff()
    {
        rHandTrail.enabled = false;
    }

    void OnHitAttack()  // OnThirdAttack���� �ٲ� ����(�ִϸ��̼� �̺�Ʈ�� ����)
    {
        //Debug.Log("ȣ���� ����° Ÿ��");
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
            //float animTime = animator.GetCurrentAnimatorClipInfo(0).Length;
            //yield return new WaitForSeconds(animTime); 
        }
    }

    IEnumerator FoxSkill_2()
    {
        if(canSkills[1])
        {
            canSkills[1] = false;
            coolImg[1].enabled = true;
            coolTxt[1].enabled = true;
            animator.SetInteger("SkillState", 2);
            StartCoroutine(CoolTimeImg(healData.f_skillCoolTime, coolImg[1], coolTxt[1]));

            yield return new WaitForSeconds(healData.f_skillCoolTime);
            canSkills[1] = true;

            //float animTime = animator.GetCurrentAnimatorClipInfo(0).Length;
            //yield return new WaitForSeconds(animTime);
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
            //float animTime = animator.GetCurrentAnimatorClipInfo(0).Length;
            //yield return new WaitForSeconds(animTime);
        }
    }
    void OnSkillStart()
    {
        bIsSkill = true;
        playerState.state = PlayerState.State.ATTACK;
        moveStop();
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
        GameObject Effect = Instantiate(healData.skillEffect, transform.position, Quaternion.identity);
        Destroy(Effect, 1.5f);
        PlayerDamage playerDamage = GetComponent<PlayerDamage>();
        playerDamage.RestoreHp(healData.f_skillDamage);
        //Debug.Log("Heal");
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
    IEnumerator DownSpeed(GameObject target)
    {
        // ���� ���ʹ� �����ͷ� �ӵ� ���� ���Ѿ���
        // *���� �� ����*
        target.GetComponent<EnemyDamage>().testSpeed -= 2f;
        yield return new WaitForSeconds(2f);
        target.GetComponent<EnemyDamage>().testSpeed += 2f;
    }

    void OnSkillEnd()
    {
        bIsSkill = false;
        playerState.state = PlayerState.State.IDLE;
        animator.SetInteger("SkillState", 0);
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
