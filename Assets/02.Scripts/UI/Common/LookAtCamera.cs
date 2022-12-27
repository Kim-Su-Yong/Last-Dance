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
    }

    void Update()
    {
        CanvasTr.LookAt(CamTr.transform);
        CanvasTr.rotation = Quaternion.LookRotation(CanvasTr.position - CamTr.position);
    }
}
