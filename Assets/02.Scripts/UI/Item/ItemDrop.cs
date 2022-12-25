using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDrop : MonoBehaviour
{
    public GameObject[] ConsumeItems; //������ ������ ������ ������Ʈ
    public GameObject[] EquipItems; //������ ������ ������ ������Ʈ
    public GameObject[] ETCItems; //������ ������ ������ ������Ʈ

    private Transform tr; //�������� ������ ��ġ
    private MonsterAI monsterAI;

    public bool isDrop = false;

    void Start()
    {
        ConsumeItems = Resources.LoadAll<GameObject>("ItemPrefabs/Consume");
        EquipItems = Resources.LoadAll<GameObject>("ItemPrefabs/Equip");
        ETCItems = Resources.LoadAll<GameObject>("ItemPrefabs/ETC");
        monsterAI = GetComponent<MonsterAI>();
        tr = GetComponent<Transform>();
    }


    void Update()
    {
        if (!isDrop) //�ѹ��� ���� �ǵ���
            itemDrop();
        //dropItem();
        
    }

    void dropItem()
    {
        if (monsterAI.isDie) //���Ͱ� ������ ����
        {
            Debug.Log("������ ����");
            GameObject item = (GameObject)Instantiate(ConsumeItems[Random.Range(0, ConsumeItems.Length)], new Vector3(tr.position.x, tr.position.y + 0.5f, tr.position.z), Quaternion.identity);
            isDrop = true;
            //�ܼ��� ���ҽ��� ���� �� ������ ������ �޾Ƽ� �������� �����ǵ��� �Ǿ��ִ�.(Ȯ�� ����)
        }

    }
    

    #region Ȯ�� ���� �Լ�
    public void itemDrop()
    {
        Choose(new float[3] { 35f, 15f, 50f });
        float Choose(float[] probs)
        {
            float total = 0;
            foreach (float elem in probs)
            {
                total += elem;
            }
            float randomPoint = Random.value * total;
            for (int i = 0; i < probs.Length; i++)
            {
                if (randomPoint < probs[i] && monsterAI.isDie)
                {
                    switch(i)
                    {
                        case 0:
                            Debug.Log("�Һ������ ����");
                            GameObject consumeitem = (GameObject)Instantiate(ConsumeItems[Random.Range(0, ConsumeItems.Length)],
                                new Vector3(tr.position.x, tr.position.y + 0.5f, tr.position.z), Quaternion.identity);
                            isDrop = true;
                            break;
                        case 1:
                            Debug.Log("�������� ����");
                            GameObject equipitem = (GameObject)Instantiate(EquipItems[Random.Range(0, EquipItems.Length)],
                                new Vector3(tr.position.x, tr.position.y + 0.5f, tr.position.z), Quaternion.identity);
                            isDrop = true;
                            break;
                        case 2:
                            Debug.Log("��Ÿ������ ����");
                            GameObject etcitem = (GameObject)Instantiate(ETCItems[Random.Range(0, ETCItems.Length)],
                                new Vector3(tr.position.x, tr.position.y + 0.5f, tr.position.z), Quaternion.identity);
                            isDrop = true;
                            break;
                    }
                    return i;
                }
                else
                {
                    randomPoint -= probs[i];
                }
            }
            return probs.Length - 1;
        }
        //������ return ���� �ʿ��� ������ Random.value�� ��ȯ ����� 1�� �� �ֱ� �����̴�.
        //�� ��� �˻����� ������ ���� ��𿡼��� ã�� �� ����.
        //���� �� �۰ų� ���� �׽�Ʈ�� �����ϸ� �߰� return ���� ���������� Ȯ���� 0�� �׸��� ���������� ���õ� �� �ִ�.
    }

    #endregion

}
