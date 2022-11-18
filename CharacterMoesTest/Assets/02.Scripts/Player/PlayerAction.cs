using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAction : MonoBehaviour
{
    ChangeForm Form;
    Animator animator;
    CharacterController controller;

    [Header("���̾ �߻�")]
    public Transform FirePos;       // ���̾ ������ �߻� ��ġ
    [SerializeField]
    GameObject FireBall;            // ���̾ ������Ʈ
    readonly string playerTag = "Player";
    float fireRate = 1f;          // �߻� ��� �ð�
    float nextFire = 0f;

    [Header("����� ����")]
    public GameObject[] FoxFires;   // �����ϴ� ����� �迭 
    public bool canSkill = true;    // ��ų ��� ���� ���� ����
    float skill1_CoolTime = 10f;
    float skill_CoolTimer;

    [Header("ȣ���� ��ġ")]
    public CapsuleCollider[] punchColliders;
    float lastAttackTime = 0f;  // ���������� ������ �ð�
    bool isPunching;            // ������ ���� Ȯ��
    int punchCount = 0;
    readonly int hashCombo = Animator.StringToHash("Combo");
    public GameObject thirdEffect;

    void Start()
    {
        Form = GetComponent<ChangeForm>();
        controller = GetComponent<CharacterController>();
        FireBall = Resources.Load("Magic fire") as GameObject;
        animator = GetComponent<Animator>();

        AttackCollider(false);                  // ȣ���� �����ϴ� �ð� �ܿ��� �ݶ��̴��� �����־�� ��
    }

    private void AttackCollider(bool isActive)
    {
        foreach (var col in punchColliders)
        {
            col.enabled = isActive;
        }
    }

    void Update()
    {
        // ĳ���Ͱ� ���� ���¿����� ���ݺҰ�
        if (GetComponent<PlayerDamage>().isDie)
            return;
        if(Form.curForm == ChangeForm.FormType.FOX)
        {
            // ���� ���콺 ��ư ������ �⺻ ����(fireRate�� �߻� ��� �ð�)
            if (Input.GetButtonDown("Fire1") && Time.time > nextFire)   
            {
                StartCoroutine(FoxBaseAttack());
            }
            // ���콺 ������ ��ư ������ ��ų ���(��, ��ų ��� ���� �����϶��� �ߵ��ȴ�)
            else if (Input.GetButtonDown("Fire2"))
            {
                //FoxSkill_1();
                StartCoroutine(FoxSkill_1());
            }
            CoolDown(); // ��ų ��Ÿ�� ���� �ڵ� �߰� �ʿ�
        }
        else if(Form.curForm == ChangeForm.FormType.TIGER)
        {
            if(Input.GetButtonDown("Fire1"))
            {
                TigerBaseAttack(true);
            }
            else if(Input.GetButtonUp("Fire1"))
            {
                TigerBaseAttack(false);
                //isPunching = false;
                //AttackCollider(false);
                //animator.SetBool(hashCombo, isPunching);
                punchCount = 0;
            }
        }
        else if(Form.curForm == ChangeForm.FormType.EAGLE)
        {

        }
    }

    private void TigerBaseAttack(bool isPunching)
    {
        //isPunching = true;
        AttackCollider(isPunching);
        animator.SetBool(hashCombo, isPunching);
        StartCoroutine(StartPunch());
    }

    IEnumerator FoxSkill_1()
    {
        if (canSkill)
        {
            //animator.SetTrigger("Fox_FireGuard");
            animator.SetInteger("SkillState", 1);
            yield return new WaitForSeconds(0.5f);
            foreach (GameObject fire in FoxFires)
            {
                fire.SetActive(true);                
            }
        }
        canSkill = false;
        animator.SetInteger("SkillState", 0);
    }

    IEnumerator FoxBaseAttack()
    {
        nextFire = Time.time + fireRate;
        animator.SetTrigger("Fox_Attack");
        yield return new WaitForSeconds(0.4f);
        GameObject Fire = Instantiate(FireBall, FirePos.position, FirePos.rotation);
        
    }

    void CoolDown()
    {
        if(!canSkill)
        {
            skill1_CoolTime += Time.deltaTime;
            canSkill = true;
            //if(skill_CoolTimer)
        }
    }

    IEnumerator StartPunch()
    {
        if(Time.time - lastAttackTime > 1f)
        {
            lastAttackTime = Time.time;
            punchCount++;
            if(punchCount == 3)
            {
                punchCount = 0;
            }
            while(isPunching)
            {
                animator.SetBool(hashCombo, true);
                yield return new WaitForSeconds(1f);
            }
        }
    }

}
