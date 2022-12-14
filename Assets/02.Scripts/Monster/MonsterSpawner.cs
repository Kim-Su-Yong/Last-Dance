using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
struct MonsterInfo
{
    [HideInInspector]
    public string MonsterName;
    public GameObject monsterPrefab;
    public MonsterData monsterData;
}
public class MonsterSpawner : MonoBehaviour
{
    public enum MonsterType
    {
        A_Skeleton,
        B_Fishman,
        C_Slime
    }
    [Header("Which monster do you want to Spawn?")]
    public MonsterType monsterType;
    [Header("How many?")]
    public int monsterCount = 5;
    public float CreateTime = 3.0f;

    [SerializeField]
    MonsterInfo[] monsterInfos;

    // List
    public List<Transform> spawnPoints = new List<Transform>();

    private List<GameObject> MonsterA = new List<GameObject>();
    private List<GameObject> MonsterB = new List<GameObject>();
    private List<GameObject> MonsterC = new List<GameObject>();

    // Bool
    public bool isGameOver = false;

    // Script

    private void Awake()
    {
        var points = GetComponent<Transform>();
        points.GetComponentsInChildren<Transform>(spawnPoints);
        spawnPoints.RemoveAt(0);

        // Resource Load
        monsterInfos[0].monsterPrefab = Resources.Load<GameObject>("MonsterData/Skeleton Prefab");
        monsterInfos[1].monsterPrefab = Resources.Load<GameObject>("MonsterData/Fishman Prefab");
        monsterInfos[2].monsterPrefab = Resources.Load<GameObject>("MonsterData/Slime Prefab");

        monsterInfos[0].monsterData = Resources.Load<MonsterData>("MonsterData/Skeleton Data");
        monsterInfos[1].monsterData = Resources.Load<MonsterData>("MonsterData/Fishman Data");
        monsterInfos[2].monsterData = Resources.Load<MonsterData>("MonsterData/Slime Data");
    }

    private void Start()
    {
        CreateMonster();
    }

    private void Update()
    {
        StartCoroutine(RepeatingMonster());
    }

    IEnumerator RepeatingMonster()
    {
        if (isGameOver == true) yield break;
        switch (monsterType)
        {
            case MonsterType.A_Skeleton:
                foreach (GameObject _monster in MonsterA)
                {
                    if (_monster.activeSelf == false)
                    {
                        yield return new WaitForSeconds(3.0f);
                        Transform randomPoint = spawnPoints[UnityEngine.Random.Range(0, spawnPoints.Count)];
                        Transform spawnPoint = randomPoint;
                        _monster.transform.position = randomPoint.position;
                        
                        _monster.transform.position = spawnPoint.position;
                        _monster.SetActive(true);
                        _monster.GetComponent<MonsterAI>().isDie = false;
                        _monster.GetComponent<MonsterAI>().state = MonsterAI.State.PATROL;
                        break;
                    }
                }
                break;
            case MonsterType.B_Fishman:
                foreach (GameObject _monster in MonsterB)
                {
                    if (_monster.activeSelf == false)
                    {
                        yield return new WaitForSeconds(3.0f);
                        Transform pastPosition = _monster.transform;
                        Transform spawnPoint = pastPosition;

                        if (spawnPoint == pastPosition)
                        {
                            spawnPoint = spawnPoints[UnityEngine.Random.Range(0, spawnPoints.Count)];
                            _monster.transform.position = spawnPoint.position;
                        }

                        _monster.transform.position = spawnPoint.position;
                        _monster.SetActive(true);
                        _monster.GetComponent<MonsterAI>().isDie = false;
                        _monster.GetComponent<MonsterAI>().state = MonsterAI.State.PATROL;
                        break;
                    }
                }
                break;
            case MonsterType.C_Slime:
                foreach (GameObject _monster in MonsterC)
                {
                    if (_monster.activeSelf == false)
                    {
                        yield return new WaitForSeconds(3.0f);
                        Transform pastPosition = _monster.transform;
                        Transform spawnPoint = pastPosition;

                        if (spawnPoint == pastPosition)
                        {
                            spawnPoint = spawnPoints[UnityEngine.Random.Range(0, spawnPoints.Count)];
                            _monster.transform.position = spawnPoint.position;
                        }
                        _monster.SetActive(true);
                        _monster.GetComponent<MonsterAI>().isDie = false;
                        _monster.GetComponent<MonsterAI>().state = MonsterAI.State.PATROL;
                        break;
                    }
                }
                break;
        }

    }
    private void CreateMonster()
    {
        switch (monsterType)
        {
            case MonsterType.A_Skeleton:
                StartCoroutine(CreateMonster_A());
                break;
            case MonsterType.B_Fishman:
                StartCoroutine(CreateMonster_B());
                break;
            case MonsterType.C_Slime:
                StartCoroutine(CreateMonster_C());
                break;
        }
    }

    IEnumerator CreateMonster_A()
    {
        GameObject A_obj = new GameObject("A_Skeleton-Pool");
        for (int i = 0; i < monsterCount; i++)
        {
            yield return new WaitForSeconds(CreateTime);
            
            Transform spawnPoint = spawnPoints[UnityEngine.Random.Range(0, spawnPoints.Count)];
            var A_monster = Instantiate(monsterInfos[0].monsterPrefab, spawnPoint.position, spawnPoint.rotation, A_obj.transform);
            A_monster.GetComponent<MonsterAI>().SetUp(monsterInfos[0].monsterData);
            A_monster.GetComponent<MonsterAI>().LetMeKnowMonsterType(0);
            A_monster.name = "A_Skeleton" + i.ToString();
            A_monster.SetActive(true);
            MonsterA.Add(A_monster);
        }
    }

    IEnumerator CreateMonster_B()
    {
        GameObject B_obj = new GameObject("B_Fishman-Pool");
        for (int i = 0; i < monsterCount; i++)
        {
            yield return new WaitForSeconds(CreateTime);

            Transform spawnPoint = spawnPoints[UnityEngine.Random.Range(0, spawnPoints.Count)];
            var B_monster = Instantiate(monsterInfos[1].monsterPrefab, spawnPoint.position, spawnPoint.rotation, B_obj.transform);
            B_monster.name = "B_Fishman" + i.ToString();
            B_monster.GetComponent<MonsterAI>().SetUp(monsterInfos[1].monsterData);
            B_monster.GetComponent<MonsterAI>().LetMeKnowMonsterType(1);
            B_monster.SetActive(true);
            MonsterB.Add(B_monster);
        }
    }

    IEnumerator CreateMonster_C()
    {
        GameObject C_obj = new GameObject("C_Slime-Pool");
        for (int i = 0; i < monsterCount; i++)
        {
            yield return new WaitForSeconds(CreateTime);

            Transform spawnPoint = spawnPoints[UnityEngine.Random.Range(0, spawnPoints.Count)];
            var C_monster = Instantiate(monsterInfos[2].monsterPrefab, spawnPoint.position, spawnPoint.rotation, C_obj.transform);
            C_monster.name = "C_Slime" + i.ToString();
            C_monster.GetComponent<MonsterAI>().SetUp(monsterInfos[2].monsterData);
            C_monster.GetComponent<MonsterAI>().LetMeKnowMonsterType(2);
            C_monster.SetActive(true);
            MonsterA.Add(C_monster);
        }
    }
}
