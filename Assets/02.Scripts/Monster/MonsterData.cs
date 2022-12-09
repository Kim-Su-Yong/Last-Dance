using UnityEngine;

[CreateAssetMenu(menuName ="Scriptable/MonsterData", fileName = "Monster Data")]
public class MonsterData : ScriptableObject
{
    [Header("Condition")]
    public float HP = 100f;
    public float damage = 20f;

    [Header("Movement")]
    public float patrolSpeed = 1.5f;
    public float traceSpeed = 4.0f;

    [Header("AI Distance")]
    public float attackDist = 2.0f;
    public float traceDist = 10.0f;

    [Header("Material Color")]
    public Color skinColor = Color.white;
}
