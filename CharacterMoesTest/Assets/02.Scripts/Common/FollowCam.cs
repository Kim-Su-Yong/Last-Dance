using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCam : MonoBehaviour
{
    public GameObject target;
    public Vector3 offset = new Vector3(0f, 5f, -5f);

    void Start()
    {

    }

    void LateUpdate()
    {
        transform.position = target.transform.position + offset;

    }
}
