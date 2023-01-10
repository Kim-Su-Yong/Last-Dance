using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerAttack : MonoBehaviour
{
    // 스크립트
    ChangeForm changeForm;
    ThirdPersonCtrl playerCtrl;
    Shooter shooter;
    PlayerState playerState;

    // 컴포넌트
    Animator animator;
    CharacterController controller;
    AudioSource source; 

    // 변수
    public GameObject target;       // 자동으로 타겟팅할 목표(에너미)

    [Header("공격 및 스킬")]
    // 여우 공격
    public GameObject[] FoxFires;   // 공전하는 여우불 배열 
    public SkillData fireData;      // 여우불 스킬 데이터
    public SkillData healData;      // 힐 스킬 데이터

    // 오브젝트 풀링
    public List<GameObject> fireBallPool = new List<GameObject>();
    int maxCount = 10;    // 오브젝트 풀링할 개수

    public Transform FirePos;       // 파이어볼 던져질 발사 위치
    GameObject FireBall;            // 파이어볼 오브젝트

    // 호랑이 스킬
    public BoxCollider[] punchCollider; // 펀치 충돌 콜라이더 배열
    public ParticleSystem thirdEffect;  // 호랑이 세번째 공격 이펙트
    public TrailRenderer rHandTrail;    // 호랑이 세번째 공격 이펙트2

    public SkillData roarData;      // 포효 스킬 데이터
    public SphereCollider roarCollider; // 포효 충돌 콜라이더
    public Transform roarTr;        // 포효 이펙트 위치

    // 독수리 스킬
    public SkillData buffData;      // 독수리 스킬 데이터

    [Header("제어 변수")]
    public bool bIsAttack;          // 공격 중인지 확인
    public bool bIsSkill;           // 스킬 사용중인지 확인
    public bool[] canSkills;        // 스킬 사용 가능 상태 유무 배열

    [SerializeField]
    LayerMask enemyLayer;           // 에너미 검출을 위한 레이어

    [SerializeField]
    LayerMask bossLayer;            // 보스 검출을 위한 레이어

    [Header("UI")]
    public Image[] coolImg;         // 스킬 쿨타임 이미지들
    public Text[] coolTxt;          // 스킬 쿨타임 텍스트들

    [Header("Sounds")]
    public AudioClip fireBallClip;  // 여우 기본 공격 사운드
    public AudioClip foxFireClip;   // 여우불 스킬 사운드
    public AudioClip healClip;      // 여우 힐 스킬 사운드

    public AudioClip punchClip;     // 호랑이 기본 공격 사운드
    public AudioClip thirdPunchClip;// 호랑이 3번째 펀치 사운드    
    public AudioClip roarClip;      // 호랑이 포효 스킬 사운드
    
    public AudioClip bezierClip;    // 독수리 기본 공격 사운드
    public AudioClip buffClip;      // 독수리 버프 스킬 사운드

    // 최적화를 위한 변수
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
        CreateFireBallPool();       // 파이어볼 오브젝트 풀 생성

        foreach(var img in coolImg) // 쿨타임 이미지 초기화
        {
            img.fillAmount = 0f;
            img.enabled = false;
        }
        foreach(var txt in coolTxt) // 쿨타임 텍스트 초기화
        {
            txt.enabled = false;
        }
    }

    private void OnEnable()
    {
        thirdEffect.Stop();         // 이펙트 파티클 정지
        rHandTrail.enabled = false; // 라인렌더러 비활성화
    }
    void Update()
    {
        // 캐릭터가 죽은 상태에서는 공격불가
        if (playerState.state == PlayerState.State.DIE ||
            playerState.state == PlayerState.State.JUMP ||
            playerState.state == PlayerState.State.HIT)
        {
            return;
        }
        if (MouseHover.instance.isUIHover) return;  // UI에 마우스 호버시 실행하지 않음
        if (input.cursorLocked == false) return;
        Attack();
    }

    private void Attack()
    {
        if (changeForm.curForm == ChangeForm.FormType.FOX)  // 여우 폼
        {
            // 왼쪽 마우스 버튼 누르면 기본 공격(fireRate는 발사 대기 시간)
            if (Input.GetButtonDown("Fire1") && !bIsSkill)
            {
                FoxBaseAttack();
            }
            // 마우스 오른쪽 버튼 누르면 스킬 사용(단, 스킬 사용 가능 상태일때만 발동된다)
            else if (Input.GetButtonDown("Fire2") && !bIsAttack && !bIsSkill)
            {
                StartCoroutine(FoxSkill_1());
            }
            else if (Input.GetKeyDown(KeyCode.Q) && !bIsAttack && !bIsSkill)           
            {
                StartCoroutine(FoxSkill_2());
            }
        }
        else if (changeForm.curForm == ChangeForm.FormType.TIGER)   // 호랑이 폼
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
        else if (changeForm.curForm == ChangeForm.FormType.EAGLE)   // 독수리 폼
        {
            //Shooter 내용

            if(Input.GetKeyDown(KeyCode.Q) && !bIsAttack && !bIsSkill)
            {
                StartCoroutine(EagleSkill_1());
            }
        }
    }

    // 공격키를 눌렀을때 타겟을 바라보게 해주는 함수
    void LookAtTarget() 
    {
        if (target != null)
        {
            Vector3 dir = new Vector3(target.transform.position.x, transform.position.y, target.transform.position.z);
            transform.LookAt(dir); // 적을 바라보긴 하지만 부자연스러움
        }

        //if(target != null)
        //{
        //    Vector3 lookDir = (target.transform.position - transform.position).normalized;

        //    Quaternion from = transform.rotation;
        //    Quaternion to = Quaternion.LookRotation(lookDir);

        //    transform.rotation = Quaternion.Lerp(from, to, Time.deltaTime*5f);
        //}
    }

    #region FireBall 오브젝트 풀링
    void CreateFireBallPool()   // 오브젝트 풀링용 오브젝트 생성(FireBall)
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

    public GameObject GetFireBall() // 오브젝트 풀링된 FireBall 정보를 가져오는 함수
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

    #region 기본 공격
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

    void OnFire()       // 여우 기본 공격 애니메이션 이벤트
    {
        // 오브젝트 풀링 방식
        GameObject _fireBall = GetFireBall();
        if(_fireBall != null)
        {
            _fireBall.transform.position = FirePos.position;
            _fireBall.transform.rotation = FirePos.rotation;
            _fireBall.SetActive(true);
            source.PlayOneShot(fireBallClip);
        }
    }
    void OnAttackStart(int count)   // 기본 공격 애니메이션 이벤트
    {
        bIsAttack = true;
        LookAtTarget();             // 타겟을 바라보며 공격
        playerState.state = PlayerState.State.ATTACK;   // 플레이어 상태 = 공격 상태
        if(count != 0) // 펀치공격(여우, 독수리 기본공격은 count 값이 0)
        {
            if (count < 3) // 강공격이 아닌경우
            {
                BoxCollider punch = punchCollider[0].GetComponent<BoxCollider>();
                source.PlayOneShot(punchClip);
                StartCoroutine(ColOff(punch));
            }
            else if (count == 3)
                rHandTrail.enabled = true;  // 라인 렌더러 활성화
            Invoke("TrailOff", 2f);         // 2초후 라인 렌더러 비활성화
        }

        moveStop();                         // 공격중에 이동하지 않도록 하는 함수
    }

    IEnumerator ColOff(Collider col)        // 타격 콜라이더 비활성화 코루틴(0.3초)
    {
        col.enabled = true;
        yield return new WaitForSeconds(0.3f);
        col.enabled = false;
    }

    void TrailOff()                         // 라인 렌더러 비활성화 함수
    {
        rHandTrail.enabled = false;
    }

    void OnAttackEnd(int count)             // 기본 공격 종료 함수
    {
        bIsAttack = false;
        //if (!animator.GetBool("Combo"))
        playerState.state = PlayerState.State.IDLE; // 공격 상태가 끝나면 IDLE로 돌아옴
    }
    #endregion

    #region 스킬
    IEnumerator FoxSkill_1()
    {
        if (canSkills[0])           // 여우불 스킬이 사용 가능한 상태라면
        {
            canSkills[0] = false;   // 여우불 스킬 사용 불가능 상태
            // 쿨타임 표시 활성화
            coolImg[0].enabled = true;      
            coolTxt[0].enabled = true;
            animator.SetInteger(hashSkillState, 1);   // 애니메이션 실행
            // 이펙트 생성 후 1초후 제거
            GameObject Effect = Instantiate(fireData.skillEffect, transform.position, Quaternion.identity);
            Destroy(Effect, 1f);

            // 쿨타임 코루틴 시작
            StartCoroutine(CoolTimeImg(fireData.f_skillCoolTime, coolImg[0], coolTxt[0]));
            // 스킬 쿨타임 만큼 기다림
            yield return new WaitForSeconds(fireData.f_skillCoolTime);
            // 여우불 스킬 가능 상태로 변경
            canSkills[0] = true;
        }
        else    // 해당 스킬이 쿨타임 중이라면
        {
            // 이부분 UI로 표시하면 좋을듯
            Debug.Log("스킬이 쿨타임 중입니다.");
        }
    }

    IEnumerator FoxSkill_2()
    {
        if (canSkills[1])   // 힐 스킬 사용 가능한 상태라면
        {
            // 체력이 가득차 있으면 스킬을 사용하지 않음
            if (GetComponent<PlayerDamage>().HpBar.fillAmount == 1f)
            {
                Debug.Log("체력이 가득차 있습니다.");
                yield return null;
            }
            else    // 체력이 꽉차있지 않다면
            {
                canSkills[1] = false;       // 힐 스킬 불가능 상태
                // 쿨타임 표시 활성화
                coolImg[1].enabled = true;  
                coolTxt[1].enabled = true;
                animator.SetInteger(hashSkillState, 2);   // 애니메이션 실행
                // 쿨타임 코루틴 실행
                StartCoroutine(CoolTimeImg(healData.f_skillCoolTime, coolImg[1], coolTxt[1]));

                yield return new WaitForSeconds(healData.f_skillCoolTime);
                canSkills[1] = true;    // 쿨타임이 다 되면 힐 스킬 사용가능한 상태
            }
        }
        else    // 해당 스킬이 쿨타임 중이라면
        {
            Debug.Log("스킬이 쿨타임 중입니다.");
        }
    }

    IEnumerator TigerSkill_1()
    {
        if (canSkills[2])       // 포효 스킬이 사용 가능한 상태라면
        {
            canSkills[2] = false;   // 포효 스킬 불가능 상태
            // 쿨타임 표시 활성화
            coolImg[2].enabled = true;
            coolTxt[2].enabled = true;
            animator.SetInteger(hashSkillState, 1);   // 애니메이션 실행
            // 쿨타임 코루틴 실행
            StartCoroutine(CoolTimeImg(roarData.f_skillCoolTime, coolImg[2], coolTxt[2]));

            yield return new WaitForSeconds(roarData.f_skillCoolTime);
            canSkills[2] = true;    // 쿨타임이 다 되면 포효 스킬 사용가능한 상태
        }
        else    // 해당 스킬이 쿨타임 중이라면
        {
            // 이부분 UI로 표시하면 좋을듯
            Debug.Log("스킬이 쿨타임 중입니다.");
        }
    }

    IEnumerator EagleSkill_1()
    {   
        if(canSkills[3])        // 버프 스킬 사용가능한 상태라면
        {
            canSkills[3] = false;   // 버프 스킬 불가능 상태
            // 쿨타임 표시 활성화
            coolImg[3].enabled = true;
            coolTxt[3].enabled = true;
            animator.SetInteger(hashSkillState, 1);   // 애니메이션 실행
            // 쿨타임 코루틴 실행
            StartCoroutine(CoolTimeImg(buffData.f_skillCoolTime, coolImg[3], coolTxt[3]));

            yield return new WaitForSeconds(buffData.f_skillCoolTime);
            canSkills[3] = true;    // 쿨타임이 다 되면 버프 스킬 사용가능한 상태
        }
        else    // 해당 스킬이 쿨타임 중이라면
        {
            // 이부분 UI로 표시하면 좋을듯
            Debug.Log("스킬이 쿨타임 중입니다.");
        }
    }
    void OnSkillStart()     // 스킬 시작 애니메이션 이벤트
    {
        bIsSkill = true;
        playerState.state = PlayerState.State.ATTACK;   // 스킬 사용시 공격상태로 변경(다른 동작을 막기 위함)
        moveStop(); // 스킬 사용중 이동 불가
    }
    void OnSkillEnd()       // 스킬 종료 애니메이션 이벤트
    {
        bIsSkill = false;
        playerState.state = PlayerState.State.IDLE;     // 플레이어 => IDLE로 변경
        animator.SetInteger(hashSkillState, 0);           // 애니메이션 상태 IDLE
    }

    void OnFireGuard()  // 여우불 스킬 발동 이벤트
    {
        foreach (GameObject fire in FoxFires)   // 여우불 활성화(애니메이션과 타이밍을 맞게 하기 위한 이벤트)
        {
            fire.SetActive(true);
            source.PlayOneShot(foxFireClip);
        }
    }

    void OnHeal()       // 힐 스킬 발동 이벤트
    {
        int newHP = (int)(healData.f_skillDamage * PlayerStat.instance.maxHP);    // 최대체력의 50%를 회복
        source.PlayOneShot(healClip);
        GameObject Effect = Instantiate(healData.skillEffect, transform.position, Quaternion.identity); // 이펙트 Instantiate 후 1.5초 후 제거
        Destroy(Effect, 1.5f);
        PlayerDamage playerDamage = GetComponent<PlayerDamage>();
        playerDamage.RestoreHp(newHP);  // 체력 회복
    }
    void OnClaw()       // 호랑이 강펀치 스킬 이벤트
    {
        thirdEffect.Play(); // 이펙트 파티클 실행
        BoxCollider claw = punchCollider[1].GetComponent<BoxCollider>();
        source.PlayOneShot(thirdPunchClip);
        StartCoroutine(ColOff(claw)); // 강펀치 콜라이더 비활성화 코루틴 실행
    }
    void OnRoar()       // 호랑이 포효 스킬 이벤트
    {
        // 이펙트 Instantiate 후 1초 후 제거
        GameObject Effect = Instantiate(roarData.skillEffect, roarTr.position, roarTr.rotation);
        Destroy(Effect, 1f);
        source.PlayOneShot(roarClip);

        // 콜라이더 비활성화 코루틴 실행
        StartCoroutine(ColOff(roarCollider));
    }

    //IEnumerator DownSpeed(GameObject target)    // 테스트함수(최적화시 제거할 예정)
    //{
    //    // 실제 에너미 데이터로 속도 감소 시켜야함
    //    // *수정 할 예정*
    //    target.GetComponent<EnemyDamage>().testSpeed -= 2f;
    //    yield return new WaitForSeconds(2f);
    //    target.GetComponent<EnemyDamage>().testSpeed += 2f;
    //}

    void OnBuff()   // 독수리 버프 스킬 이벤트
    {
        // 이펙트 Instantiate 후 1.5초 후 제거
        GameObject Effect = Instantiate(buffData.skillEffect, transform.position, Quaternion.identity);
        Destroy(Effect, 1.5f);
        source.PlayOneShot(buffClip);
        // 공격력 증가 코루틴 실행
        StartCoroutine(AttackBuff());
    }  

    IEnumerator AttackBuff()    // 공격력 증가 버프 코루틴
    {
        // 공격력 증가(20% + (레벨/5)), 10렙일때 22프로 증가
        int increaseAtk = (int)(PlayerStat.instance.atk * 
            (buffData.f_skillDamage + (int)PlayerStat.instance.character_Lv / 10));
        PlayerStat.instance.atk += increaseAtk;     // 플레이어 스탯(공격력) 증가
        yield return new WaitForSeconds(buffData.f_skillRange); // 스킬 지속시간동안만 공격력 증가
        PlayerStat.instance.atk -= increaseAtk;     // 증가한 만큼 감소
    }
    #endregion

    // 쿨타임 표시 코루틴
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

    // 공격, 대화, 스킬 사용시 애니메이터에서 Speed 값이 0이 되지 않기때문에 애니메이션이 계속해서 작동되는 버그가 발생
    // 이를 막기 위한 이동속도 변수 값을 0으로 만들어주는 함수
    void moveStop()
    {
        animator.SetFloat(hashSpeed, 0f);
    }
}
