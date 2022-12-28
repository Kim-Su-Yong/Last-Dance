using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeBoss : MonoBehaviour
{
    public Animator ani;
    public Boss boss;
    public int melee;

    private void OnTriggerEnter(Collider col)
    {
        melee = Random.Range(0, 4);
        switch (melee)
        {
            case 0: // 평타 1
                ani.SetFloat("Skills", 0);
                boss.hit_select = 0;
                break;
            case 1: // 평타 2
                ani.SetFloat("Skills", 0.5f);
                boss.hit_select = 1;
                break;
            //case 2: // 점프 공격
            //    ani.SetFloat("Skills", 0);
            //    boss.hit_select = 2;
            //    break;
            case 3: // 파이어볼
                if (boss.phase == 2)
                {
                    ani.SetFloat("Skills", 1f);
                }
                else
                {
                    melee = 0;
                }
                break;
        }
        ani.SetBool("isWalk", false);
        ani.SetBool("isAttack", true);
        boss.isAttack = true;
        GetComponent<CapsuleCollider>().enabled = false;
    }
}
