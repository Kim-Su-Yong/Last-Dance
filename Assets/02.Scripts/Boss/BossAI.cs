using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAI : MonoBehaviour
{
    public float maxHp = 200f; // 최대 체력
    public float curHp = 0f; // 현재 체력

    public float damage = 20f;

    public bool isDie = false;
    public bool isAttack = false;
    public bool isDamaged = false;
    public bool isIdle = false;

    public float beforeHp = 0f;

    public Animator animator;

    public enum State
    {
        STAY,
        TRACE,
        ATTACK,
        DIE
    }

    public State state = State.STAY;

    private float countTime = 0f;
    private WaitForSeconds ws;

    void Awake()
    {
        var player = GameObject.FindGameObjectWithTag("Player");
        animator = GetComponentInChildren<Animator>();
        ws = new WaitForSeconds(0.3f);
    }

    void Start()
    {
        curHp = maxHp;
    }

    void Update()
    {
        StartCoroutine(Action());
    }

    IEnumerator Action()
    {
        while (!isDie)
        {
            yield return ws;
            switch (state)
            {
                case State.STAY:
                    isAttack = false;
                    break;
                case State.TRACE:
                    isAttack = false;
                    break;
                case State.ATTACK:
                    if (isAttack == false)
                    {
                        isAttack = true;
                    }
                    break;
                case State.DIE:
                    isAttack = false;
                    animator.SetBool("Dead", true);
                    isDie = true;

                    // 사망시 더이상 코루틴을 수행하지 않음
                    StopAllCoroutines();
                    break;
            }
        }
    }

    public void HpUpdate()
    {
        //// 선형 보간 함수로 데미지 부드럽게 줄어들도록 만듦
        ////Hp_Bar.fillAmount = Mathf.Lerp(Hp_Bar.fillAmount, M_HP / M_MaxHP, Time.deltaTime * 100f);
        //Hp_Bar.fillAmount = M_HP / M_MaxHP;
        //Hp_Bar_Before.fillAmount = _beforeHP / M_MaxHP;

        ////Debug.Log("Hp_Bar.fillAmount 값 : " + Hp_Bar.fillAmount * M_MaxHP);
        //Hp_Text.text = ((int)M_HP).ToString() + " / " + ((int)M_MaxHP).ToString();

        // HP 값의 범위 지정

        if (curHp <= 0)
        {
            state = State.DIE;
            //Hp_Canvas.enabled = false;
        }
    }
}
