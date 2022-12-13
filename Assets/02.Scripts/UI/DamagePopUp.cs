using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DamagePopUp : MonoBehaviour
{
    private TextMeshPro textMesh;
    private float destroyTime = 1.0f;

    private Animation anim;

    private void Awake()
    {
        anim = GetComponent<Animation>();
    }
    private void Start()
    {
        //anim.Play();
        Destroy(this.gameObject, destroyTime);
    }
}
