using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class enemyAI : MonoBehaviour
{
    public Image hpBar;
    public float hp = 100f;

    Transform tr;

    void Start() //���ʹ� ������ �׽�Ʈ����...... �÷��̾� �ݶ��̴� �̽� ������ �׽�Ʈ�� ����
    {
        tr = GetComponent<Transform>();
        hpBar = GetComponent<Transform>().GetChild(0).GetChild(0).GetComponent<Image>();
        hpBar.color = Color.green;
    }

    
    void Update()
    {
        
    }
}
