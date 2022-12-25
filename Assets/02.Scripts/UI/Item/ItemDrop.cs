using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDrop : MonoBehaviour
{
    public GameObject[] ConsumeItems; //죽으면 떨어질 아이템 오브젝트
    public GameObject[] EquipItems; //죽으면 떨어질 아이템 오브젝트
    public GameObject[] ETCItems; //죽으면 떨어질 아이템 오브젝트

    private Transform tr; //아이템이 떨어질 위치
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
        if (!isDrop) //한번만 실행 되도록
            itemDrop();
        //dropItem();
        
    }

    void dropItem()
    {
        if (monsterAI.isDie) //몬스터가 죽으면 실행
        {
            Debug.Log("아이템 생성");
            GameObject item = (GameObject)Instantiate(ConsumeItems[Random.Range(0, ConsumeItems.Length)], new Vector3(tr.position.x, tr.position.y + 0.5f, tr.position.z), Quaternion.identity);
            isDrop = true;
            //단순히 리소스에 저장 된 아이템 정보를 받아서 랜덤으로 생성되도록 되어있다.(확률 동일)
        }

    }
    

    #region 확률 조정 함수
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
                            Debug.Log("소비아이템 생성");
                            GameObject consumeitem = (GameObject)Instantiate(ConsumeItems[Random.Range(0, ConsumeItems.Length)],
                                new Vector3(tr.position.x, tr.position.y + 0.5f, tr.position.z), Quaternion.identity);
                            isDrop = true;
                            break;
                        case 1:
                            Debug.Log("장비아이템 생성");
                            GameObject equipitem = (GameObject)Instantiate(EquipItems[Random.Range(0, EquipItems.Length)],
                                new Vector3(tr.position.x, tr.position.y + 0.5f, tr.position.z), Quaternion.identity);
                            isDrop = true;
                            break;
                        case 2:
                            Debug.Log("기타아이템 생성");
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
        //마지막 return 문이 필요한 이유는 Random.value의 반환 결과가 1일 수 있기 때문이다.
        //이 경우 검색에서 임의의 점을 어디에서도 찾을 수 없다.
        //줄을 더 작거나 같음 테스트로 변경하면 추가 return 문이 방지되지만 확률이 0인 항목이 간헐적으로 선택될 수 있다.
    }

    #endregion

}
