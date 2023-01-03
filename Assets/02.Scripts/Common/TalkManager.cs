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
        talkData.Add(1001 ,new string[] { "�ȳ�? �� �糪����", "���� �Ϸ� ã�ƿ��̳���?" });
        talkData.Add(1 ,new string[] { "�ȳ��ϼ���? �÷��̾��", "���� ������ óġ���ּ���!", "������ �˳��� �帮�ڽ��ϴ�." });
    }

    public string GetTalk(int id, int talkIndex)
    {
        // ��ȭ�� ���� ������ ���Ͽ� ���� Ȯ��
        if (talkIndex == talkData[id].Length)    
            
            return null;
        // ������ �������� �Ѱ���
        else
            return talkData[id][talkIndex];

    }
    
}
