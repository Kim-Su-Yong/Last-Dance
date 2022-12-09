using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerDamage : MonoBehaviour
{
    [Header("UI")]
    public Image HpBar;             // ü�¹�
    public Text HpText;             // ü�� �ؽ�Ʈ
    [Header("Data")]
    [SerializeField] float curHp;   // ���� ü��
    public float initHp;            // ���۽� ü��
    public CharacterData charData;  // ������ ��ũ��Ʈ ����
    public bool isDie;              // ��� Ȯ��
    public GameObject hitEffect;    // �ǰ� ����Ʈ
    //public ParticleSystem hitEffect;

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
        // �����Ͱ� ���ٸ� ü�� ���´� �ʱ�ȭ ��
        //if(�����Ͱ� �������� �ʴ´ٸ�)
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
        // ü�� ���� �׽�Ʈ�� ��ũ��Ʈ
        // PŰ ������ �´� �ִϸ��̼� �� ü�� ���� ����
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

        animator.SetTrigger("Hit");     // �ǰ� �ִϸ��̼� ���
        curHp -= 10;                    // ������ ���ݷ°� ĳ������ ���¿� ���� �޴� �������� �޶���
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
        playerState.state = PlayerState.State.DIE;  // ��� ���·� ����
        isDie = true;
       
        animator.SetTrigger("Die");     // ��� �ִϸ��̼� ����
        /*
         * ��� UIâ ����
         */
        //Debug.Log("����Ͽ����ϴ�.");
        yield return new WaitForSeconds(3f);
        SetPlayerVisible(false);
        yield return new WaitForSeconds(5f); // 5�ʵ� �ڵ���Ȱ
        Respawn();
    }
    public void Respawn()   // �� �����Ǿ� �ִ� ����
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

    //    // ���� �Ŵ����� �÷��̾ �׾��ٰ� ����
    //    animator.SetTrigger("Die");     // ��� �ִϸ��̼� ����
    //    // ����� UI ���� �Ǵ� ���� ���� ������ ��ȯ
        
    //    isDie = true;


    //}

    private void OnTriggerEnter(Collider other)
    {
        // ������ ���ݹ����� ü���� �����ϴ� ��ũ��Ʈ��
        //if (other.CompareTag("E_BULLET")) // ���ʹ� ���Ÿ� ����
        //{

        //}
        //if(other.CompareTag("E_MELEE")) // ���ʹ� ���� ����
        //{

        //}
    }
}
