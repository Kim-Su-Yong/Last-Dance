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

    public GameObject[] Aura;   // 폼 전환시 캐릭터 주변의 아우라

    // 컴포넌트
    public Sprite[] FormIcons;  // 폼 스프라이트 아이콘(여우, 호랑이, 독수리)
    public Image FormImage;     // UI로 표시될 이미지(폼 전환시 변함)
    public MeshRenderer Staff;  // 스태프 모델 메쉬 렌더러
    Animator animator;

    // 변수
    public FormType curForm = FormType.FOX; // 현재 폼(디폴트 폼 : 여우)
    public bool canFormChange = true;       // 폼 전환 가능 상태
    public float charForm_CoolTime = 7.0f;  // 폼 전환 쿨타임
    public float charForm_Timer;

    // UI
    public Image coolImg; //쿨타임 이미지
    public Text coolTxt; //쿨타임 텍스트

    [Header("폼에 따른 스킬 UI")]
    public GameObject[] Fox_Skill_Panel;
    public GameObject[] Tiger_Skill_Panel;
    public GameObject[] Eagle_Skill_Panel;

    // readonly
    readonly int hashForm = Animator.StringToHash("Form");
    private void Start()
    {
        animator = GetComponent<Animator>();

        foreach(var aura in Aura)
        {
            aura.SetActive(false);
        }
        PanelOnOff(curForm);

        coolImg.fillAmount = 0f; //초기화
        coolTxt.enabled = false;
        coolImg.enabled = false;
    }

    void Update()
    {
        if (GetComponent<PlayerDamage>().isDie) return;
        FormChange();
    }

    // 폼에 따른 스킬 Panel 활성화/비활성화
    void PanelOnOff(FormType curForm)
    {
        switch(curForm)
        {
            case FormType.FOX:
                foreach (var panel in Fox_Skill_Panel)
                {
                    panel.SetActive(true);
                }

                foreach (var panel in Tiger_Skill_Panel)
                {
                    panel.SetActive(false);
                }

                foreach (var panel in Eagle_Skill_Panel)
                {
                    panel.SetActive(false);
                }
                break;
            case FormType.TIGER:
                foreach (var panel in Fox_Skill_Panel)
                {
                    panel.SetActive(false);
                }

                foreach (var panel in Tiger_Skill_Panel)
                {
                    panel.SetActive(true);
                }

                foreach (var panel in Eagle_Skill_Panel)
                {
                    panel.SetActive(false);
                }
                break;
            case FormType.EAGLE:
                foreach (var panel in Fox_Skill_Panel)
                {
                    panel.SetActive(false);
                }

                foreach (var panel in Tiger_Skill_Panel)
                {
                    panel.SetActive(false);
                }

                foreach (var panel in Eagle_Skill_Panel)
                {
                    panel.SetActive(true);
                }
                break;
        }
    }

    // 폼 변화에 따른 폼 패널 이미지 변경
    void ChangeFormSprite()
    {
        FormImage.sprite = FormIcons[(int)curForm];
    }

    // 폼 변환 함수
    void FormChange()
    {
        // 상단의 숫자1을 누르면 여우 폼으로 전환
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (curForm == FormType.FOX) return;
            FormChangeAble(FormType.FOX);
        }

        // 상단의 숫자2을 누르면 호랑이 폼으로 전환
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            if (curForm == FormType.TIGER) return;
            FormChangeAble(FormType.TIGER);
        }
        // 상단의 숫자3을 누르면 독수리 폼으로 전환
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            if (curForm == FormType.EAGLE) return;
            FormChangeAble(FormType.EAGLE);
        }
    }

    // 폼 변환이 가능할 시 호출되는 함수
    private void FormChangeAble(FormType form)
    {
        if (canFormChange)
        {
            canFormChange = false;
            curForm = form;
            StartCoroutine(ActiveAura());           // 오라 활성화 코루틴
            /* 
            * 폼 변환 애니메이션이나 파티클 넣어주기
            */
            if (form == FormType.FOX)
                Staff.enabled = true;   // 여우 폼일때는 지팡이를 사용
            else
                Staff.enabled = false;  // 나머지 폼일떄는 지팡이 사용 X

            PanelOnOff(curForm);

            animator.SetInteger(hashForm, (int)form + 1);

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

    IEnumerator ActiveAura()    // 폼에 따른 오라 활성화 코루틴(2초간 활성화)
    {
        Aura[(int)curForm].SetActive(true);
        yield return new WaitForSeconds(2f);
        Aura[(int)curForm].SetActive(false);
    }

    // UI 쿨타임
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
        canFormChange = true;       // 폼 교체 가능 상태로 변경
    }

    public void RespawnToForm()
    {
        curForm = FormType.FOX;
        Staff.enabled = true;
        PanelOnOff(curForm);
        ChangeFormSprite();
    }
}
