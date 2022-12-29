using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QButtonScript : MonoBehaviour
{
    public int questID;
    public Text questTitle;
    [HideInInspector]public Image questMarker;
    [HideInInspector]public Sprite marker_AVAILABLE;
    [HideInInspector]public Sprite marker_ACCEPTED;

    [SerializeField] private GameObject acceptButton;
    [SerializeField] private GameObject giveUpButton;
    [SerializeField] private GameObject completeButton;

    private QButtonScript acceptButtonScript;
    private QButtonScript giveUpButtonScript;
    private QButtonScript completeButtonScript;

    private void Awake()
    {
        acceptButton = GameObject.Find("Canvas_UI").transform.Find("Quest-UI").transform.
                                  Find("questLog_window").transform.Find("quest_info").transform.
                                  Find("Contents_area").transform.Find("Button_Layout").transform.
                                  Find("Button_Accept").transform.gameObject;
        acceptButtonScript = acceptButton.GetComponent<QButtonScript>();

        giveUpButton = GameObject.Find("Canvas_UI").transform.Find("Quest-UI").transform.
                                  Find("questLog_window").transform.Find("quest_info").transform.
                                  Find("Contents_area").transform.Find("Button_Layout").transform.
                                  Find("Button_GiveUp").transform.gameObject;
        giveUpButtonScript = giveUpButton.GetComponent<QButtonScript>();

        completeButton = GameObject.Find("Canvas_UI").transform.Find("Quest-UI").transform.
                                    Find("questLog_window").transform.Find("quest_info").transform.
                                    Find("Contents_area").transform.Find("Button_Layout").transform.
                                    Find("Button_Complete").transform.gameObject;
        completeButtonScript = completeButton.GetComponent<QButtonScript>();
    }

    // SHOW ALL INFOS
    public void ShowAllInfos()  // QButton Prefab runs this method.
    {
        QuestUIManager.uiManager.ShowSeclectedQuest(questID);
        // ACCEPT BUTTON
        if (QuestManager.questManager.RequestAvailableQuest(questID))
        {
            acceptButton.SetActive(true);
            acceptButtonScript.questID = questID;
        }
        else
        {
            acceptButton.SetActive(false);
        }
        // GIVE UP BUTTON
        if (QuestManager.questManager.RequestAcceptedQuest(questID))
        {
            giveUpButton.SetActive(true);
            giveUpButtonScript.questID = questID;
        }
        else
        {
            giveUpButton.SetActive(false);
        }
        // COMPLETE BUTTON
        if (QuestManager.questManager.RequestCompletedQuest(questID))
        {
            completeButton.SetActive(true);
            completeButtonScript.questID = questID;
        }
        else
        {
            completeButton.SetActive(false);
        }
    }

    // PRESS LOG PANEL BUTTONS
    public void AcceptQuest()
    {
        QuestManager.questManager.AcceptQuest(questID);
        QuestUIManager.uiManager.HideQuestPanel();
    }

    public void GiveUpQuest()
    {

    }
    public void CompleteQuest()
    {

    }
}
