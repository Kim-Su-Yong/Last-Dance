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
        }  
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == null) return;
        if (other.CompareTag(monsterTag))
        {
            other.GetComponent<MonsterAI>().Hp_Canvas.enabled = false;
        }
    }
}
