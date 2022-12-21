using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : MonoBehaviour
{
    public enum State
    {
        IDLE,
        MOVE,
        ATTACK,
        JUMP,
        DIE,
        HIT,
        TALK
    };

    [Header("플레이어 상태")]
    public State state = State.IDLE; // 캐릭터의 상태 enum 변수 초기값은 Idle

    PlayerAttack playerAttack;
    ThirdPersonCtrl playerCtrl;
    Shooter shooter;
    ChangeForm changeForm;
    PlayerAction playerAction;

    Animator animator;

    readonly int hashSpeed = Animator.StringToHash("Speed");

    void Awake()
    {
        playerAttack = GetComponent<PlayerAttack>();
        playerCtrl = GetComponent<ThirdPersonCtrl>();
        shooter = GetComponent<Shooter>();
        changeForm = GetComponent<ChangeForm>();
        playerAction = GetComponent<PlayerAction>();

        animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        StartCoroutine(CheckState());
    }

    IEnumerator CheckState()
    {
        // 사망상태가 되면 더이상 캐릭터 상태를 체크하지 않음
        while(!GetComponent<PlayerDamage>().isDie)
        {
            // 0.3초마다 상태 체크
            yield return new WaitForSeconds(0.3f);

            switch(state)
            {
                case State.IDLE: // 캐릭터가 IDLE, MOVE상태에선 모든 동작이 가능
                case State.MOVE:
                    playerAttack.enabled = true;
                    shooter.enabled = true;
                    playerCtrl.enabled = true;
                    changeForm.enabled = true;
                    playerAction.enabled = true;
                    break;
                case State.ATTACK:  // 공격 중에는 공격을 제외한 모든 동작 불가
                    shooter.enabled = false;
                    playerCtrl.enabled = false;
                    changeForm.enabled = false;
                    playerAction.enabled = false;
                    break;
                case State.JUMP:    // 점프 중에는 이동 동작만 가능
                    shooter.enabled = false;
                    playerAttack.enabled = false;
                    changeForm.enabled = false;
                    playerCtrl.enabled = true;
                    playerAction.enabled = false;
                    break;

                case State.DIE:     // 죽으면 모든 동작 불가
                    shooter.enabled = false;
                    playerCtrl.enabled = false;
                    changeForm.enabled = false;
                    playerAttack.enabled = false;
                    playerAction.enabled = false;
                    break;

                case State.HIT:     // 피격 중에도 모든 동작 불가(공격중에는 피격상태로 전환되지 않고 데미지만 받게 수정할 예정)
                    playerCtrl.enabled = false;
                    shooter.enabled = false;
                    changeForm.enabled = false;
                    playerAttack.enabled = false;
                    playerAction.enabled = false;
                    break;
                case State.TALK:    // 토크 중에는 모든 동작 불가
                    animator.SetFloat(hashSpeed, 0f);
                    playerCtrl.enabled = false;
                    shooter.enabled = false;
                    changeForm.enabled = false;
                    playerAttack.enabled = false;
                    
                    break;
            }
        }

    }
}
