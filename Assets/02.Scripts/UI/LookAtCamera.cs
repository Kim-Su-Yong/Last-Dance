using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    Transform CanvasTr;
    Transform CamTr;
    void Start()
    {
        CanvasTr = GetComponent<Transform>();
        CamTr = Camera.main.transform;
        this.transform.Rotate(0, 180, 0);
    }

    void Update()
    {
        CanvasTr.LookAt(CamTr.transform);
    }
}
