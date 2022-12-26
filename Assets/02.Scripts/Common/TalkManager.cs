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
        talkData.Add(1001 ,new string[] { "�ȳ�? �� �糪����", "�����Ϸ� ã�ƿ��̳���?" });
    }

    public string GetTalk(int id, int talkIndex)
    {
        return talkData[id][talkIndex];

    }
    
}
