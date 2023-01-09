using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStat : MonoBehaviour
{
    public static PlayerStat instance;

    public int character_Lv;    // 플레이어 레벨
    public int[] needExp;       // 레벨에 따른 필요 경험치
    public int currentEXP;      // 현재 경험치

    public int initHP;          // 시작 체력
    public int maxHP;           // 최대 체력
    public int atk;             // 공격력
    public int def;             // 방어력
    public float speed;         // 이동 속도
    public float critical;      // 크리티컬 확률-뺄 가능성 높음

    public GameObject LevelUpEffect;    // 레벨 업 시 이펙트
    public Image ExpBar;
    public Text LvText;

    AudioSource source;
    public AudioClip levelUpClip;

    public bool isDebug;

    public int money = 4500;
    void Awake()
    {
        instance = this;
        // 저장된 데이터 불러오기
        //if(저장된 데이터가 없다면)
        source = GetComponent<AudioSource>();
        InitCharacterData();
    }

    // 최초로 접속한 경우(데이터가 없는 경우 플레이어 스탯 설정)
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

        // 기본 장비에 따라 영향을 받을 예정(기본 장비 없는 경우 수정)
        atk = 5;
        def = 5;

        critical = 25f;

        expUpdate();
    }

    void Update()
    {
        //// 경험치 획득 테스트
        //if (Input.GetKeyDown(KeyCode.R))
        //{
        //    GainExp(24);
        //}
    }

    // 경험치 획득 함수(몹을 잡거나 퀘스트 클리어시 사용)
    public void GainExp(int newEXP)
    {
        currentEXP += newEXP;       // 현재 경험치 + 획득 경험치
        expUpdate();
        if (currentEXP >= needExp[character_Lv])    // 현재 경험치가 레벨업시 필요경험치 이상이면
        {
            LevelUp();
        }
    }

    private void LevelUp()
    {
        int overExp = currentEXP - needExp[character_Lv];   // 초과 경험치 계산
        character_Lv++;         // 레벨 업
        currentEXP = overExp;   // 현재 경험치를 초과 경험치로 변경(0이면 0으로 변경)
        expUpdate();

        // 레벨업 이펙트
        GameObject Effect = Instantiate(LevelUpEffect, transform.position, Quaternion.identity);
        Destroy(Effect, 2f);
        source.PlayOneShot(levelUpClip);

        // 스텟 증가
        maxHP += 10;        // 레벨업 시 최대 체력 증가
                            // UI 변경사항 적용(체력바)
        GetComponent<PlayerDamage>().curHp = maxHP; // 현재 체력을 최대체력으로 변경(레벨업시 풀피 회복)
        GetComponent<PlayerDamage>().hpUpdate();
        atk++;              // 공격력 증가
        def++;              // 방어력 증가
    }

    void expUpdate()
    {
        ExpBar.fillAmount = (float)currentEXP / needExp[character_Lv];
        LvText.text = "LV " + character_Lv.ToString() + "(EXP " + ((int)(ExpBar.fillAmount*100)).ToString() + "%)";
    }

}
