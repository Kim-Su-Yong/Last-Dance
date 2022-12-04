using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : MonoBehaviour
{
    public Transform FirePos;
    public GameObject m_bulletPrefab; // 미사일 프리팹.
    public GameObject m_target; // 도착 지점.
    Animator animator;

    [Header("미사일 기능 관련")]
    public float m_speed = 2; // 미사일 속도.
    [Space(10f)]
    public float m_distanceFromStart = 6.0f; // 시작 지점을 기준으로 얼마나 꺾일지.
    public float m_distanceFromEnd = 3.0f; // 도착 지점을 기준으로 얼마나 꺾일지.
    [Space(10f)]
    public int m_shotCount = 12; // 총 몇 개 발사할건지.
    [Range(0, 1)] public float m_interval = 0.15f;
    public int m_shotCountEveryInterval = 2; // 한번에 몇 개씩 발사할건지.

    [Header("일반공격 관련")]
    private bool CanFire = true;
    public float NextFire = 1.0f; //일반공격 -> 다음 공격과의 딜레이주는 변수
    private float NextFireTimer;
    private float Click_Timer;
    [Header("강공격 관련")]
    private bool Skill_1 = true;
    public float Skill_1_CoolTime = 7.0f; //강공격 쿨타임
    private float Skill_1_Timer;

    ThirdPersonCtrl playerctrl;
    ChangeForm Form;
    void Start()
    {
        playerctrl = GetComponent<ThirdPersonCtrl>();
        Form = GetComponent<ChangeForm>();
        animator = GetComponent<Animator>();
    }
    void Update()
    {
        if(Form.curForm == ChangeForm.FormType.EAGLE)
            Fire();
    }
    private void Fire() //공격 관련 함수
    {
        if(Input.GetMouseButton(0) && playerctrl.isGrounded)
        {
            Click_Timer += Time.deltaTime;
        }
        if (Input.GetMouseButtonUp(0))
        {
            if(Click_Timer < 2.0f && playerctrl.isGrounded)
            {
                if (CanFire)
                {
                    //기본 공격
                    m_distanceFromStart = 1;
                    m_distanceFromEnd = 1;
                    m_shotCount = 1;
                    m_shotCountEveryInterval = 1;
                    animator.SetTrigger("Attack");
                    StartCoroutine(CreateMissile());                    
                }
                CanFire = false;
            }
            else if(Click_Timer > 2.0f && playerctrl.isGrounded)
            {
                if (Skill_1)
                {
                    // Shot
                    m_distanceFromStart = 6;
                    m_distanceFromEnd = 3;
                    m_shotCount = 12;
                    m_shotCountEveryInterval = 2;
                    StartCoroutine(CreateMissile());
                }
                else if(!Skill_1 && CanFire)
                {
                    m_distanceFromStart = 1;
                    m_distanceFromEnd = 1;
                    m_shotCount = 1;
                    m_shotCountEveryInterval = 1;
                    StartCoroutine(CreateMissile());
                }
                Skill_1 = false;
            }
        }
        if (!CanFire)
        {
            NextFireTimer += Time.deltaTime;
            if (NextFireTimer > NextFire)
            {
                NextFireTimer = 0;
                CanFire = true;
            }
        }
        if (!Skill_1)
        {
            Skill_1_Timer += Time.deltaTime;
            if (Skill_1_Timer > Skill_1_CoolTime)
            {
                Skill_1_Timer = 0;
                Skill_1 = true;
            }
        }
    }
    IEnumerator CreateMissile()
    {
        int _shotCount = m_shotCount;
        yield return new WaitForSeconds(0.3f);
        while (_shotCount > 0)
        {
            for (int i = 0; i < m_shotCountEveryInterval; i++)
            {
                if (_shotCount > 0)
                {
                    GameObject missile = Instantiate(m_bulletPrefab, FirePos.position, Quaternion.identity);
                    missile.GetComponent<BezierMissile>().Init(FirePos, m_target.transform, m_speed, m_distanceFromStart, m_distanceFromEnd);

                    _shotCount--;
                }
            }
            Click_Timer = 0;
            yield return new WaitForSeconds(m_interval);
        }
        yield return null;
    }
}