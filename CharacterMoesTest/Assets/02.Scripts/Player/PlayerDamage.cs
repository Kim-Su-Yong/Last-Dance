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
    public float maxHp;
    public CharacterData charData;
    public bool isDie;
    public GameObject hitEffect;

    Animator animator;    

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void Start()
    {
        // 데이터가 없다면 체력 상태는 초기화 됨
        //if(데이터가 존재하지 않는다면)
        InitCharacterData();
        curHp = maxHp;
        HpText.text = curHp.ToString() + " / " + maxHp.ToString();
    }

    void InitCharacterData()
    {
        maxHp = charData.maxHp;
        HpBar.fillAmount = 1f;
        HpBar.color = Color.green;
    }

    void Update()
    {
        // 체력 감소 테스트용 스크립트
        // P키 누르면 맞는 애니메이션 및 체력 감소 구현
        if (Input.GetKeyDown(KeyCode.P))
        {
            animator.SetTrigger("Hit");     // 피격 애니메이션 재생
            Hit();

            if (curHp <= 0)
            {
                Die();
            }
        }
    }

    private void Hit()
    {
        curHp -= 10;                    // 몬스터의 공격력과 캐릭터의 방어력에 따라 받는 데미지가 달라짐
        curHp = Mathf.Clamp(curHp, 0, maxHp);
        HpBar.fillAmount = (float)curHp / maxHp;
        HpText.text = curHp.ToString() + " / " + maxHp.ToString();
        GameObject hitEff = Instantiate(hitEffect, transform.position + new Vector3(0, 1.5f, 0), Quaternion.identity);
        Destroy(hitEff, 1.5f);

        if (HpBar.fillAmount <= 0.3f)
            HpBar.color = Color.red;
        if (HpBar.fillAmount <= 0.5f)
            HpBar.color = Color.yellow;
    }

    void Die()
    {
        // 게임 매니저에 플레이어가 죽었다고 전달
        animator.SetTrigger("Die");     // 사망 애니메이션 실행
        // 사망시 UI 띄우기 또는 게임 오버 씬으로 전환
        StopAllCoroutines();
        isDie = true;


    }

    private void OnTriggerEnter(Collider other)
    {
        // 적에게 공격받으면 체력이 감소하는 스크립트들
        if (other.CompareTag("E_BULLET")) // 에너미 원거리 공격
        {

        }
        if(other.CompareTag("E_MELEE")) // 에너미 근접 공격
        {

        }
    }
}
