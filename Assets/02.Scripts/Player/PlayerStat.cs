using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStat : MonoBehaviour
{
    public static PlayerStat instance;

    public int character_Lv;    // �÷��̾� ����
    public int[] needExp;       // ������ ���� �ʿ� ����ġ
    public int currentEXP;      // ���� ����ġ

    public int initHP;          // ���� ü��
    public int maxHP;           // �ִ� ü��
    public int maxHP2;
    public int atk;             // ���ݷ�
    public int atk2;
    public int def;             // ����
    public int def2;
    public float speed;         // �̵� �ӵ�
    public float speed2;
    public float critical;      // ũ��Ƽ�� Ȯ��-�� ���ɼ� ����

    public GameObject LevelUpEffect;    // ���� �� �� ����Ʈ
    public Image ExpBar;                // ����ġ �� �̹���
    public Text LvText;                 // ���� UI �ؽ�Ʈ(����ġ %�� ǥ��)

    public Text moneyText;

    AudioSource source;             
    public AudioClip levelUpClip;   // ������ ����

    public bool isDebug;        // ����� ���� ü�� ���� ����

    public int money = 4500;    // ������
    void Awake()
    {
        instance = this;
        // ����� ������ �ҷ�����
        //if(����� �����Ͱ� ���ٸ�)
        levelUpClip = Resources.Load<AudioClip>("Sound/Player/PlayerLvUp");
        source = GetComponent<AudioSource>();
        InitCharacterData();
    }

    // ���ʷ� ������ ���(�����Ͱ� ���� ��� �÷��̾� ���� ����)
    void InitCharacterData()
    {
        character_Lv = 1;
        currentEXP = 0;

        if (isDebug)
            initHP = 1000;
        else
            initHP = 100;
        maxHP = initHP;
        maxHP2 = maxHP;

        speed = 10f;
        speed2 = speed;
        //currentHP = maxHP;

        // �⺻ ��� ���� ������ ���� ����(�⺻ ��� ���� ��� ����)
        atk = 5;
        atk2 = atk;
        def = 5;
        def2 = def;

        critical = 25f;

        expUpdate();
        moneyText.text = money.ToString();
    }

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
        expUpdate();
        if (currentEXP >= needExp[character_Lv])    // ���� ����ġ�� �������� �ʿ����ġ �̻��̸�
        {
            LevelUp();
        }
    }

    private void LevelUp()
    {
        int overExp = currentEXP - needExp[character_Lv];   // �ʰ� ����ġ ���
        character_Lv++;         // ���� ��
        currentEXP = overExp;   // ���� ����ġ�� �ʰ� ����ġ�� ����(0�̸� 0���� ����)
        expUpdate();

        // ������ ����Ʈ
        GameObject Effect = Instantiate(LevelUpEffect, transform.position, Quaternion.identity);
        Destroy(Effect, 2f);
        source.PlayOneShot(levelUpClip);

        // ���� ����
        maxHP += 10;        // ������ �� �ִ� ü�� ����
                            // UI ������� ����(ü�¹�)
        maxHP2 = maxHP;
        GetComponent<PlayerDamage>().curHp = maxHP; // ���� ü���� �ִ�ü������ ����(�������� Ǯ�� ȸ��)
        GetComponent<PlayerDamage>().hpUpdate();
        atk++;              // ���ݷ� ����
        atk2 = atk;
        def++;              // ���� ����
        def2 = def;
    }

    public void expUpdate()
    {
        ExpBar.fillAmount = (float)currentEXP / needExp[character_Lv];
        LvText.text = "LV " + character_Lv.ToString() + "(EXP " + ((int)(ExpBar.fillAmount*100)).ToString() + "%)";
    }

    public void ChangeMoney(int newMoney)
    {
        money += newMoney;
        money = Mathf.Clamp(money, 0, 99999999);
        moneyText.text = money.ToString();
    }

}
