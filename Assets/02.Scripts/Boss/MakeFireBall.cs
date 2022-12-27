using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakeFireBall : MonoBehaviour
{
    public float timer;

    void Start()
    {
        
    }

    void Update()
    {
        timer += 1 * Time.deltaTime;
        if (timer > 3)
        {
            gameObject.SetActive(false);
            timer = 0;
        }
        transform.Translate(Vector3.forward * 15 * Time.deltaTime);
    }
}
