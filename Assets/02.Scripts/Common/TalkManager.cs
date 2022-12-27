using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkManager : MonoBehaviour
{
    Dictionary<int, string[]> talkData;
    void Awake()
    {
        talkData = new Dictionary<int, string[]>();
        GenerateData();
    }


    void GenerateData()
    {
        talkData.Add(1001 ,new string[] { "안녕? 전 루나에요", "무슨일로 찾아오셨나요?" });
    }

    public string GetTalk(int id, int talkIndex)
    {
        return talkData[id][talkIndex];

    }
    
}
