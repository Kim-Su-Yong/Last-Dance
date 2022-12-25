using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

// Instantiate �� MonsterDamage.cs ��ũ��Ʈ ShowDamageEffect() �޼��忡 �ֽ��ϴ�.
public class DamagePopUp : MonoBehaviour
{
    private float destroyTime = 1.0f;

    private Animation anim;

    private void Awake()
    {
        anim = GetComponent<Animation>();
    }
    private void Start()
    {
        Destroy(this.gameObject, destroyTime);
    }
}
