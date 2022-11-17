using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//// [아리 여우불]
// 스킬 사용시 캐릭터 주위에 3개의 여우불 오브젝트가 활성화되며
// 각 여우불은 캐릭터를 중심으로 공전한다.
// 짧은 시간(대략 1~1.5초)뒤에 주변에 적이 있다면 자동으로 발사되며
// 발사체는 적을 유도탄 처럼 따라가 공격한다
// 이때 캐릭터 주변을 공전하는 여우불이 발사된다

public class PlayerAction : MonoBehaviour
{
    ChangeForm Form;
    Animator animator;
    CharacterController controller;

    [Header("여우불 던지기")]
    public Transform FirePos;              // 여우불이 던져질 최초 위치
    [SerializeField]
    GameObject FireBall;            // 여우불 오브젝트
    readonly string playerTag = "Player";
    float fireRate = 1f;          // 발사 대기 시간
    float nextFire = 0f;

    [Header("여우불 가드")]
    public GameObject[] FoxFires;   // 공전하는 여우불 배열 
    public bool canSkill = true;           // 스킬 사용 가능 상태 유무
    float skill1_CoolTime = 10f;
    float skill_CoolTimer;

    [Header("호랑이 펀치")]
    public CapsuleCollider[] punchColliders;
    float lastAttackTime = 0f;  // 마지막으로 공격한 시간
    bool isPunching;            // 공격중 인지 확인
    int punchCount = 0;
    readonly int hashCombo = Animator.StringToHash("Combo");
    public GameObject thirdEffect;

    void Start()
    {
        Form = GetComponent<ChangeForm>();
        controller = GetComponent<CharacterController>();
        FireBall = Resources.Load("Magic fire") as GameObject;
        animator = GetComponent<Animator>();

        AttackCollider(false);

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
        if (GetComponent<PlayerDamage>().isDie)
            return;
        if(Form.curForm == ChangeForm.FormType.FOX)
        {
            if (Input.GetButtonDown("Fire1") && Time.time > nextFire)
            {
                Fire();
            }
            else if (Input.GetButtonDown("Fire2"))
            {
                if (canSkill)
                {
                    Debug.Log("Hi");
                    foreach (GameObject fire in FoxFires)
                    {
                        fire.SetActive(true);
                        animator.SetTrigger("Fox_FireGuard");
                    }
                }
                canSkill = false;
            }
            CoolDown();
        }
        else if(Form.curForm == ChangeForm.FormType.TIGER)
        {
            if(Input.GetButtonDown("Fire1"))
            {
                isPunching = true;
                AttackCollider(true);
                animator.SetBool(hashCombo, true);
                StartCoroutine(StartPunch());
            }
            else if(Input.GetButtonUp("Fire1"))
            {
                isPunching = false;
                AttackCollider(false);
                animator.SetBool(hashCombo, isPunching);
                punchCount = 0;
            }
        }
        else if(Form.curForm == ChangeForm.FormType.EAGLE)
        {

        }
    }

    private void Fire()
    {
        nextFire = Time.time + fireRate;
        GameObject Fire = Instantiate(FireBall, FirePos.position, FirePos.rotation);
        //Fire.GetComponent<Rigidbody>().velocity = Fire.transform.forward * throwPower;
        animator.SetTrigger("Fox_Attack");
    }
    void CoolDown()
    {
        if(!canSkill)
        {
            skill1_CoolTime += Time.deltaTime;
            canSkill = true;
            //if(skill_CoolTimer)
        }
    }

    IEnumerator StartPunch()
    {
        if(Time.time - lastAttackTime > 1f)
        {
            lastAttackTime = Time.time;
            punchCount++;
            if(punchCount == 3)
            {
                punchCount = 0;
            }
            while(isPunching)
            {
                animator.SetBool(hashCombo, true);
                yield return new WaitForSeconds(1f);
            }
        }
    }

}
