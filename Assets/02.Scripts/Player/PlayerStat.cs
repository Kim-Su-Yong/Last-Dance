using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStat : MonoBehaviour
{
    public static PlayerStat instance;

    public int character_Lv;    // �÷��̾� ����
    public int[] needExp;       // ������ ���� �ʿ� ����ġ
    public int currentEXP;

    public int initHP;          // ���� ü��
    public int maxHP;           // �ִ� ü��

    public int atk;             // ���ݷ�
    public int def;             // ����

    public float speed;         // �̵� �ӵ�

    public float critical;      // ũ��Ƽ�� Ȯ��-�� ���ɼ� ����

    public GameObject LevelUpEffect;    // ���� �� �� ����Ʈ

    AudioSource source;
    public AudioClip levelUpClip;

    public bool isDebug;
    void Awake()
    {
        instance = this;
        // ����� ������ �ҷ�����
        //if(����� �����Ͱ� ���ٸ�)
        source = GetComponent<AudioSource>();
        InitCharacterData();
    }

    // ���ʷ� ������ ���(�����Ͱ� ���� ��� �÷��̾� ���� ����)
    void InitCharacterData()
    {
        character_Lv = 1;
        currentEXP = 0;

        //if (isDebug)
        //    initHP = 1000;
        //else
            initHP = 100;
        maxHP = initHP;

        speed = 6f;
        //currentHP = maxHP;

        // �⺻ ��� ���� ������ ���� ����(�⺻ ��� ���� ��� ����)
        atk = 5;
        def = 5;

        critical = 25f;
    }

    //public void Hit(int _enemyAtk)
    //{
    //    int dmg;

    //    if (def >= _enemyAtk)
    //        dmg = 10;
    //    else
    //        dmg = _enemyAtk - def;

    //    currentHP -= dmg;

    //    if (currentHP <= 0)
    //        Debug.Log("ü�� 0�̸�, ���ӿ���");

    //    SoundManager.instance.Play(dmgSound);

    //    Vector3 vector = this.transform.position;
    //    vector.y += 60;

    //    GameObject clone = Instantiate(prefabs_floating_text, vector, Quaternion.Euler(Vector3.zero));
    //    //clone.GetComponent<FloatingText>().text.text = dmg.ToString();
    //    //clone.GetComponent<FloatingText>().text.color = Color.red;
    //    //clone.GetComponent<FloatingText>().text.fontSize = 25;
    //    //clone.transform.SetParent(parent.transform);
    //    //StopAllCoroutines();
    //    //StartCoroutine(HitCoroutine());
    //}
    //IEnumerator HitCoroutine()
    //{
    //    Color color = GetComponent<SpriteRenderer>().color;
    //    color.a = 0;
    //    GetComponent<SpriteRenderer>().color = color;
    //    yield return new WaitForSeconds(0.1f);
    //    color.a = 1f;
    //    GetComponent<SpriteRenderer>().color = color;
    //    yield return new WaitForSeconds(0.1f);
    //    color.a = 0f;
    //    GetComponent<SpriteRenderer>().color = color;
    //    yield return new WaitForSeconds(0.1f);
    //    color.a = 1f;
    //    GetComponent<SpriteRenderer>().color = color;
    //    yield return new WaitForSeconds(0.1f);
    //    color.a = 0f;
    //    GetComponent<SpriteRenderer>().color = color;
    //    yield return new WaitForSeconds(0.1f);
    //    color.a = 1f;
    //    GetComponent<SpriteRenderer>().color = color;
    //}
    void Update()
    {
        //// ����ġ ȹ�� �׽�Ʈ
        //if (Input.GetKeyDown(KeyCode.R))
        //{
        //    GainExp(24);
        //}
    }

    // ����ġ ȹ�� �Լ�(���� ��ų� ����Ʈ Ŭ����� ���)
    public void GainExp(int newEXP)
    {
        currentEXP += newEXP;       // ���� ����ġ + ȹ�� ����ġ
        if (currentEXP >= needExp[character_Lv])    // ���� ����ġ�� �������� �ʿ����ġ �̻��̸�
        {
            //Debug.Log("���� ��!");

            int overExp = currentEXP - needExp[character_Lv];   // �ʰ� ����ġ ���
            character_Lv++;         // ���� ��
            currentEXP = overExp;   // ���� ����ġ�� �ʰ� ����ġ�� ����(0�̸� 0���� ����)

            // ������ ����Ʈ
            GameObject Effect = Instantiate(LevelUpEffect, transform.position, Quaternion.identity);
            Destroy(Effect, 2f);
            source.PlayOneShot(levelUpClip);

            // ���� ����
            maxHP += 10;        // ������ �� �ִ� ü�� ����
            // UI ������� ����(ü�¹�)
            GetComponent<PlayerDamage>().curHp = maxHP; // ���� ü���� �ִ�ü������ ����(�������� Ǯ�� ȸ��)
            GetComponent<PlayerDamage>().hpUpdate();
            atk++;              // ���ݷ� ����
            def++;              // ���� ����

        }
    }

}
