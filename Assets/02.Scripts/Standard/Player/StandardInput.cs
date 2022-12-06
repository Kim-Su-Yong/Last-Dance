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

    public void OnMove(InputValue value)
    {
        MoveInput(value.Get<Vector2>());
    }
    public void OnLook(InputValue value)
    {
        if(cursorInputForLook)
        {
            LookInput(value.Get<Vector2>());
        }
    }
    public void OnJump(InputValue value)
    {
        JumpInput(value.isPressed);
    }
    public void OnWalk(InputValue value)
    {
        WalkInput(value.isPressed);
    }

    public void MoveInput(Vector2 newMoveDirection)
    {
        move = newMoveDirection;
    }

    public void LookInput(Vector2 newLookDirection)
    {
        look = newLookDirection;
    }

    public void JumpInput(bool newJumpState)
    {
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

    private void SetCursorState(bool newState)
    {
        Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
    }

}
