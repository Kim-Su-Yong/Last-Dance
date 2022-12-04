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
    // ��ȣ�ۿ� Ÿ��(NPC, ������ ��)
    InterActionType InterType { get; }
    // �÷��̾�� ��ȣ�ۿ� �������� üũ
    bool enable { get; set; }
    // ��ȣ�ۿ� �õ� �� �� �ִٴ� ���¸� �����ֱ� ����
    string InterString { get; set; }
    // ��ȣ�ۿ��� ���� �� �߻��ϴ� �ൿ(NPC �� ����)
    void ShowInter();
    // ��ȣ�ۿ�Ű�� ������ �� ����
    void ActionKey();
    // ��ȣ�ۿ��� ���� ��
    void EndInter();
    // ��ȣ�ۿ��� �Ұ��� �� �߻��ϴ� �ൿ
    void NonShowInter();

}
