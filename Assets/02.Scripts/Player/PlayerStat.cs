using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStat : MonoBehaviour
{
    public static PlayerStat instance;

    public int character_Lv;
    public int[] needExp; //만랩 설정
    public int currentEXP;

    public int initHP;      // 시작 체력
    //public int currentHP;   // 현재 체력
    public int maxHP;              // 최대 체력

    public int atk;         // 공격력
    public int def;         // 방어력

    public float critical;  // 크리티컬 확률

    public GameObject LevelUpEffect;    // 레벨 업 시 이펙트
    void Awake()
    {
        instance = this;
        // 저장된 데이터 불러오기
        //if(저장된 데이터가 없다면)
        InitCharacterData();
    }

    // 최초로 접속한 경우(데이터가 없는 경우 플레이어 스탯 설정)
    void InitCharacterData()
    {
        character_Lv = 1;
        currentEXP = 0;

        initHP = 100;
        maxHP = initHP;
        //currentHP = maxHP;

        // 기본 장비에 따라 영향을 받을 예정(기본 장비 없는 경우 수정)
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
    //        Debug.Log("체력 0미만, 게임오버");

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
            Debug.Log("레벨 업!");

            int overExp = currentEXP - needExp[character_Lv];
            character_Lv++;
            currentEXP = overExp;

            GameObject Effect = Instantiate(LevelUpEffect, transform.position, Quaternion.identity);
            Destroy(Effect, 2f);
            maxHP += 10;        // 레벨업 시 최대 체력 증가
            GetComponent<PlayerDamage>().curHp = maxHP;
            GetComponent<PlayerDamage>().hpUpdate();
            //currentHP = maxHP;  // 레벨업 시 체력이 완전히 회복됨
            atk++;              // 공격력 증가
            def++;              // 방어력 증가

        }
    }

}
