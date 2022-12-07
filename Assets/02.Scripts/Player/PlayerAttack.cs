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

    [Header("파이어볼 발사")]
    public Transform FirePos;       // 파이어볼 던져질 발사 위치
    [SerializeField]
    GameObject FireBall;            // 파이어볼 오브젝트
    readonly string playerTag = "Player";
    float fireRate = 1f;          // 발사 대기 시간
    float nextFire = 0f;

    [Header("여우불 가드")]
    public GameObject[] FoxFires;   // 공전하는 여우불 배열 
    public bool canSkill = true;    // 스킬 사용 가능 상태 유무
    float skill1_CoolTime = 10f;
    float skill_CoolTimer;

    [Header("호랑이 펀치")]
    public CapsuleCollider[] punchColliders;
    float lastAttackTime = 0f;  // 마지막으로 공격한 시간
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
        AttackCollider(false);                  // 호랑이 공격하는 시간 외에는 콜라이더가 꺼져있어야 함
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
        // 캐릭터가 죽은 상태에서는 공격불가
        if (GetComponent<PlayerDamage>().isDie ||
            GetComponent<ThirdPersonCtrl>().isGrounded == false)
        {
            return;
        }

        if (changeForm.curForm == ChangeForm.FormType.FOX)
        {
            // 왼쪽 마우스 버튼 누르면 기본 공격(fireRate는 발사 대기 시간)
            if (Input.GetButtonDown("Fire1") && Time.time > nextFire)
            {
                StartCoroutine(FoxBaseAttack());
            }
            // 마우스 오른쪽 버튼 누르면 스킬 사용(단, 스킬 사용 가능 상태일때만 발동된다)
            else if (Input.GetButtonDown("Fire2"))
            {
                //FoxSkill_1();
                StartCoroutine(FoxSkill_1());
            }
            CoolDown(); // 스킬 쿨타임 관련 코드 추가 필요
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
        Debug.Log("공격 시작");
        bIsAttack = true;
        animator.SetFloat("Speed", 0f);
        //playerCtrl.enabled = false;
        shooter.enabled = false;
        changeForm.enabled = false;

    }
    void OnAttackEnd()
    {
        Debug.Log("공격 종료");
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
