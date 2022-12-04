using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkill : MonoBehaviour
{
    Transform playerTr;
    Animator animator;
    GameObject enemy;

    readonly int hashRoar = Animator.StringToHash("IsRoar");
    
    void Start()
    {
        playerTr = GetComponent<Transform>();
        animator = GetComponent<Animator>();
        enemy = GameObject.FindWithTag("Enemy").gameObject;
    }

    
    void Update()
    {
        Roar();
    }
    void Roar() //포효 스킬
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            animator.SetTrigger(hashRoar);
            
        }
    }
    //IEnumerator enemyDebuff() 적 스크립트 
    //{
    //    enemy
    //}
}
