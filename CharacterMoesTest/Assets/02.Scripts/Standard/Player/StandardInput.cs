using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;  

public class StandardInput : MonoBehaviour
{
    [Header("캐릭터 입력 값")]
    public Vector2 move;        // 이동 입력값
    public Vector2 look;        // 캐릭터가 바라보는 방향
    public bool isJump;         // 점프 상태확인
    public bool isWalk;         // 걷기 상태인지 확인

    [Header("이동 설정")]
    public bool analogMovement; // ????

    [Header("마우스 커서 설정")]
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
