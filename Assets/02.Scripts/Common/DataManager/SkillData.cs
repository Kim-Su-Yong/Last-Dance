using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Skill Data",
    menuName = "Scriptable/Skill Data")]
public class SkillData : ScriptableObject
{
    [Header("스킬 정보")]
    public string skillName;            // 스킬 명
    public Sprite skillIcon;            // 스킬 아이콘UI용 이미지
    public GameObject skillEffect;      // 스킬 이펙트
    public string skillComment;         // 스킬 정보(설명)

    [Header("스킬 공격 관련 정보")]
    public float f_skillRange = 0f;     // 스킬 범위
    public float f_skillDamage = 0f;    // 스킬 데미지(공격력에 따른 공식화 예정)
    public float f_skillCoolTime = 0f;  // 스킬 쿨타임
}
