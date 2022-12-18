using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDrop : MonoBehaviour
{
    public GameObject[] Items; //죽으면 떨어질 아이템 오브젝트

    private Transform tr; //아이템이 떨어질 위치
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
        if (!isDrop) //한번만 실행 되도록
            dropItem();
    }

    void dropItem()
    {
        if (monsterAI.isDie) //몬스터가 죽으면 실행
        {
            Debug.Log("아이템 생성");
            GameObject item = (GameObject)Instantiate(Items[Random.Range(0, Items.Length)], new Vector3(tr.position.x, tr.position.y + 0.5f, tr.position.z), Quaternion.identity);
            isDrop = true;
        }
    }
}
