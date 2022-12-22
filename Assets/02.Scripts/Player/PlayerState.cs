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

    [Header("�÷��̾� ����")]
    public State state = State.IDLE; // ĳ������ ���� enum ���� �ʱⰪ�� Idle

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
        // ������°� �Ǹ� ���̻� ĳ���� ���¸� üũ���� ����
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
                    playerAction.enabled = true;
                    break;
                case State.ATTACK:  // ���� �߿��� ������ ������ ��� ���� �Ұ�
                    shooter.enabled = false;
                    playerCtrl.enabled = false;
                    changeForm.enabled = false;
                    playerAction.enabled = false;
                    break;
                case State.JUMP:    // ���� �߿��� �̵� ���۸� ����
                    shooter.enabled = false;
                    playerAttack.enabled = false;
                    changeForm.enabled = false;
                    playerCtrl.enabled = true;
                    playerAction.enabled = false;
                    break;

                case State.DIE:     // ������ ��� ���� �Ұ�
                    shooter.enabled = false;
                    playerCtrl.enabled = false;
                    changeForm.enabled = false;
                    playerAttack.enabled = false;
                    playerAction.enabled = false;
                    break;

                case State.HIT:     // �ǰ� �߿��� ��� ���� �Ұ�(�����߿��� �ǰݻ��·� ��ȯ���� �ʰ� �������� �ް� ������ ����)
                    playerCtrl.enabled = false;
                    shooter.enabled = false;
                    changeForm.enabled = false;
                    playerAttack.enabled = false;
                    playerAction.enabled = false;
                    break;
                case State.TALK:    // ��ũ �߿��� ��� ���� �Ұ�
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
