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

    [Header("�÷��̾� ����")]
    public State state = State.IDLE; // ĳ������ ���� enum ����

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
            // 0.3�ʸ��� ���� üũ
            yield return new WaitForSeconds(0.3f);

            switch(state)
            {
                case State.IDLE: // ĳ���Ͱ� IDLE, MOVE���¿��� ��� ������ ����
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
