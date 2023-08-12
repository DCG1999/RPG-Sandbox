using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestSystem
{
    public  int currentMainQuestNumber = 0;
    public  bool QuestActive = false;
    public  List<Quest> MainQuestList = new List<Quest>();
    public  List<Quest> SideQuestList = new List<Quest>();

    GameManager gameManager;

    public bool CheckForCollectedItems()
    {
        gameManager = GameObject.FindObjectOfType<GameManager>();
        foreach(Item item in gameManager.inventorySytem.inventory)
        {
            if (item.itemData.displayName == MainQuestList[currentMainQuestNumber].objective)
            {
                if (item.stackSize == MainQuestList[currentMainQuestNumber].objectiveCount)
                {
                    Debug.Log("collected");
                    return true;
                }
                else return false;
            }
            else return false;
        }
        return false;
    }

    public bool CheckForEnemiesKilled()
    {
        int objectiveEnemyCounter = 0;
        gameManager = GameObject.FindObjectOfType<GameManager>();
        foreach (string enemy in gameManager.deadEnemies)
        {
            if (enemy.Contains(MainQuestList[currentMainQuestNumber].objective))
            {
                objectiveEnemyCounter++;
            }
        }

        if (objectiveEnemyCounter >= MainQuestList[currentMainQuestNumber].objectiveCount)
        {
            return true;
        }
        else return false;
    }

    public void UpdateQuestObjective()
    {
        GameObject.FindObjectOfType<HUDManager>().objectiveText.text = MainQuestList[currentMainQuestNumber].objectiveInstruction;
    }

    public void ResetQuestObjective() => GameObject.FindObjectOfType<HUDManager>().objectiveText.text = "";
}

[System.Serializable]
public class Quest
{

    public int QuestNumber;

    public enum QuestType {  MAIN, SIDE};
    public QuestType questType;

    public enum QuestObjective { COLLECT, KILL };
    public QuestObjective questObjective;

    public string objective;
    public string objectiveInstruction;
    public float objectiveCount;
    [HideInInspector] public bool completed = false;
    [HideInInspector] public bool activated;

    public QuestDialogue questDialogue;

}

[System.Serializable]
public class QuestDialogue
{
    public string questInitDialogue;
    public string questActivatedDialogue;
    public string questCompleteDialogue;

}
