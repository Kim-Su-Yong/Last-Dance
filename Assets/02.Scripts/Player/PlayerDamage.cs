using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class PlayerDamage : MonoBehaviour
{
    [Header("UI")]
    public Image HpBar;                 // ü�¹�
    public Text HpText;                 // ü�� �ؽ�Ʈ

    [Header("Data")]
    public int curHp;                 // ���� ü��
    public bool isDie;                  // ��� Ȯ��
    public GameObject hitEffect;        // �ǰ� ����Ʈ
    public GameObject damageUIPrefab;   // ������ UI

    readonly string M_AttackTag = "M_ATTACK";   // ���� ���� �ݶ��̴� �±�

    Animator animator;
    ThirdPersonCtrl controller;
    PlayerAttack attack;
    PlayerState playerState;
    PlayerStat playerStat;

    readonly int hashDie = Animator.StringToHash("Die");
    readonly int hashHit = Animator.StringToHash("Hit");
    readonly int hashSpeed = Animator.StringToHash("Speed");

    //public GameObject deathPanel;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        controller = GetComponent<ThirdPersonCtrl>();
        attack = GetComponent<PlayerAttack>();
        playerState = GetComponent<PlayerState>();
        playerStat = GetComponent<PlayerStat>();

        damageUIPrefab = Resources.Load<GameObject>("Effects/DamagePopUp");
    }

    void Start()
    {
        // �����Ͱ� ���ٸ� ü�� ���´� �ʱ�ȭ ��
        //if(�����Ͱ� �������� �ʴ´ٸ�)
        
        LoadCharacterData();
        hpUpdate();
    }

    // ĳ���� �ʱ� ������(������ ����Ȱ��� ���� ��� ��)
    void LoadCharacterData()
    {
        curHp = playerStat.maxHP;
    }

    // ü�� ������Ʈ �Լ�(ü�°��� ����� �� ���� ȣ���ؾ���
    public void hpUpdate()
    {
        HpBar.fillAmount = (float)curHp / playerStat.maxHP;                   // ü�¹� �̹��� ����
        HpText.text = curHp.ToString() + " / " + playerStat.maxHP.ToString(); // ü�¹� �ؽ�Ʈ ����

        // ü���� 30�������� ��� ������
        if (HpBar.fillAmount <= 0.3f)
            HpBar.color = Color.red;
        //ü���� 50�������� ��� �����
        else if (HpBar.fillAmount <= 0.5f)
            HpBar.color = Color.yellow;
        else HpBar.color = Color.green;
    }

    void Update()
    {
        // ü�� ���� �׽�Ʈ �� ȸ���� ���� �׽�Ʈ�� �Լ�
        if (Input.GetKeyDown(KeyCode.P))
        {
            curHp -= 10;
            hpUpdate();
            if (curHp <= 0)
                StartCoroutine(Die());
        }
            
    }

    // �ǰ� �ڷ�ƾ
    IEnumerator Hit(GameObject Enemy)
    {
        // Move, Idle�� ��쿡�� �ǰ� ��� �߻��ϵ��� ���� ����
        // ���� ĳ���Ͱ� �������� �ƴ϶�� �ִϸ��̼� ����
        if(playerState.state != PlayerState.State.ATTACK)
            animator.SetTrigger(hashHit);     // �ǰ� �ִϸ��̼� ���
        animator.SetFloat(hashSpeed, 0f); // �ǰ� �ִϸ��̼� ����� �������� �ʵ��� ����

        // ���ʹ�AI�κ��� ������ ���� �޾ƿ�(+ �����ϰ� 0~9���� ������ �߰�)
        int _damage = (int)(Enemy.GetComponent<MonsterAI>().damage + Random.Range(0f, 9f));
        curHp -= _damage;           // ���� ü���� ������ ��ŭ ����
        ShowDamageEffect(_damage);  // ������ ����Ʈ ���
        // + ���� : ���̳����� ������ ���� ������ 0~9�� ���� ���� �߰��Ǵ� ������ �����߽��ϴ�!

        // ���� ü�°��� 0 ~ �ʱ� ü��(�Ƹ� �ִ�ü������ ����� ����)������ ���� �������� ����
        curHp = Mathf.Clamp(curHp, 0, playerStat.maxHP);
        hpUpdate();

        playerState.state = PlayerState.State.HIT;  // �÷��̾� ����->�ǰݻ��� �� ����
        
        // �ǰ� ����Ʈ ����(���Ŀ� �ǰݹ��� ������ ���� �װ����� �ǰ� ����Ʈ �����ǵ��� ����)
        GameObject hitEff = Instantiate(hitEffect, transform.position + new Vector3(0, 1.5f, 0), Quaternion.identity);
        Destroy(hitEff, 1.5f);
        
        // �ǰ��� 1�ʵ� �÷��̾� ����->Idle �� ����
        yield return new WaitForSeconds(1f);
        playerState.state = PlayerState.State.IDLE;
    }

    IEnumerator Die()
    {
        playerState.state = PlayerState.State.DIE;  // ��� ���·� ����
        isDie = true;
       
        animator.SetTrigger("Die");     // ��� �ִϸ��̼� ����
        GetComponent<CharacterController>().enabled = false;    // �浹���� ���Ÿ� ���� ĳ���� ��Ʈ�ѷ� ��Ȱ��ȭ

        // ����� UI �߰��ؾ���(����� ��ư, ����ߴٸ� �˸��� UI��)
        yield return new WaitForSeconds(2f);    
        //deathPanel.SetActive(true);
        yield return new WaitForSeconds(3f);
        //deathPanel.SetActive(false);
        Respawn();
    }
    public void Respawn()   // �� �����Ǿ� �ִ� ����
    {
        Transform SpawnPoint = GameObject.Find("SpawnManager").
            transform.GetChild(0).GetComponent<Transform>();
        transform.position = SpawnPoint.position;
        curHp = playerStat.maxHP;
        HpBar.color = Color.green;
        hpUpdate();
        playerState.state = PlayerState.State.IDLE;
        GetComponent<CharacterController>().enabled = true;
        GetComponent<ChangeForm>().curForm = ChangeForm.FormType.FOX;
        GetComponent<ChangeForm>().Staff.enabled = true;
        isDie = false;

        gameObject.SetActive(false);
        gameObject.SetActive(true);
    }

    private void OnTriggerEnter(Collider other)
    {
        // ���Ϳ��� ������ �¾Ҵٸ�
        if(other.CompareTag(M_AttackTag))
        {
            // �׾��ų� �ǰ� ���� ��� �������� ����
            if (isDie ||
            playerState.state == PlayerState.State.DIE ||
            playerState.state == PlayerState.State.HIT)
                return;
            // ���ʹ� ������ �ѱ�(������ ����ŭ ü���� �޾ƾ��ϱ� ����)
            GameObject EnemyInfo = other.GetComponentInParent<MonsterAI>().gameObject;
            StartCoroutine(Hit(EnemyInfo));
            // ü���� 0���ϰ� �Ǹ� Die�ڷ�ƾ ����
            if (curHp <= 0)
            {
                StartCoroutine("Die");
            }
        }
    }

    void ShowDamageEffect(int _damage) // �÷��̾� ��ġ�� transform�� �ǹǷ� ��ġ ������ ���� �ʿ� ����
    {
        Vector3 MonsterHeader = new Vector3(transform.position.x + Random.Range(-0.5f, 0.5f),
                                            transform.position.y + 1.8f,
                                            transform.position.z + Random.Range(-0.5f, 0.5f));
        GameObject Effect_M_DamageAmount = Instantiate(damageUIPrefab,
                                           MonsterHeader,
                                           Quaternion.identity,
                                           transform);
        Effect_M_DamageAmount.GetComponent<TextMeshPro>().text = _damage.ToString();
        Effect_M_DamageAmount.GetComponent<TextMeshPro>().color = new Color(150f, 0f, 255f);
    }

    void OnHit()
    {
        animator.SetFloat(hashSpeed, 0f);
    }

    // ü�� ȸ�� �Լ�(���� ���, �� ��ų ���)
    public void RestoreHp(int newHp)
    {
        // �׾����� �Լ� ����
        if (playerState.state == PlayerState.State.DIE)
            return;
        curHp += newHp;     // newHP��ŭ ü�� ȸ��
        // ü�µ� ȸ���� �ִ�ü�º��� ū ��� ����
        if (curHp > playerStat.maxHP)
            curHp = playerStat.maxHP;
        hpUpdate();
    }


}
