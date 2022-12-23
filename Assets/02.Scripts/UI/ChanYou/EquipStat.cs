using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EquipStat : MonoBehaviour
{
    public int Atk { get; set; } //���ݷ�
    public int Def { get; set; } //����
    public int MaxHP { get; set; } //�ִ�ü��
    public float Speed { get; set; } //�̵��ӵ�

    public EquipStat(int _Atk, int _Def, int _MaxHP, float _Speed)
    {
        Atk = _Atk;
        Def = _Def;
        MaxHP = _MaxHP;
        Speed = _Speed;
    }
    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
