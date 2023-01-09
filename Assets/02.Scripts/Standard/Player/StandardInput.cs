using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class StandardInput : MonoBehaviour
{
    [Header("ĳ���� �Է� ��")]
    public Vector2 move;        // �̵� �Է°�
    public Vector2 look;        // ĳ���Ͱ� �ٶ󺸴� ����
    public bool isJump;         // ���� ����Ȯ��
    public bool isWalk;         // �ȱ� �������� Ȯ��

    [Header("�̵� ����")]
    public bool analogMovement; // ????

    [Header("���콺 Ŀ�� ����")]
    public bool cursorLocked = true;
    public bool cursorInputForLook = true;

    [Header("�Է� ���� ������ ���� �߰�")]
    PlayerAttack playerAttack;
    ThirdPersonCtrl playerCtrl;
    Shooter shooter;
    ChangeForm changeForm;

    private void Awake()
    {
        playerAttack = GetComponent<PlayerAttack>();
        playerCtrl = GetComponent<ThirdPersonCtrl>();
        shooter = GetComponent<Shooter>();
        changeForm = GetComponent<ChangeForm>();
    }

    public void OnMove(InputValue value)
    {
        if (!cursorLocked)
            return;
        MoveInput(value.Get<Vector2>());
    }
    public void OnLook(InputValue value)
    {
        if (!cursorLocked)
            return;
        if (cursorInputForLook)
        {
            LookInput(value.Get<Vector2>());
        }
    }
    public void OnJump(InputValue value)
    {
        if (!cursorLocked)
            return;
        // �÷��̾ ��ȣ �ۿ��, �׾��� �� �Է��� ���� ����
        if (GetComponent<PlayerDamage>().isDie)
        {
            return;
        }
        if (GameManager.instance.isAction)  
            return;
        JumpInput(value.isPressed);
    }
    public void OnWalk(InputValue value)
    {
        if (!cursorLocked)
            return;
        WalkInput(value.isPressed);
    }

    public void MoveInput(Vector2 newMoveDirection)
    {
        if (!cursorLocked)
            return;
        move = newMoveDirection;
    }

    public void LookInput(Vector2 newLookDirection)
    {
        if (!cursorLocked)
            return;
        look = newLookDirection;
    }

    public void JumpInput(bool newJumpState)
    {
        if (GetComponent<PlayerState>().state == PlayerState.State.ATTACK)
            return;
        isJump = newJumpState;
    }

    public void WalkInput(bool newSprintState)
    {
        isWalk = newSprintState;
    }

    private void OnApplicationFocus(bool hasFocus)
    {
        SetCursorState(cursorLocked);
    }

    public void SetCursorState(bool newState)
    {
        Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
    }

    void OnIdleStart()
    {
        //    playerAttack.enabled = true;
        //    playerAttack.bIsAttack = false;
        //    playerCtrl.enabled = true;
        //    shooter.enabled = true;
        //    changeForm.enabled = true;
    }

}
