using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
// Script that NPC can have (only have QuestID number)
public class QuestObject : MonoBehaviour
{
    private bool inTrigger = false;

    public List<int> availableQuestIDs = new List<int>();   // can get it
    public List<int> receivableQuestIDs = new List<int>();  // if conditions are met

    private readonly string playerTag = "Player";

    // UI
    public GameObject questMarker;
    public Image theImage;                  // 적용할 이미지
    public Sprite questAvailableSprite;     // ! (yellow)
    public Sprite questReceivableSprite;    // ? (gray) / Complete = ? (yellow)
    

    void Start()
    {
        SetQuestMarker();
    }

    void SetQuestMarker()
    {
        if (QuestManager.questManager.CheckCompleteQuests(this))
        {
            questMarker.SetActive(true);
            theImage.sprite = questReceivableSprite;     // ?
            theImage.color = Color.yellow;
        }
        else if (QuestManager.questManager.CheckAvailableQuests(this))
        {
            questMarker.SetActive(true);
            theImage.sprite = questAvailableSprite; // !
            theImage.color = Color.yellow;
        }
        else if (QuestManager.questManager.CheckAcceptedQuests(this))
        {
            questMarker.SetActive(true);
            theImage.sprite = questReceivableSprite;     // ! (gray)
            theImage.color = Color.gray;
        }
        else
        {
            questMarker.SetActive(false);
        }
    }

    void Update()
    {
        SetQuestMarker();
        if (inTrigger && Input.GetKeyDown(KeyCode.Space))
        {
            // quest ui manager (question marker)
            QuestUIManager.uiManager.CheckQuest(this);
            //QuestManager.questManager.QuestRequest(this);

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == playerTag)
        {
            inTrigger = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == playerTag)
        {
            inTrigger = false;
        }
    }
}
