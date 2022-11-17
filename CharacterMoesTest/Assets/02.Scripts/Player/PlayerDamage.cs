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
        // �����Ͱ� ���ٸ� ü�� ���´� �ʱ�ȭ ��
        //if(�����Ͱ� �������� �ʴ´ٸ�)
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
        // ü�� ���� �׽�Ʈ�� ��ũ��Ʈ
        // PŰ ������ �´� �ִϸ��̼� �� ü�� ���� ����
        if (Input.GetKeyDown(KeyCode.P))
        {
            animator.SetTrigger("Hit");     // �ǰ� �ִϸ��̼� ���
            Hit();

            if (curHp <= 0)
            {
                Die();
            }
        }
    }

    private void Hit()
    {
        curHp -= 10;                    // ������ ���ݷ°� ĳ������ ���¿� ���� �޴� �������� �޶���
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
        // ���� �Ŵ����� �÷��̾ �׾��ٰ� ����
        animator.SetTrigger("Die");     // ��� �ִϸ��̼� ����
        // ����� UI ���� �Ǵ� ���� ���� ������ ��ȯ
        StopAllCoroutines();
        isDie = true;


    }

    private void OnTriggerEnter(Collider other)
    {
        // ������ ���ݹ����� ü���� �����ϴ� ��ũ��Ʈ��
        if (other.CompareTag("E_BULLET")) // ���ʹ� ���Ÿ� ����
        {

        }
        if(other.CompareTag("E_MELEE")) // ���ʹ� ���� ����
        {

        }
    }
}
