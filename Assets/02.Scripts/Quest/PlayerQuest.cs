using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerQuest : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        QuestManager.questManager.AddItemQuest("���� ���� óġ", 5);
    }
}
