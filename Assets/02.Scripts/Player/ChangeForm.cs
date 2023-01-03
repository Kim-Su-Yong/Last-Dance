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

    public GameObject[] Aura;   // �� ��ȯ�� ĳ���� �ֺ��� �ƿ��

    // ������Ʈ
    public Sprite[] FormIcons;  // �� ��������Ʈ ������(����, ȣ����, ������)
    public Image FormImage;     // UI�� ǥ�õ� �̹���(�� ��ȯ�� ����)
    public MeshRenderer Staff;  // ������ �� �޽� ������
    Animator animator;

    // ����
    public FormType curForm = FormType.FOX; // ���� ��(����Ʈ �� : ����)
    public bool canFormChange = true;       // �� ��ȯ ���� ����
    public float charForm_CoolTime = 7.0f;  // �� ��ȯ ��Ÿ��
    public float charForm_Timer;

    // UI
    public Image coolImg; //��Ÿ�� �̹���
    public Text coolTxt; //��Ÿ�� �ؽ�Ʈ

    [Header("���� ���� ��ų UI")]
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

        coolImg.fillAmount = 0f; //�ʱ�ȭ
        coolTxt.enabled = false;
        coolImg.enabled = false;
    }

    void Update()
    {
        if (GetComponent<PlayerDamage>().isDie) return;
        FormChange();
    }

    // ���� ���� ��ų Panel Ȱ��ȭ/��Ȱ��ȭ
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

    // �� ��ȭ�� ���� �� �г� �̹��� ����
    void ChangeFormSprite()
    {
        FormImage.sprite = FormIcons[(int)curForm];
    }

    // �� ��ȯ �Լ�
    void FormChange()
    {
        // ����� ����1�� ������ ���� ������ ��ȯ
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (curForm == FormType.FOX) return;
            FormChangeAble(FormType.FOX);
        }

        // ����� ����2�� ������ ȣ���� ������ ��ȯ
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            if (curForm == FormType.TIGER) return;
            FormChangeAble(FormType.TIGER);
        }
        // ����� ����3�� ������ ������ ������ ��ȯ
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            if (curForm == FormType.EAGLE) return;
            FormChangeAble(FormType.EAGLE);
        }
    }

    // �� ��ȯ�� ������ �� ȣ��Ǵ� �Լ�
    private void FormChangeAble(FormType form)
    {
        if (canFormChange)
        {
            canFormChange = false;
            curForm = form;
            StartCoroutine(ActiveAura());           // ���� Ȱ��ȭ �ڷ�ƾ
            /* 
            * �� ��ȯ �ִϸ��̼��̳� ��ƼŬ �־��ֱ�
            */
            if (form == FormType.FOX)
                Staff.enabled = true;   // ���� ���϶��� �����̸� ���
            else
                Staff.enabled = false;  // ������ ���ϋ��� ������ ��� X

            PanelOnOff(curForm);

            animator.SetInteger(hashForm, (int)form + 1);

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

    IEnumerator ActiveAura()    // ���� ���� ���� Ȱ��ȭ �ڷ�ƾ(2�ʰ� Ȱ��ȭ)
    {
        Aura[(int)curForm].SetActive(true);
        yield return new WaitForSeconds(2f);
        Aura[(int)curForm].SetActive(false);
    }

    // UI ��Ÿ��
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
        canFormChange = true;       // �� ��ü ���� ���·� ����
    }

    public void RespawnToForm()
    {
        curForm = FormType.FOX;
        Staff.enabled = true;
        PanelOnOff(curForm);
        ChangeFormSprite();
    }
}
