using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PunchCtrl : MonoBehaviour
{
    Transform playerTr;
    Animator animator;
    [SerializeField]
    Transform hitPos;
    [SerializeField]
    ParticleSystem hitEffect;

    public static bool IsIdle = false;

    readonly int hashPunch = Animator.StringToHash("Punch");
    readonly int hashIdle = Animator.StringToHash("Idle");
    int punchCount = 0;
    void Awake()
    {
        hitEffect = GetComponent<Transform>().GetChild(0).GetChild(0).GetChild(0).GetComponent<ParticleSystem>();
        hitPos = GetComponent<Transform>().GetChild(0).GetChild(0).transform;
        playerTr = GetComponent<Transform>();
        animator = GetComponent<Animator>();
        hitEffect.Stop();
        animator.SetBool(hashIdle, true);
    }
    
    void Update()
    {
        Punch();
    }
    void Punch()
    {
        if (Input.GetMouseButtonDown(0))
        {
            punchCount++;
            animator.SetInteger(hashPunch, punchCount % 3);
            //AnimationControl.animationControl.AttackAni();
            if (punchCount % 3 == 0)
            {
                StartCoroutine(Effect());
                //Invoke("EffectPlay", 0.6f);
                //Invoke("EffectStop", 1.2f);
            }
            //콜라이더와 닿지 않았다면 punchCount = 0이 되도록하기. 일단 캐릭터 정해서 콜라이더 설정하고 enemy정해지면 해야 될 듯.
            animator.SetBool(hashIdle, false);
            IsIdle = false;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            animator.SetBool(hashIdle, true);
            //Invoke("LayerWeight", 1f); - Layer Mask로 부위별 애니메이션 작동하도록 설정해야되는데 적당한 애니메이션 찾는게 중요할 듯
            IsIdle = true;
        }
            
    }
    IEnumerator Effect()
    {
        yield return new WaitForSeconds(0.6f);
        hitEffect.Play();
        yield return new WaitForSeconds(0.6f);
        hitEffect.Stop();
    }
    //void LayerWeight()
    //{
    //    animator.SetLayerWeight(1, 0);
    //}
    //void EffectStop()
    //{
    //    hitEffect.Stop();
    //}
    //void EffectPlay()
    //{
    //    hitEffect.Play();
    //}
}
