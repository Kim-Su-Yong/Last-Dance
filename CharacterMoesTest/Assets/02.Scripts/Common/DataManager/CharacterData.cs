using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Scriptable
// �뷮�� �����͸� ���� �� ���� �뵵�� ���Ǵ� ������� Asset

[CreateAssetMenu(fileName = "Character Data", 
    menuName ="Data/Character Data", order = 1)]
public class CharacterData : ScriptableObject
{
    [Header("ĳ���� �⺻ ����")]
    public string charName;         // ĳ���� �̸�(�г����� �� �� ���� ��?)
    public string charForm;         // ĳ���� �� ����(���� ����� � ���� ������ ���̿��� �� ����)
    public Sprite charImage;        // ĳ���� �ʻ�ȭ??
    public string comment = "";     // ����

    [Header("ĳ���� ���� ����")]
    public int damage;              // ĳ���� ���ݷ�
    public float atkSpeed = 1;      // ĳ���� ���ݼӵ�(���̳� ��� ���� ���ݼӵ� ��ȭ�� ���� �� ����)
    public int maxHp = 100;         // �ִ� ü��
    public int curHp;               // ���� ü��
    public int def = 10;            // ����
    public float moveSpeed = 6;     // �̵� �ӵ�
}
