using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBoss : MonoBehaviour
{
    public int damage;

    private void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Player"))
        {
            //col.GetComponent<PlayerAttack>().hpMin -= damage;

        }
    }
    void Start()
    {

    }

    void Update()
    {

    }
}
