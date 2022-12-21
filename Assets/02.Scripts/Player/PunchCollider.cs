using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PunchCollider : MonoBehaviour
{
    public int damage;      // ÆÝÄ¡ µ¥¹ÌÁö
    BoxCollider collider;

    private void Awake()
    {
        collider = GetComponent<BoxCollider>();
        collider.enabled = false;
        damage = 20;//(int)(PlayerStat.instance.atk * 2) + 6;
    }
    void OnEnable()
    {
        //
        
    }

    private void Update()
    {
        damage = (int)(PlayerStat.instance.atk * 2) + 6;
    }

}
