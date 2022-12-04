using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAction : MonoBehaviour
{
    [SerializeField]
    GameObject nearObject;  // ĳ���Ϳ� ���� ����� ������Ʈ�� ����

    void Interaction()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (nearObject == null) return;
            /*
             * ����� ������� ���¿��� eŰ�� ������ �ش� ��ȣ�ۿ� �˾�â ���
             */
            if(nearObject.CompareTag("NPC"))
            {
                NPCInterAction npc = nearObject.GetComponent<NPCInterAction>();
                npc.IsTalk = true;
                // �÷��̾� �̵�, ���� �� �׼� ���� ���� �ʿ�
            }
        }
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Interaction();
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.CompareTag("NPC"))
        {
            nearObject = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("NPC"))
        {
            nearObject = null;
        }
    }
}
