using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAction : MonoBehaviour
{
    [SerializeField]
    GameObject nearObject;  // ĳ���Ϳ� ���� ����� ������Ʈ�� ����
    PlayerState playerState;
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

    private void Awake()
    {
        playerState = GetComponent<PlayerState>();
    }

    private void OnEnable()
    {
        //StartCoroutine(FindNearObject());
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

    IEnumerator FindNearObject()
    {
        //0.3�ʸ��� 
        while(playerState.state != PlayerState.State.DIE)
        {
            yield return new WaitForSeconds(0.3f);
            // 0.3�ʸ��� 20���� �ȿ� �ִ� �ݶ��̴����� ã�´�
            Collider[] Cols = Physics.OverlapSphere(transform.position, 20f);

            foreach (Collider col in Cols)
            {
                if (col.CompareTag("ENEMY"))
                    nearObject = col.gameObject;
            }
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
