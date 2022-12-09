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

    [Header("파이어볼 발사")]
    public Transform FirePos;       // 파이어볼 던져질 발사 위치
    [SerializeField]
    GameObject FireBall;            // 파이어볼 오브젝트
    readonly string playerTag = "Player";
    float fireRate = 1f;          // 발사 대기 시간
    float nextFire = 0f;

    [Header("여우불 가드(스킬1)")]
    public GameObject[] FoxFires;   // 공전하는 여우불 배열 
    public bool canSkill = true;    // 스킬 사용 가능 상태 유무
    float skill1_CoolTime = 10f;
    float skill_CoolTimer;

    [Header("호랑이 펀치")]
    public BoxCollider punchCollider;
    float lastAttackTime = 0f;  // 마지막으로 공격한 시간
    int punchCount = 0;
    readonly int hashCombo = Animator.StringToHash("Combo");
    public GameObject thirdEffect;

    [Header("호랑이 포효(스킬1)")]
    public GameObject roarEffect;           // 포효 이펙트
    public float skill2_CoolTime = 20f;     // 포효 스킬 쿨타임
        
    [Header("제어 변수")]
    public bool bIsAttack;          //  공격 중인지 확인
    public bool bIsSkill;           // 스킬 사용중인지 확인

    [Space(10)]
    public GameObject target;       // 캐릭터 근처에 가장 가까이 있는 적을 자동으로 타겟으로 설정하여 공격시 타겟을 바라보며 공격 모션을 실행
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
        // 호랑이 공격하는 시간 외에는 콜라이더가 꺼져있어야 함
        punchCollider.gameObject.SetActive(false);
    }

    void Update()
    {
        // 캐릭터가 죽은 상태에서는 공격불가
        if (playerState.state == PlayerState.State.DIE||
            playerState.state == PlayerState.State.JUMP)
        {
            return;
        }

        if (changeForm.curForm == ChangeForm.FormType.FOX)
        {
            // 왼쪽 마우스 버튼 누르면 기본 공격(fireRate는 발사 대기 시간)
            if (Input.GetButtonDown("Fire1")&& !bIsSkill)
            {
                StartCoroutine(FoxBaseAttack());
            }
            // 마우스 오른쪽 버튼 누르면 스킬 사용(단, 스킬 사용 가능 상태일때만 발동된다)
            else if (Input.GetButtonDown("Fire2") && !bIsAttack)
            {
                //FoxSkill_1();
                StartCoroutine(FoxSkill_1());
            }
            CoolDown(); // 스킬 쿨타임 관련 코드 추가 필요
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
            CoolDown(); // 스킬 쿨타임 관련 코드 추가 필요
        }
        else if (changeForm.curForm == ChangeForm.FormType.EAGLE)
        {

        }
    }

    #region 기본 공격
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
        if(count != 0) // 펀치공격
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
        Debug.Log("호랑이 세번째 타격");
        punchCollider.gameObject.SetActive(true);
    }
    void OnAttackEnd(int count)
    {
        bIsAttack = false;
        if (!animator.GetBool("Combo"))
            playerState.state = PlayerState.State.IDLE;
        if (count != 0) // 펀치공격
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

    // 공격키를 눌렀을때 타겟을 바라보게 해주는 함수
    void LookAtTarget()
    {
        if(target != null)
        {
            Vector3 dir = new Vector3(target.transform.position.x, transform.position.y, target.transform.position.z);
            transform.LookAt(dir); // 적을 바라보긴 하지만 부자연스러움
        }
    }
}
