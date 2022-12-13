using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Skill Data",
    menuName = "Scriptable/Skill Data")]
public class SkillData : ScriptableObject
{
    [Header("��ų ����")]
    public string skillName;            // ��ų ��
    public Sprite skillIcon;            // ��ų ������UI�� �̹���
    public GameObject skillEffect;      // ��ų ����Ʈ
    public string skillComment;         // ��ų ����(����)

    [Header("��ų ���� ���� ����")]
    public float f_skillRange = 0f;     // ��ų ����
    public float f_skillDamage = 0f;    // ��ų ������(���ݷ¿� ���� ����ȭ ����)
    public float f_skillCoolTime = 0f;  // ��ų ��Ÿ��
}
