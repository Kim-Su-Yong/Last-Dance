using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStat : MonoBehaviour
{
    public static PlayerStat instance;

    public int character_Lv;
    public int[] needExp; //���� ����
    public int currentEXP;

    public int initHP;      // ���� ü��
    //public int currentHP;   // ���� ü��
    public int maxHP;              // �ִ� ü��

    public int atk;         // ���ݷ�
    public int def;         // ����

    public float critical;  // ũ��Ƽ�� Ȯ��

    public GameObject LevelUpEffect;    // ���� �� �� ����Ʈ
    void Awake()
    {
        instance = this;
        // ����� ������ �ҷ�����
        //if(����� �����Ͱ� ���ٸ�)
        InitCharacterData();
    }

    // ���ʷ� ������ ���(�����Ͱ� ���� ��� �÷��̾� ���� ����)
    void InitCharacterData()
    {
        character_Lv = 1;
        currentEXP = 0;

        initHP = 100;
        maxHP = initHP;
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
        if (Input.GetKeyDown(KeyCode.R))
        {
            GainExp(24);
        }
    }
    void GainExp(int newEXP)
    {
        currentEXP += newEXP;
        if (currentEXP >= needExp[character_Lv])
        {
            Debug.Log("���� ��!");

            int overExp = currentEXP - needExp[character_Lv];
            character_Lv++;
            currentEXP = overExp;

            GameObject Effect = Instantiate(LevelUpEffect, transform.position, Quaternion.identity);
            Destroy(Effect, 2f);
            maxHP += 10;        // ������ �� �ִ� ü�� ����
            GetComponent<PlayerDamage>().curHp = maxHP;
            GetComponent<PlayerDamage>().hpUpdate();
            //currentHP = maxHP;  // ������ �� ü���� ������ ȸ����
            atk++;              // ���ݷ� ����
            def++;              // ���� ����

        }
    }

}
