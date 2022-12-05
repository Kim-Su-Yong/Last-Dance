using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeForm : MonoBehaviour
{
    public enum FormType
    {
        FOX,
        TIGER,
        EAGLE
    }
    public FormType curForm = FormType.FOX; // 현재 폼(디폴트 폼 : 여우)

    public Sprite[] FormIcons;  // 폼 스프라이트 아이콘(여우, 호랑이, 독수리)
    public Image FormImage;     // UI로 표시될 이미지(폼 전환시 변함)
    public MeshRenderer Staff;  // 스태프 모델 메쉬 렌더러
    Animator animator;
    
    public bool canFormChange = true;      // 폼 전환 가능 상태
    public float charForm_CoolTime = 7.0f;  // 폼 전환 쿨타임
    public float charForm_Timer;

    public Image coolImg; //쿨타임 이미지
    public Text coolTxt; //쿨타임 텍스트


    private void Start()
    {
        animator = GetComponent<Animator>();
        coolImg.fillAmount = 0f; //초기화
        coolTxt.enabled = false;
        coolImg.enabled = false;

    }

    void Update()
    {
        if (GetComponent<PlayerDamage>().isDie) return;
        FormChange();
    }


    void ChangeFormSprite()
    {
        FormImage.sprite = FormIcons[(int)curForm];
    }
    void FormChange()
    {
        // 상단의 숫자1을 누르면 여우 폼으로 전환
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (curForm == FormType.FOX) return;
            if (canFormChange)
            {
                canFormChange = false;
                /* 
                * 폼 변환 애니메이션이나 파티클 넣어주기
                */
                Staff.enabled = true;   // 여우 폼일때는 지팡이를 사용
                /*
                 * 다른 폼에서 무기를 사용한다면 코드 추가
                */
                curForm = FormType.FOX;
                animator.SetInteger("Form", 1);

                ChangeFormSprite();
                //UI 에서 쿨타임 도는 기능 구현
                coolTxt.enabled = true;
                coolImg.enabled = true;
                StartCoroutine(CoolTimeImg(charForm_CoolTime));

            }
            else
            {
                // 폼전환 쿨타임시 UI로 메시지를 띄울 코드
                Debug.Log("쿨타임 중입니다.");
            }
        }

        // 상단의 숫자2을 누르면 호랑이 폼으로 전환
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            if (curForm == FormType.TIGER) return;
            if (canFormChange)
            {
                /* 
               * 폼 변환 애니메이션이나 파티클 넣어주기
               */
                canFormChange = false;
                Staff.enabled = false;   // 호랑이 폼일때는 지팡이를 사용X
                /*
                 * 다른 폼에서 무기를 사용한다면 코드 추가
                */
                curForm = FormType.TIGER;
                animator.SetInteger("Form", 2);
                ChangeFormSprite();
                //UI 에서 쿨타임 도는 기능 구현
                coolTxt.enabled = true;
                coolImg.enabled = true;
                StartCoroutine(CoolTimeImg(charForm_CoolTime));
            }
            else
            {
                Debug.Log("쿨타임 중입니다.");
            }
        }
        // 상단의 숫자3을 누르면 독수리 폼으로 전환
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            if (curForm == FormType.EAGLE) return;
            if (canFormChange)
            {
                /* 
             * 폼 변환 애니메이션이나 파티클 넣어주기
             */
                canFormChange = false;
                Staff.enabled = false;   // 독수리 폼일때는 지팡이를 사용X
                /*
                 * 다른 폼에서 무기를 사용한다면 코드 추가
                */
                curForm = FormType.EAGLE;
                animator.SetInteger("Form", 3);
                ChangeFormSprite();
                coolTxt.enabled = true;
                coolImg.enabled = true;
                StartCoroutine(CoolTimeImg(charForm_CoolTime));
            }
            else
            {
                Debug.Log("쿨타임 중입니다.");
            }
        }
        //CoolDown();
    }
    IEnumerator CoolTimeImg(float cool)
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
        canFormChange = true;
    }

    private void CoolDown()
    {
        if (!canFormChange)
        {
            charForm_Timer += Time.deltaTime;
            if (charForm_Timer > charForm_CoolTime)
            {
                charForm_Timer = 0;
                canFormChange = true;
            }
        }
    }
}
