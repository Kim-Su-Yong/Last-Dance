using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PunchCollider : MonoBehaviour
{
    public int damage;      // ��ġ ������
    public bool powerPunch; // ����ġ �Ǵܺ���
    BoxCollider collider;

    private void Awake()
    {
        collider = GetComponent<BoxCollider>();
        collider.enabled = false;
        //damage = 20;//(int)(PlayerStat.instance.atk * 2) + 6;
    }
    void OnEnable()
    {
        //
        
    }

    private void Update()
    {
        if(!powerPunch)
            damage = (int)(PlayerStat.instance.atk * 2) + 6;
        else
            damage = (int)((PlayerStat.instance.atk * 2) + 6) * 2;

    }

}
