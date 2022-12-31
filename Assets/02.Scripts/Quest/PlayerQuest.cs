using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerQuest : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        QuestManager.questManager.AddItemQuest("¹ö¼¸ ±«¹° Ã³Ä¡", 5);
    }
}
