using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoarCollider : MonoBehaviour
{
    public int damage;      // ÆÝÄ¡ µ¥¹ÌÁö
    SphereCollider sphereCollider;
    public SkillData roarData;

    private void Awake()
    {
        roarData = Resources.Load("SkillData/Roar Data") as SkillData;
        sphereCollider = GetComponent<SphereCollider>();
        sphereCollider.enabled = false;   
    }

    void Update()
    {
        damage = (int)(roarData.f_skillDamage + PlayerStat.instance.atk * 1.8);
    }
}
