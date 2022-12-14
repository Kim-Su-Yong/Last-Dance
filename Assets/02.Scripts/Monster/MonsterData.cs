using UnityEngine;

[CreateAssetMenu(menuName ="Scriptable/MonsterData", fileName = "Monster Data")]
public class MonsterData : ScriptableObject
{
    [Header("Condition")]
    public float HP = 100f;

    [Header("Attack")]
    public float damage = 20f;
    public float attackSpeed = 0.5f;  // Between 공격 속도?
    //public float lastAttackTime;       // 마지막 공격 시점

    [Header("Movement")]
    public float patrolSpeed = 1.5f;
    public float traceSpeed = 4.0f;

    [Header("AI Distance")]
    public float attackDist = 2.0f;
    public float traceDist = 10.0f;
}
