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

    // 캐릭터 근처에 가장 가까이 있는 적을 자동으로 타겟으로 설정하여
    // 공격시 타겟을 바라보며 공격 모션을 실행
    public GameObject target;

    [Header("오브젝트 풀링")]
    public List<GameObject> fireBallPool = new List<GameObject>();
    int maxCount = 5;    // 오브젝트 풀링할 개수

    [Header("파이어볼 발사")]
    public Transform FirePos;       // 파이어볼 던져질 발사 위치
    GameObject FireBall;            // 파이어볼 오브젝트

    [Header("여우 스킬")]
    public GameObject[] FoxFires;   // 공전하는 여우불 배열 
    public bool[] canSkills;
    //public bool canSkill = true;    // 스킬 사용 가능 상태 유무
    public SkillData fireData;      // 여우불 스킬 데이터
    public SkillData healData;      // 힐 스킬 데이터

    [Header("호랑이 펀치")]
    public BoxCollider[] punchCollider; // 펀치 충돌 콜라이더 배열
    int punchCount = 0;
    public ParticleSystem thirdEffect;  // 호랑이 세번째 공격 이펙트
    public TrailRenderer rHandTrail;

    [Header("호랑이 스킬")]
    public SkillData roarData;      // 포효 스킬 데이터
    public Transform roarTr;        // 포효 이펙트 위치
        
    [Header("제어 변수")]
    public bool bIsAttack;          //  공격 중인지 확인
    public bool bIsSkill;           // 스킬 사용중인지 확인
    [SerializeField]
    LayerMask enemyLayer;           // 에너미 검출을 위한 레이어

    public Image[] coolImg;         // 스킬 쿨타임 이미지들
    public Text[] coolTxt;          // 스킬 쿨타임 텍스트들

    // 최적화를 위한 변수
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
        // 캐릭터가 죽은 상태에서는 공격불가
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
            // 왼쪽 마우스 버튼 누르면 기본 공격(fireRate는 발사 대기 시간)
            if (Input.GetButtonDown("Fire1") && !bIsSkill)
            {
                FoxBaseAttack();
            }
            // 마우스 오른쪽 버튼 누르면 스킬 사용(단, 스킬 사용 가능 상태일때만 발동된다)
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
            //Shooter 내용
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
        animator.SetTrigger("Attack");
    }

    private void TigerBaseAttack(bool isPunching)
    {
        animator.SetBool(hashCombo, isPunching);
    }

    void OnFire()
    {
        // 오브젝트 풀링 방식
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
        if(count != 0) // 펀치공격
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

    void OnHitAttack()  // OnThirdAttack으로 바꿀 예정(애니메이션 이벤트도 변경)
    {
        //Debug.Log("호랑이 세번째 타격");
        thirdEffect.Play();
        punchCollider[1].gameObject.SetActive(true);

    }
    void OnAttackEnd(int count)
    {
        bIsAttack = false;
        if (!animator.GetBool("Combo"))
            playerState.state = PlayerState.State.IDLE;
        if (count != 0) // 펀치공격
        {
            punchCount = 0;
        }
    }
    #endregion

    #region 스킬
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
        // 이펙트 소환
        GameObject Effect = Instantiate(roarData.skillEffect, roarTr.position, roarTr.rotation);
        Destroy(Effect, 1f);

        // 포효 스킬 반경의 에너미 레이어 충돌체를 모두 저장
        Collider[] Cols = Physics.OverlapSphere(transform.position, roarData.f_skillRange, enemyLayer);
        
        // 콜라이더가 에너미들이라면 
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
        // 실제 에너미 데이터로 속도 감소 시켜야함
        // *수정 할 예정*
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

    // 공격, 대화, 스킬 사용시 애니메이터에서 Speed 값이 0이 되지 않기때문에 애니메이션이 계속해서 작동되는 버그가 발생
    // 이를 막기 위한 이동속도 변수 값을 0으로 만들어주는 함수
    void moveStop()
    {
        animator.SetFloat(hashSpeed, 0f);
    }
}
