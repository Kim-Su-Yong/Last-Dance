using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestUIManager : MonoBehaviour
{
    public static QuestUIManager uiManager;

    // BOOLS
    public bool questAvailable = false;
    public bool questRunning = false;

    public bool NPCquestPanelActive = false;
    public bool questPanelActive = false;
    public bool questLogPanelActive = false;

    // PANELS
    public GameObject questPanel;       // Player
    public GameObject questLogPanel;    // Player
    public GameObject NPCquestPanel;    // NPC

    // QUEST OBJECT
    private QuestObject currentQuestObject;

    // QUEST LISTS
    public List<Quest> availableQuests = new List<Quest>();
    public List<Quest> activeQuests = new List<Quest>();     // running quest

    // BUTTONS
    public GameObject qButton;          // key for Log Panel Activation
    public GameObject qLogButton;
    public GameObject NPCqButton;       // NPC
    private List<GameObject> qButtons = new List<GameObject>();

    public GameObject objective_Remove;
    public GameObject objective_Add;
    public GameObject reward_Gold;
    public GameObject reward_Item;
    public GameObject reward_Exp;

    // SPACER
    public Transform qButtonSpacer1;    // qButton for available
    public Transform qButtonSpacer2;    // running qButton
    public Transform qLogButtonSpacer;  // running in qLog

    // QUEST INFOS
    public Text questTitle;
    public Text questDescription;
    public Text questSummary;
    public Text questQuota;             // ÇÒ´ç·®
    public Text questReward_gold;
    public Text questReward_Item;
    public Text questReward_EXP;

    public Text questLogTitle;
    public Text questLogDescription;
    public Text questLogSummary;


    private void Awake()
    {
        if (uiManager == null)
        {
            uiManager = this;
        }
        else if (uiManager != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);

    }

    private void Start()
    {
        HideQuestPanel();
        
    }
    void Update()
    {
        PanelQKey();
    }

    void PanelQKey()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            questPanelActive = !questPanelActive;   // switch
            questPanel.SetActive(questPanelActive);
            //ShowQuestLogPanel();
        }
    }

    public void NPCPanelActivation()
    {
        NPCquestPanelActive = true;
        NPCquestPanel.SetActive(NPCquestPanelActive);
        foreach (Quest availableQuest in availableQuests)
        {
            GameObject questButton = Instantiate(qButton);

            // SCRIPT
            QButtonScript qBScript = questButton.GetComponent<QButtonScript>();
            qBScript.questID = availableQuest.id;
            qBScript.questTitle.text = availableQuest.title;    // .text = string
            qBScript.questMarker.sprite = qBScript.marker_AVAILABLE;
            qBScript.questMarker.color = Color.yellow;

            questButton.transform.SetParent(qButtonSpacer1, false);
            qButtons.Add(questButton);
        }

        foreach (Quest activeQuest in activeQuests)
        {
            GameObject questButton = Instantiate(qButton);

            // SCRIPT
            QButtonScript qBScript = questButton.GetComponent<QButtonScript>();
            qBScript.questID = activeQuest.id;
            qBScript.questTitle.text = activeQuest.title;    // .text = string
            qBScript.questMarker.sprite = qBScript.marker_ACCEPTED;
            qBScript.questMarker.color = Color.white;
            if (activeQuest.progress == Quest.QuestProgress.COMPLETE)
            {
                qBScript.questMarker.color = Color.yellow;
            }

            questButton.transform.SetParent(qButtonSpacer2, false);
            qButtons.Add(questButton);
        }
    }

    public void PanelXButton()
    {
        questPanelActive = !questPanelActive;
        questPanel.SetActive(questPanelActive);
    }

    // CALLED FROM QUEST OBJECT
    public void CheckQuest(QuestObject questObject)
    {
        currentQuestObject = questObject;
        QuestManager.questManager.QuestRequest(questObject);
        if ((questRunning || questAvailable) && !questPanelActive)
        {
            ShowQuestPanel();
        }
        else
        {
            Debug.Log("No Quests Available");   // Default talking should be in.
        }
    }

    // SHOW PANEL
    public void ShowQuestPanel()
    {
        questPanelActive = true;
        questPanel.SetActive(questPanelActive);
        // FILL IN DATA
        FillQuestButtons();
    }

    // QUEST LOG

    // HIDE QUEST PANEL
    public void HideQuestPanel()
    {
        questPanelActive = false;
        questAvailable = false;
        questRunning = false;

        // CLEAR TEXT
        questTitle.text = "";
        questDescription.text = "";
        questSummary.text = "";

        // CLEAR LISTS
        availableQuests.Clear();
        activeQuests.Clear();

        // CLEAR BUTTON LISTS
        for (int i = 0; i < qButtons.Count; i++)
        {
            Destroy(qButtons[i]);
        }
        qButtons.Clear();

        //HIDE PANEL
        questPanel.SetActive(questPanelActive);
        questLogPanel.SetActive(questPanelActive);

        objective_Remove.SetActive(false);
        objective_Add.SetActive(false);
        reward_Gold.SetActive(false);
        reward_Item.SetActive(false);
        reward_Exp.SetActive(false);
    }

    // FILL BUTTONS FOR QUEST PANEL
    void FillQuestButtons()             // ¡â I don't need a Layout Group.
    {
        foreach (Quest availableQuest in availableQuests)
        {
            GameObject questButton = Instantiate(qButton);

            // SCRIPT
            QButtonScript qBScript = questButton.GetComponent<QButtonScript>();
            qBScript.questID = availableQuest.id;
            qBScript.questTitle.text = availableQuest.title;    // .text = string
            qBScript.questMarker.sprite = qBScript.marker_AVAILABLE;
            qBScript.questMarker.color = Color.yellow;

            questButton.transform.SetParent(qButtonSpacer1, false);
            qButtons.Add(questButton);
        }

        foreach (Quest activeQuest in activeQuests)
        {
            GameObject questButton = Instantiate(qButton);

            // SCRIPT
            QButtonScript qBScript = questButton.GetComponent<QButtonScript>();
            qBScript.questID = activeQuest.id;
            qBScript.questTitle.text = activeQuest.title;    // .text = string
            qBScript.questMarker.sprite = qBScript.marker_ACCEPTED;
            qBScript.questMarker.color = Color.white;
            if (activeQuest.progress == Quest.QuestProgress.COMPLETE)
            {
                qBScript.questMarker.color = Color.yellow;
            }

            questButton.transform.SetParent(qButtonSpacer2, false);
            qButtons.Add(questButton);
        }
    }

    // SHOW QUEST ON BUTTON PRESS IN QUEST PANEL
    public void ShowSeclectedQuest(int questID)
    {
        questLogPanel.SetActive(true);
        for (int i = 0; i < availableQuests.Count; i++)
        {
            if (availableQuests[i].id == questID)
            {
                questTitle.text = availableQuests[i].title;
                if (availableQuests[i].progress == Quest.QuestProgress.AVAILABLE)
                {
                    // INFOS
                    questDescription.text = availableQuests[i].description;
                    if (availableQuests[i].questObjective_Remove != "")
                    {
                        objective_Remove.SetActive(true);
                        questSummary.text = availableQuests[i].questObjective_Remove + " : ";
                        questQuota.text = availableQuests[i].questObjectiveCount_R + " / " 
                                        + availableQuests[i].questObjectiveRequirement_R;
                    }
                    if (availableQuests[i].questObjective_Add != "")
                    {
                        objective_Add.SetActive(true);
                        questSummary.text = availableQuests[i].questObjective_Add + " : ";
                        questQuota.text = availableQuests[i].questObjectiveCount_R + " / "
                                        + availableQuests[i].questObjectiveRequirement_A;
                    }

                    // REWARDS
                    if (availableQuests[i].goldReward != 0)
                    {
                        reward_Gold.SetActive(true);
                        questReward_gold.text = availableQuests[i].goldReward + "G";
                    }
                    if (availableQuests[i].itemReward != "")
                    {
                        reward_Item.SetActive(true);
                        questReward_Item.text = availableQuests[i].itemReward;
                    }
                    if (availableQuests[i].expReward != 0)
                    {
                        reward_Exp.SetActive(true);
                        questReward_EXP.text = availableQuests[i].expReward + "Exp";
                    }
                }
            }
        }

        for (int i = 0; i < activeQuests.Count; i++)
        {
            if (activeQuests[i].id == questID)
            {
                questTitle.text = activeQuests[i].title;
                if (activeQuests[i].progress == Quest.QuestProgress.ACCEPTED)
                {
                    questDescription.text = activeQuests[i].description;
                    questSummary.text = activeQuests[i].questObjective_Remove + " : ";
                    questQuota.text = activeQuests[i].questObjectiveCount_R + " / " + activeQuests[i].questObjectiveRequirement_R;
                    questReward_gold.text = activeQuests[i].goldReward + "G";
                    questReward_Item.text = activeQuests[i].itemReward;
                    questReward_EXP.text = activeQuests[i].expReward + "Exp";
                }
            }
        }
    }
}
