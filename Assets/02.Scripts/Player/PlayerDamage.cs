using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerDamage : MonoBehaviour
{
    [Header("UI")]
    public Image HpBar;
    public Text HpText;
    [Header("Data")]
    [SerializeField] float curHp;
    public float initHp;
    public CharacterData charData;
    public bool isDie;
    public GameObject hitEffect;

    Renderer[] renderers;

    Animator animator;
    ThirdPersonCtrl controller;
    PlayerAttack attack;

    readonly int hashDie = Animator.StringToHash("Die");

    private void Awake()
    {
        renderers = GetComponentsInChildren<MeshRenderer>();
        animator = GetComponent<Animator>();
        controller = GetComponent<ThirdPersonCtrl>();
        attack = GetComponent<PlayerAttack>();
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
        initHp = charData.maxHp;
        HpBar.fillAmount = 1f;
        HpBar.color = Color.green;
    }

    void Update()
    {
        // 체력 감소 테스트용 스크립트
        // P키 누르면 맞는 애니메이션 및 체력 감소 구현
        if (Input.GetKeyDown(KeyCode.P))
        {
            Hit();
            if (curHp <= 0)
            {
                StartCoroutine("Die");
            }
        }
    }

    private void Hit()
    {
        if (isDie) return;

        animator.SetTrigger("Hit");     // 피격 애니메이션 재생
        curHp -= 10;                    // 몬스터의 공격력과 캐릭터의 방어력에 따라 받는 데미지가 달라짐
        curHp = Mathf.Clamp(curHp, 0, initHp);
        HpBar.fillAmount = (float)curHp / initHp;
        HpText.text = curHp.ToString() + " / " + initHp.ToString();
        GameObject hitEff = Instantiate(hitEffect, transform.position + new Vector3(0, 1.5f, 0), Quaternion.identity);
        Destroy(hitEff, 1.5f);

        if (HpBar.fillAmount <= 0.3f)
            HpBar.color = Color.red;
        else if (HpBar.fillAmount <= 0.5f)
            HpBar.color = Color.yellow;
    }

    IEnumerator Die()
    {
        controller.enabled = false;
        attack.enabled = false;
        isDie = true;
       
        animator.SetTrigger("Die");     // 사망 애니메이션 실행
        /*
         * 사망 UI창 띄우기
         */
        Debug.Log("사망하였습니다.");
        yield return new WaitForSeconds(3f);
        SetPlayerVisible(false);
        yield return new WaitForSeconds(5f); // 5초뒤 자동부활
        Respawn();
    }
    public void Respawn()
    {
        Transform SpawnPoint = GameObject.Find("SpawnManager").
            transform.GetChild(0).GetComponent<Transform>();
        transform.position = SpawnPoint.position;
        curHp = initHp;
        HpBar.color = Color.green;
        HpBar.fillAmount = (float)curHp / initHp;
        HpText.text = curHp.ToString() + " / " + initHp.ToString();
        SetPlayerVisible(true);
        controller.enabled = true;
        attack.enabled = true;
        isDie = false;
    }

    void SetPlayerVisible(bool visible)
    {
        foreach (var renderer in renderers)
        {
            renderer.enabled = visible;
        }
    }

    //void Die()
    //{
    //    if (isDie) return;

    //    // 게임 매니저에 플레이어가 죽었다고 전달
    //    animator.SetTrigger("Die");     // 사망 애니메이션 실행
    //    // 사망시 UI 띄우기 또는 게임 오버 씬으로 전환
        
    //    isDie = true;


    //}

    private void OnTriggerEnter(Collider other)
    {
        // 적에게 공격받으면 체력이 감소하는 스크립트들
        //if (other.CompareTag("E_BULLET")) // 에너미 원거리 공격
        //{

        //}
        //if(other.CompareTag("E_MELEE")) // 에너미 근접 공격
        //{

        //}
    }
}
