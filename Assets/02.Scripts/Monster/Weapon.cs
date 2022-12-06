using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public enum Type { Melee, Range };
    public Type type;
    public int damage = 20;
    public float rate;
    public BoxCollider meleeArea;
    //public TrailRenderer trailEffect;

    public static Weapon equipWeapon;

    void Awake()
    {
        meleeArea.enabled = false;
        equipWeapon = this;
    }
    public void Use()
    {
        if (type == Type.Melee)
        {
            //StopCoroutine("Swing");
            StartCoroutine("Swing");
        }
    }
    IEnumerator Swing()
    {
        yield return new WaitForSeconds(0.1f);
        meleeArea.enabled = true;
        //trailEffect.enabled = true;
        
        yield return new WaitForSeconds(0.6f);
        meleeArea.enabled = false;

        //yield return new WaitForSeconds(0.5f);
        //trailEffect.enabled = false;
    }
}
