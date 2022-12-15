using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDrop : MonoBehaviour
{
    public GameObject[] Items; //������ ������ ������ ������Ʈ

    private Transform tr; //�������� ������ ��ġ
    private MonsterAI monsterAI;

    public bool isDrop = false;


    void Start()
    {
        Items = Resources.LoadAll<GameObject>("Items");
        monsterAI = GetComponent<MonsterAI>();
        tr = GetComponent<Transform>();
    }

    
    void Update()
    {
        if (!isDrop) //�ѹ��� ���� �ǵ���
            dropItem();
    }

    void dropItem()
    {
        if (monsterAI.isDie) //���Ͱ� ������ ����
        {
            Debug.Log("������ ����");
            GameObject item = (GameObject)Instantiate(Items[Random.Range(0, Items.Length)], new Vector3(tr.position.x, tr.position.y + 0.5f, tr.position.z), Quaternion.identity);
            isDrop = true;
        }
    }
}
