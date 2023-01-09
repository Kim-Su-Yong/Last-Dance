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

    [Header("입력 감지 제한을 위해 추가")]
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
        // 플레이어가 상호 작용시, 죽었을 시 입력을 받지 않음
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
