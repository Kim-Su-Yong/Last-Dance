using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove1 : MonoBehaviour
{
    [SerializeField]
    Transform tr;
    [SerializeField]
    Transform camTr;
    [SerializeField]
    Animator animator;

    public float moveSpeed = 6f;
    public float jumpPower = 10f;

    void Start()
    {
        tr = GetComponent<Transform>();
        camTr = Camera.main.transform;
        animator = GetComponent<Animator>();
    }

    void Move()
    {
        Vector2 moveInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        bool isMove = moveInput.magnitude != 0;
        animator.SetBool("IsRun", isMove);
        if(isMove)
        {
            Vector3 lookForward = new Vector3(camTr.forward.x, 0f, camTr.forward.z).normalized;
            Vector3 lookRight = new Vector3(camTr.right.x, 0f, camTr.right.z).normalized;
            Vector3 moveDir = lookForward * moveInput.y + lookRight * moveInput.x;

            tr.forward = moveDir;
            tr.position += moveDir * Time.deltaTime * moveSpeed;
        }
    }

    void LookAround()
    {
        float rAxis = Input.GetAxis("Mouse X");
        Vector3 camAngle = camTr.rotation.eulerAngles;
        float y = camAngle.y + rAxis;

        if(y <180)
        {
            y = Mathf.Clamp(y, -1f, 30f);
        }
        else
        {
            y = Mathf.Clamp(y, 330f,361f);
        }
        camTr.rotation = Quaternion.Euler(camAngle.x, y, camAngle.z);
    }

    void Update()
    {
        LookAround();
        Move();
    }
}
