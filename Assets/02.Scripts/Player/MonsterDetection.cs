using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterDetection : MonoBehaviour
{
    private SphereCollider _radius;

    private readonly string monsterTag = "ENEMY";

    MonsterAI monsterAI;

    void Start()
    {
        _radius = GetComponent<SphereCollider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == null) return; 
        if (other.CompareTag(monsterTag))
        {
            other.GetComponent<MonsterAI>().Hp_Canvas.enabled = true;
            other.GetComponent<MonsterAI>().isInDetection = true;
        }  
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == null) return;
        if (other.CompareTag(monsterTag))
        {
            other.GetComponent<MonsterAI>().isInDetection = false;
            // 감지 반경을 벗어나더라도 공격 받은 몬스터의 Hp Bar 활성화가 일정시간 유지된다.
            // (이후 MonsterAI.cs > HpUIActivation() 에서 처리됨)
            if (other.GetComponent<MonsterAI>().isDamaged == true) return;
            other.GetComponent<MonsterAI>().Hp_Canvas.enabled = false;
        }
    }
}
