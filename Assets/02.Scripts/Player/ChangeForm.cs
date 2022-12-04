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
    
    public bool canFormChange = true;      // �� ��ȯ ���� ����
    public float charForm_CoolTime = 7.0f;  // �� ��ȯ ��Ÿ��
    public float charForm_Timer;

    private void Start()
    {
        animator = GetComponent<Animator>();
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
        // ����� ����1�� ������ ���� ������ ��ȯ
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (curForm == FormType.FOX) return;
            if (canFormChange)
            {
                /* 
                * �� ��ȯ �ִϸ��̼��̳� ��ƼŬ �־��ֱ�
                */
                Staff.enabled = true;   // ���� ���϶��� �����̸� ���
                /*
                 * �ٸ� ������ ���⸦ ����Ѵٸ� �ڵ� �߰�
                */
                curForm = FormType.FOX;
                animator.SetInteger("Form", 1);

                ChangeFormSprite();
                /*
                 * UI ���� ��Ÿ�� ���� ��� ����
                 * ���� : https://rito15.github.io/posts/unity-memo-cooldown-icon-ui/
                */
            }
            else
            {
                // ����ȯ ��Ÿ�ӽ� UI�� �޽����� ��� �ڵ�
                Debug.Log("��Ÿ�� ���Դϴ�.");
            }
            canFormChange = false;
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
                Staff.enabled = false;   // ȣ���� ���϶��� �����̸� ���X
                /*
                 * �ٸ� ������ ���⸦ ����Ѵٸ� �ڵ� �߰�
                */
                curForm = FormType.TIGER;
                animator.SetInteger("Form", 2);
                ChangeFormSprite();
            }
            else
            {
                Debug.Log("��Ÿ�� ���Դϴ�.");
            }
            canFormChange = false;
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
                Staff.enabled = false;   // ������ ���϶��� �����̸� ���X
                /*
                 * �ٸ� ������ ���⸦ ����Ѵٸ� �ڵ� �߰�
                */
                curForm = FormType.EAGLE;
                animator.SetInteger("Form", 3);
                ChangeFormSprite();
            }
            else
            {
                Debug.Log("��Ÿ�� ���Դϴ�.");
            }
            canFormChange = false;

        }
        CoolDown();
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
