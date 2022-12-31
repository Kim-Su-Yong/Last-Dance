using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[CreateAssetMenu(menuName = "Scriptable/QuestData", fileName ="Quest Data")]
[System.Serializable]
public class Quest// : ScriptableObject
{
    public enum QuestProgress { NOT_AVAILABLE, AVAILABLE, ACCEPTED, COMPLETE, DONE }

    public string title;            // title of the quest
    public int id;                  // ID number for the quest
    public QuestProgress progress;  // state of the current quest (enum)
    public string description;      // string from our quest Giver/Receiver
    public string hint;             // string from our quest Giver/Receiver
    public string congratulation;   // string from our quest Giver/Receiver
    public string summary;          // string from our quest Giver/Receiver
    public int nextQuest;           // the next quest, if there is any (chain quest)

    // REMOVE MONSTERS
    public string questObjective_Remove;        // name of the quest objective(also for remove monsters)
    public int questObjectiveCount_R;           // current number of quest objective Removement count
    public int questObjectiveRequirement_R;     // required amount of quest objective Removement count

    // ADD ITEMS
    public string questObjective_Add;           // name of the quest objective(also for remove monsters)
    public int questObjectiveCount_A;           // current number of quest objective Add Item count
    public int questObjectiveRequirement_A;     // required amount of quest objective Add Item count

    // REWARD
    public int expReward;
    public int goldReward;
    public string itemReward;
}
