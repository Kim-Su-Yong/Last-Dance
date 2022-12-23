using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EquipStat : MonoBehaviour
{
    public int Atk { get; set; } //공격력
    public int Def { get; set; } //방어력
    public int MaxHP { get; set; } //최대체력
    public float Speed { get; set; } //이동속도

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
