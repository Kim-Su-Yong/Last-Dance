using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAttack : MonoBehaviour
{
    // Attack Value
    public float damage = 20f;
    public float attackSpeed = 0.5f;  // Between
    private float nextAttackTime = 0;

    // 해야 할 것 : 공격하지 않을 땐 콜라이더 enabled = false;로 변경
    // Components
    [HideInInspector]
    public SphereCollider attackCollider;

    // Etc.
    private readonly float damping = 10.0f; // Monster LookAt Player 회전 계수

    // Script
    MonsterAI monsterAI;

    //// Animation String To Hash Array
    //private readonly int hashAttack1 = Animator.StringToHash("Attack 1");
    //private readonly int hashAttack2 = Animator.StringToHash("Attack 2");
    //private readonly int hashAttack3 = Animator.StringToHash("Attack 3");
    //private readonly int hashAttack4 = Animator.StringToHash("Attack 4");

    //public int[] hashArrayAni; // hashAttack(n)을 넣기 위한 배열
    // 굳이 배열에 넣어서 하고 싶으면 String 값을 배열에 넣어서 해볼 것

    // Audio Clip
    private AudioClip attackSound;

    // Bool
    public bool isAttack = false;

    void Awake()
    {
        monsterAI = GetComponent<MonsterAI>();
        // Resources
        attackSound = Resources.Load<AudioClip>("Sound/");
    }
    void Start()
    {

    }
    void Update()
    {
        if (isAttack)
        {
            if (Time.time >= nextAttackTime)
            {
                Attack();
                nextAttackTime = Time.time + attackSpeed + Random.Range(0.1f, 0.3f);
            }
            Quaternion rot = Quaternion.LookRotation(monsterAI.playerTr.position - monsterAI.monsterTr.position);
            monsterAI.monsterTr.rotation = Quaternion.Slerp(monsterAI.monsterTr.rotation, rot, Time.deltaTime * damping);
            
        }
    }

    private void Attack()
    {
        // 애니메이션 랜덤 재생
        switch(monsterAI.monsterType)
        {
            case MonsterAI.MonsterType.A_Skeleton:
                monsterAI.animator.SetTrigger($"Attack {Random.Range(1, 4)}");
                break;
            case MonsterAI.MonsterType.B_Fishman:
                monsterAI.animator.SetTrigger($"Attack {Random.Range(1, 3)}");
                break;
            case MonsterAI.MonsterType.C_Slime:
                break;
        }
        //monsterAI.animator.SetTrigger(hashArrayAni[Random.Range(0,3)]);
        //monsterAI.audio.PlayOneShot(attackSound, 1.0f);
    }
}