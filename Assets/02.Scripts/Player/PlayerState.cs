using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : MonoBehaviour
{
    public enum State 
        { IDLE,
          MOVE,
          ATTACK,
          JUMP,
          DIE, 
          HIT
        };

    [Header("플레이어 상태")]
    public State state = State.IDLE; // 캐릭터의 상태 enum 변수

    PlayerAttack playerAttack;
    ThirdPersonCtrl playerCtrl;
    Shooter shooter;
    ChangeForm changeForm;

    void Awake()
    {
        playerAttack = GetComponent<PlayerAttack>();
        playerCtrl = GetComponent<ThirdPersonCtrl>();
        shooter = GetComponent<Shooter>();
        changeForm = GetComponent<ChangeForm>();
    }

    private void OnEnable()
    {
        StartCoroutine(CheckState());
    }

    IEnumerator CheckState()
    {
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
                    break;
                case State.ATTACK:
                    shooter.enabled = false;
                    playerCtrl.enabled = false;
                    //playerAttack.enabled = false;
                    changeForm.enabled = false;
                    break;
                case State.JUMP:
                    shooter.enabled = false;
                    playerAttack.enabled = false;
                    changeForm.enabled = false;
                    playerCtrl.enabled = true;
                    break;

                case State.DIE:
                    shooter.enabled = false;
                    playerCtrl.enabled = false;
                    changeForm.enabled = false;
                    playerAttack.enabled = false;
                    break;

                case State.HIT:
                    playerCtrl.enabled = false;
                    shooter.enabled = true;
                    changeForm.enabled = false;
                    playerAttack.enabled = true;
                    break;
            }
        }

    }
}
