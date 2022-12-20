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
    public FormType curForm = FormType.FOX; // ���� ��(����Ʈ �� : ����)

    public Sprite[] FormIcons;  // �� ��������Ʈ ������(����, ȣ����, ������)
    public Image FormImage;     // UI�� ǥ�õ� �̹���(�� ��ȯ�� ����)
    public MeshRenderer Staff;  // ������ �� �޽� ������
    Animator animator;

    public bool canFormChange = true;       // �� ��ȯ ���� ����
    public float charForm_CoolTime = 7.0f;  // �� ��ȯ ��Ÿ��
    public float charForm_Timer;

    public Image coolImg; //��Ÿ�� �̹���
    public Text coolTxt; //��Ÿ�� �ؽ�Ʈ

    [Header("���� ���� ��ų UI")]
    public GameObject[] Fox_Skill_Panel;
    public GameObject[] Tiger_Skill_Panel;
    public GameObject[] Eagle_Skill_Panel;

    private void Start()
    {
        animator = GetComponent<Animator>();

        PanelOnOff(curForm);

        coolImg.fillAmount = 0f; //�ʱ�ȭ
        coolTxt.enabled = false;
        coolImg.enabled = false;
    }

    void Update()
    {
        if (GetComponent<PlayerDamage>().isDie) return;
        FormChange();
    }

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

    void ChangeFormSprite()
    {
        FormImage.sprite = FormIcons[(int)curForm];
    }
    void FormChange()
    {
        // ����� ����1�� ������ ���� ������ ��ȯ
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (curForm == FormType.FOX) return;
            if (canFormChange)
            {
                canFormChange = false;
                /* 
                * �� ��ȯ �ִϸ��̼��̳� ��ƼŬ �־��ֱ�
                */
                Staff.enabled = true;   // ���� ���϶��� �����̸� ���
                curForm = FormType.FOX;
                PanelOnOff(curForm);
                
                animator.SetInteger("Form", 1);

                ChangeFormSprite();
                //UI ���� ��Ÿ�� ���� ��� ����
                coolTxt.enabled = true;
                coolImg.enabled = true;
                StartCoroutine(CoolTimeImg(charForm_CoolTime));
            }
            else
            {
                // ����ȯ ��Ÿ�ӽ� UI�� �޽����� ��� �ڵ�
                Debug.Log("��Ÿ�� ���Դϴ�.");
            }
        }

        // ����� ����2�� ������ ȣ���� ������ ��ȯ
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            if (curForm == FormType.TIGER) return;
            if (canFormChange)
            {
                /* 
               * �� ��ȯ �ִϸ��̼��̳� ��ƼŬ �־��ֱ�
               */
                canFormChange = false;
                Staff.enabled = false;   // ȣ���� ���϶��� �����̸� ���X

                curForm = FormType.TIGER;
                PanelOnOff(curForm);
                animator.SetInteger("Form", 2);
                ChangeFormSprite();
                //UI ���� ��Ÿ�� ���� ��� ����
                coolTxt.enabled = true;
                coolImg.enabled = true;
                StartCoroutine(CoolTimeImg(charForm_CoolTime));
            }
            else
            {
                Debug.Log("��Ÿ�� ���Դϴ�.");
            }
        }
        // ����� ����3�� ������ ������ ������ ��ȯ
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            if (curForm == FormType.EAGLE) return;
            if (canFormChange)
            {
                /* 
             * �� ��ȯ �ִϸ��̼��̳� ��ƼŬ �־��ֱ�
             */
                canFormChange = false;
                Staff.enabled = false;   // ������ ���϶��� �����̸� ���X
                /*
                 * �ٸ� ������ ���⸦ ����Ѵٸ� �ڵ� �߰�
                */
                curForm = FormType.EAGLE;
                PanelOnOff(curForm);
                animator.SetInteger("Form", 3);
                ChangeFormSprite();
                coolTxt.enabled = true;
                coolImg.enabled = true;
                StartCoroutine(CoolTimeImg(charForm_CoolTime));
            }
            else
            {
                Debug.Log("��Ÿ�� ���Դϴ�.");
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
}
