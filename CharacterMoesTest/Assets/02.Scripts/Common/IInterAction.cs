using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum InterActionType
{
    NPC,
    ITEM
}

public interface IInterAction
{
    // 상호작용 타입(NPC, 아이템 등)
    InterActionType InterType { get; }
    // 플레이어와 상호작용 가능한지 체크
    bool enable { get; set; }
    // 상호작용 시도 할 수 있다는 상태를 보여주기 위함
    string InterString { get; set; }
    // 상호작용이 가능 시 발생하는 행동(NPC 손 흔들기)
    void ShowInter();
    // 상호작용키를 눌렀을 때 실행
    void ActionKey();
    // 상호작용이 끝난 후
    void EndInter();
    // 상호작용이 불가능 시 발생하는 행동
    void NonShowInter();

}
