using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//// [�Ƹ� �����]
// ��ų ���� ĳ���� ������ 3���� ����� ������Ʈ�� Ȱ��ȭ�Ǹ�
// �� ������� ĳ���͸� �߽����� �����Ѵ�.
// ª�� �ð�(�뷫 1~1.5��)�ڿ� �ֺ��� ���� �ִٸ� �ڵ����� �߻�Ǹ�
// �߻�ü�� ���� ����ź ó�� ���� �����Ѵ�
// �̶� ĳ���� �ֺ��� �����ϴ� ������� �߻�ȴ�

public class PlayerAction : MonoBehaviour
{
    ChangeForm Form;
    Animator animator;
    CharacterController controller;

    [Header("����� ������")]
    public Transform FirePos;              // ������� ������ ���� ��ġ
    [SerializeField]
    GameObject FireBall;            // ����� ������Ʈ
    readonly string playerTag = "Player";
    float fireRate = 1f;          // �߻� ��� �ð�
    float nextFire = 0f;

    [Header("����� ����")]
    public GameObject[] FoxFires;   // �����ϴ� ����� �迭 
    public bool canSkill = true;           // ��ų ��� ���� ���� ����
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

        AttackCollider(false);

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
        if(Form.curForm == ChangeForm.FormType.FOX)
        {
            if (Input.GetButtonDown("Fire1") && Time.time > nextFire)
            {
                Fire();
            }
            else if (Input.GetButtonDown("Fire2"))
            {
                if (canSkill)
                {
                    Debug.Log("Hi");
                    foreach (GameObject fire in FoxFires)
                    {
                        fire.SetActive(true);
                        animator.SetTrigger("Fox_FireGuard");
                    }
                }
                canSkill = false;
            }
            CoolDown();
        }
        else if(Form.curForm == ChangeForm.FormType.TIGER)
        {
            if(Input.GetButtonDown("Fire1"))
            {
                isPunching = true;
                AttackCollider(true);
                animator.SetBool(hashCombo, true);
                StartCoroutine(StartPunch());
            }
            else if(Input.GetButtonUp("Fire1"))
            {
                isPunching = false;
                AttackCollider(false);
                animator.SetBool(hashCombo, isPunching);
                punchCount = 0;
            }
        }
        else if(Form.curForm == ChangeForm.FormType.EAGLE)
        {

        }
    }

    private void Fire()
    {
        nextFire = Time.time + fireRate;
        GameObject Fire = Instantiate(FireBall, FirePos.position, FirePos.rotation);
        //Fire.GetComponent<Rigidbody>().velocity = Fire.transform.forward * throwPower;
        animator.SetTrigger("Fox_Attack");
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
