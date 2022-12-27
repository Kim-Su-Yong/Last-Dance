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
        talkData.Add(1001 ,new string[] { "안녕? 전 루나에요", "무슨 일로 찾아오셨나요?" });
    }

    public string GetTalk(int id, int talkIndex)
    {
        // 대화의 문장 개수를 비교하여 끝을 확인
        if (talkIndex == talkData[id].Length)    
            return null;
        // 문장이 남았으니 넘겨줌
        else
            return talkData[id][talkIndex];

    }
    
}
