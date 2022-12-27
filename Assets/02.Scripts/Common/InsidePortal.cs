using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InsidePortal : MonoBehaviour
{
    public Transform TranslatePosition;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Debug.Log("��Ż �Լ�");
            Transform ParentTransform = other.transform;
            while (true)
            {
                if (ParentTransform.parent == null)
                    break;
                else
                    ParentTransform = ParentTransform.parent;
            }

            ParentTransform.position = TranslatePosition.position;
        }
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if(hit.gameObject.tag == "Player")
        {
            Debug.Log("��Ż �Լ�");
            Transform ParentTransform = hit.transform;
            while(true)
            {
                if (ParentTransform.parent == null)
                    break;
                else
                    ParentTransform = ParentTransform.parent;
            }

            ParentTransform.position = TranslatePosition.position;
        }
    }
}
