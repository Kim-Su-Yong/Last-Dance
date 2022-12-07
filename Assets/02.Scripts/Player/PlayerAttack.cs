using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    ChangeForm changeForm;
    ThirdPersonCtrl playerCtrl;
    Shooter shooter;

    Animator animator;
    CharacterController controller;

    [Header("���̾ �߻�")]
    public Transform FirePos;       // ���̾ ������ �߻� ��ġ
    [SerializeField]
    GameObject FireBall;            // ���̾ ������Ʈ
    readonly string playerTag = "Player";
    float fireRate = 1f;          // �߻� ��� �ð�
    float nextFire = 0f;

    [Header("����� ����")]
    public GameObject[] FoxFires;   // �����ϴ� ����� �迭 
    public bool canSkill = true;    // ��ų ��� ���� ���� ����
    float skill1_CoolTime = 10f;
    float skill_CoolTimer;

    [Header("ȣ���� ��ġ")]
    public CapsuleCollider[] punchColliders;
    float lastAttackTime = 0f;  // ���������� ������ �ð�
    int punchCount = 0;
    readonly int hashCombo = Animator.StringToHash("Combo");
    public GameObject thirdEffect;
    bool isPunching;
    public bool bIsAttack;

    void Awake()
    {
        playerCtrl = GetComponent<ThirdPersonCtrl>();
        shooter = GetComponent<Shooter>();
        changeForm = GetComponent<ChangeForm>();
        controller = GetComponent<CharacterController>();
        FireBall = Resources.Load("Magic fire") as GameObject;
        animator = GetComponent<Animator>();


    }

    private void Start()
    {
        AttackCollider(false);                  // ȣ���� �����ϴ� �ð� �ܿ��� �ݶ��̴��� �����־�� ��
    }

    private void AttackCollider(bool isActive)
    {
        foreach (var col in punchColliders)
        {
            col.enabled = isActive;
        }
    }

    void Update()
    {
        // ĳ���Ͱ� ���� ���¿����� ���ݺҰ�
        if (GetComponent<PlayerDamage>().isDie ||
            GetComponent<ThirdPersonCtrl>().isGrounded == false)
        {
            return;
        }

        if (changeForm.curForm == ChangeForm.FormType.FOX)
        {
            // ���� ���콺 ��ư ������ �⺻ ����(fireRate�� �߻� ��� �ð�)
            if (Input.GetButtonDown("Fire1") && Time.time > nextFire)
            {
                StartCoroutine(FoxBaseAttack());
            }
            // ���콺 ������ ��ư ������ ��ų ���(��, ��ų ��� ���� �����϶��� �ߵ��ȴ�)
            else if (Input.GetButtonDown("Fire2"))
            {
                //FoxSkill_1();
                StartCoroutine(FoxSkill_1());
            }
            CoolDown(); // ��ų ��Ÿ�� ���� �ڵ� �߰� �ʿ�
        }
        else if (changeForm.curForm == ChangeForm.FormType.TIGER)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                bIsAttack = true;
                TigerBaseAttack(true);
            }
            else if (Input.GetButtonUp("Fire1"))
            {
                TigerBaseAttack(false);
                //isPunching = false;
                //AttackCollider(false);
                //animator.SetBool(hashCombo, isPunching)
                bIsAttack = false;
                punchCount = 0;
            }
        }
        else if (changeForm.curForm == ChangeForm.FormType.EAGLE)
        {

        }
    }
    IEnumerator FoxSkill_1()
    {
        if (canSkill)
        {
            bIsAttack = true;
            animator.SetInteger("SkillState", 1);
            yield return new WaitForSeconds(0.8f);
            foreach (GameObject fire in FoxFires)
            {
                fire.SetActive(true);
            }
            canSkill = false;
            animator.SetInteger("SkillState", 0);
            yield return new WaitForSeconds(0.7f);
            bIsAttack = false;
        }

    }

    IEnumerator FoxBaseAttack()
    {
        bIsAttack = true;
        nextFire = Time.time + fireRate;
        animator.SetTrigger("Attack");
        yield return new WaitForSeconds(0.4f);
        GameObject Fire = Instantiate(FireBall, FirePos.position, FirePos.rotation);
        //yield return new WaitForSeconds(0.65f);
        //bIsAttack = false;
    }
    void OnAttackStart()
    {
        Debug.Log("���� ����");
        bIsAttack = true;
        animator.SetFloat("Speed", 0f);
        //playerCtrl.enabled = false;
        shooter.enabled = false;
        changeForm.enabled = false;

    }
    void OnAttackEnd()
    {
        Debug.Log("���� ����");
        Debug.Log(controller.velocity);
        bIsAttack = false;
        //playerCtrl.enabled = true;
        shooter.enabled = true;
        changeForm.enabled = true;
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

    private void TigerBaseAttack(bool isPunching)
    {
        AttackCollider(isPunching);
        animator.SetBool(hashCombo, isPunching);
        if (isPunching)
            StartCoroutine(StartPunch());
    }

    IEnumerator StartPunch()
    {
        if (Time.time - lastAttackTime > 1f)
        {
            lastAttackTime = Time.time;
            punchCount++;
            if (punchCount == 3)
            {
                punchCount = 0;
            }
            while (isPunching)
            {
                animator.SetBool(hashCombo, true);
                yield return new WaitForSeconds(1f);
            }
        }
    }

}
