using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAction : MonoBehaviour
{
    [SerializeField]
    GameObject nearObject;  // ĳ���Ϳ� ���� ����� ������Ʈ�� ����
    public static PlayerAction instance;
    public string currentMapName;
    public string currentSceneName;
    private SaveNLoad theSaveNLoad;
    void Interaction()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (nearObject == null) return;
            /*
             * ����� ������� ���¿��� eŰ�� ������ �ش� ��ȣ�ۿ� �˾�â ���
             */
            if (nearObject.CompareTag("NPC"))
            {
                NPCInterAction npc = nearObject.GetComponent<NPCInterAction>();
                npc.IsTalk = true;
                // �÷��̾� �̵�, ���� �� �׼� ���� ���� �ʿ�
            }
        }
    }

    void Start()
    {
        theSaveNLoad = FindObjectOfType<SaveNLoad>();
    }
    void Update()
    {
        Interaction();

        if (Input.GetKeyDown(KeyCode.F5)) //����
        {

        }
        if (Input.GetKeyDown(KeyCode.F9)) //�ҷ�����
        {

        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("NPC"))
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
