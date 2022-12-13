using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerDamage : MonoBehaviour
{
    [Header("UI")]
    public Image HpBar;             // 체력바
    public Text HpText;             // 체력 텍스트

    [Header("Data")]
    [SerializeField] float curHp;   // 현재 체력
    public float initHp;            // 시작시 체력
    public bool isDie;              // 사망 확인
    public GameObject hitEffect;    // 피격 이펙트

    readonly string M_AttackTag = "M_ATTACK";   // 몬스터 공격 콜라이더 태그

    Renderer[] renderers;

    Animator animator;
    ThirdPersonCtrl controller;
    PlayerAttack attack;
    PlayerState playerState;

    readonly int hashDie = Animator.StringToHash("Die");

    private void Awake()
    {
        renderers = GetComponentsInChildren<MeshRenderer>();
        animator = GetComponent<Animator>();
        controller = GetComponent<ThirdPersonCtrl>();
        attack = GetComponent<PlayerAttack>();
        playerState = GetComponent<PlayerState>();
    }

    void Start()
    {
        // 데이터가 없다면 체력 상태는 초기화 됨
        //if(데이터가 존재하지 않는다면)
        InitCharacterData();
        curHp = initHp;
        HpText.text = curHp.ToString() + " / " + initHp.ToString();
    }

    void InitCharacterData()
    {
        initHp = 1000;
        HpBar.fillAmount = 1f;
        HpBar.color = Color.green;
    }

    void Update()
    {
   
    }

    // 피격 코루틴
    IEnumerator Hit(GameObject Enemy)
    {
        animator.SetTrigger("Hit");     // 피격 애니메이션 재생
        animator.SetFloat("Speed", 0f);

        curHp -= Enemy.GetComponent<MonsterAI>().damage;
        //curHp -= enemy.GetComponent<MonsterAI>().damage;
        //curHp -= 10;                    // 몬스터의 공격력과 캐릭터의 방어력에 따라 받는 데미지가 달라짐
        curHp = Mathf.Clamp(curHp, 0, initHp);
        HpBar.fillAmount = (float)curHp / initHp;
        HpText.text = curHp.ToString() + " / " + initHp.ToString();

        playerState.state = PlayerState.State.HIT;
        
        GameObject hitEff = Instantiate(hitEffect, transform.position + new Vector3(0, 1.5f, 0), Quaternion.identity);
        Destroy(hitEff, 1.5f);

        if (HpBar.fillAmount <= 0.3f)
            HpBar.color = Color.red;
        else if (HpBar.fillAmount <= 0.5f)
            HpBar.color = Color.yellow;

        yield return new WaitForSeconds(1f);
        playerState.state = PlayerState.State.IDLE;
        //hitCollider.enabled = true;
    }

    IEnumerator Die()
    {
        playerState.state = PlayerState.State.DIE;  // 사망 상태로 변경
        isDie = true;
       
        animator.SetTrigger("Die");     // 사망 애니메이션 실행
        /*
         * 사망 UI창 띄우기
         */
        //Debug.Log("사망하였습니다.");
        yield return new WaitForSeconds(3f);
        SetPlayerVisible(false);
        yield return new WaitForSeconds(5f); // 5초뒤 자동부활
        Respawn();
    }
    public void Respawn()   // 덜 구현되어 있는 상태
    {
        Transform SpawnPoint = GameObject.Find("SpawnManager").
            transform.GetChild(0).GetComponent<Transform>();
        transform.position = SpawnPoint.position;
        curHp = initHp;
        HpBar.color = Color.green;
        HpBar.fillAmount = (float)curHp / initHp;
        HpText.text = curHp.ToString() + " / " + initHp.ToString();
        SetPlayerVisible(true);
        playerState.state = PlayerState.State.IDLE;
        //controller.enabled = true;
        //attack.enabled = true;
        isDie = false;
    }

    void SetPlayerVisible(bool visible)
    {
        foreach (var renderer in renderers)
        {
            renderer.enabled = visible;
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag(M_AttackTag))
        {
            // 죽었거나 피격 상태 라면 실행하지 않음
            if (isDie ||
            playerState.state == PlayerState.State.DIE ||
            playerState.state == PlayerState.State.HIT)
                return;
            GameObject EnemyInfo = other.GetComponentInParent<MonsterAI>().gameObject;
            StartCoroutine(Hit(EnemyInfo));
            if (curHp <= 0)
            {
                StartCoroutine("Die");
            }
        }
    }

    void OnHit()
    {
        animator.SetFloat("Speed", 0f);
    }
}
