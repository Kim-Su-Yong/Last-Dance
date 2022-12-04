using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Scriptable
// 대량의 데이터를 저장 및 보관 용도로 사용되는 만들어진 Asset

[CreateAssetMenu(fileName = "Character Data", 
    menuName ="Data/Character Data", order = 1)]
public class CharacterData : ScriptableObject
{
    [Header("캐릭터 기본 정보")]
    public string charName;         // 캐릭터 이름(닉네임이 될 수 있을 듯?)
    public string charForm;         // 캐릭터 폼 상태(게임 종료시 어떤 폼이 마지막 폼이였는 지 저장)
    public Sprite charImage;        // 캐릭터 초상화??
    public string comment = "";     // 설명

    [Header("캐릭터 스탯 정보")]
    public int damage;              // 캐릭터 공격력
    public float atkSpeed = 1;      // 캐릭터 공격속도(폼이나 장비에 따른 공격속도 변화가 있을 수 있음)
    public int maxHp = 100;         // 최대 체력
    public int curHp;               // 현재 체력
    public int def = 10;            // 방어력
    public float moveSpeed = 6;     // 이동 속도
}
