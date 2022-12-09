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

    [Header("���̾ �߻�")]
    public Transform FirePos;       // ���̾ ������ �߻� ��ġ
    [SerializeField]
    GameObject FireBall;            // ���̾ ������Ʈ
    readonly string playerTag = "Player";
    float fireRate = 1f;          // �߻� ��� �ð�
    float nextFire = 0f;

    [Header("����� ����(��ų1)")]
    public GameObject[] FoxFires;   // �����ϴ� ����� �迭 
    public bool canSkill = true;    // ��ų ��� ���� ���� ����
    float skill1_CoolTime = 10f;
    float skill_CoolTimer;

    [Header("ȣ���� ��ġ")]
    public BoxCollider punchCollider;
    float lastAttackTime = 0f;  // ���������� ������ �ð�
    int punchCount = 0;
    readonly int hashCombo = Animator.StringToHash("Combo");
    public GameObject thirdEffect;

    [Header("ȣ���� ��ȿ(��ų1)")]
    public GameObject roarEffect;           // ��ȿ ����Ʈ
    public float skill2_CoolTime = 20f;     // ��ȿ ��ų ��Ÿ��
        
    [Header("���� ����")]
    public bool bIsAttack;          //  ���� ������ Ȯ��
    public bool bIsSkill;           // ��ų ��������� Ȯ��

    [Space(10)]
    public GameObject target;       // ĳ���� ��ó�� ���� ������ �ִ� ���� �ڵ����� Ÿ������ �����Ͽ� ���ݽ� Ÿ���� �ٶ󺸸� ���� ����� ����
    void Awake()
    {
        playerCtrl = GetComponent<ThirdPersonCtrl>();
        shooter = GetComponent<Shooter>();
        changeForm = GetComponent<ChangeForm>();
        playerState = GetComponent<PlayerState>();
        controller = GetComponent<CharacterController>();
        FireBall = Resources.Load("Magic fire") as GameObject;
        animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        // ȣ���� �����ϴ� �ð� �ܿ��� �ݶ��̴��� �����־�� ��
        punchCollider.gameObject.SetActive(false);
    }

    void Update()
    {
        // ĳ���Ͱ� ���� ���¿����� ���ݺҰ�
        if (playerState.state == PlayerState.State.DIE||
            playerState.state == PlayerState.State.JUMP)
        {
            return;
        }

        if (changeForm.curForm == ChangeForm.FormType.FOX)
        {
            // ���� ���콺 ��ư ������ �⺻ ����(fireRate�� �߻� ��� �ð�)
            if (Input.GetButtonDown("Fire1")&& !bIsSkill)
            {
                StartCoroutine(FoxBaseAttack());
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

    #region �⺻ ����
    IEnumerator FoxBaseAttack()
    {
        //nextFire = Time.time + fireRate;
        animator.SetTrigger("Attack");
        float animTime = animator.GetCurrentAnimatorClipInfo(0).Length;
        yield return new WaitForSeconds(animTime);
    }

    private void TigerBaseAttack(bool isPunching)
    {
        //AttackCollider(isPunching);
        animator.SetBool(hashCombo, isPunching);
        //if (isPunching)
        //    StartCoroutine(StartPunch());
    }

    //IEnumerator StartPunch()
    //{
    //    if (Time.time - lastAttackTime > 1f)
    //    {
    //        lastAttackTime = Time.time;
    //        punchCount++;
    //        if (punchCount == 3)
    //        {
    //            punchCount = 0;
    //        }
    //        while (isPunching)
    //        {
    //            animator.SetBool(hashCombo, true);
    //            yield return new WaitForSeconds(1f);
    //        }
    //    }
    //}

    void OnFire()
    {
        GameObject Fire = Instantiate(FireBall, FirePos.position, FirePos.rotation);
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
                punchCollider.gameObject.SetActive(true);
            }
            punchCount++;
        }
        
        animator.SetFloat("Speed", 0f);
    }
    void OnHitAttack()
    {
        Debug.Log("ȣ���� ����° Ÿ��");
        punchCollider.gameObject.SetActive(true);
    }
    void OnAttackEnd(int count)
    {
        bIsAttack = false;
        if (!animator.GetBool("Combo"))
            playerState.state = PlayerState.State.IDLE;
        if (count != 0) // ��ġ����
        {
            //punchCollider.gameObject.SetActive(false);
            punchCount = 0;
        }
    }
    #endregion
    IEnumerator FoxSkill_1()
    {
        if (canSkill)
        {
            animator.SetInteger("SkillState", 1);
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
        GameObject rEffect = Instantiate(roarEffect,
            transform.position + transform.up * 2
            , Quaternion.identity);
        Destroy(rEffect, rEffect.GetComponent<ParticleSystem>().duration);
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
}
