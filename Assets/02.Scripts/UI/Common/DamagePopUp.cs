using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

// Instantiate 는 MonsterDamage.cs 스크립트 ShowDamageEffect() 메서드에 있습니다.
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
