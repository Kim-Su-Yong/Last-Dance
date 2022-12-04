using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationControl : MonoBehaviour
{
    public Animator animator;
    float temp = 1;
    public static AnimationControl animationControl;

    void Start()
    {
        animationControl = this;
        animator = GameObject.FindWithTag("Player").GetComponent<Animator>();
        animator.SetLayerWeight(1, 0);
    }

    
    void Update()
    {

    }
    public void AttackAni()
    {
        animator.SetLayerWeight(1, 1);
        //GetCurrentAnimatorStateInfo : �ִϸ��̼� Layer ����, normalizedTime : �ִϸ��̼� ���� �ð�(0~1)
        if (animator.GetCurrentAnimatorStateInfo(1).normalizedTime > 0.5f)
        {
            if (temp >= 0) // �ð��� ������ ���� Layer Weight�� ������ ���ҽñ��.
            {
                temp -= Time.deltaTime;
            }
            animator.SetLayerWeight(1, temp);
            //SetLayerWeight(x, y) : x�� �ε����� ���̾� Weight�� y�� �ٲ۴�.
        }
        
    }
}
