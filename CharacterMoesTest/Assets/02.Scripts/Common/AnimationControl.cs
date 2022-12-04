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
        //GetCurrentAnimatorStateInfo : 애니메이션 Layer 지정, normalizedTime : 애니메이션 진행 시간(0~1)
        if (animator.GetCurrentAnimatorStateInfo(1).normalizedTime > 0.5f)
        {
            if (temp >= 0) // 시간이 지남에 따라 Layer Weight을 서서히 감소시긴다.
            {
                temp -= Time.deltaTime;
            }
            animator.SetLayerWeight(1, temp);
            //SetLayerWeight(x, y) : x번 인덱스의 레이어 Weight을 y로 바꾼다.
        }
        
    }
}
