using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCScript : MonoBehaviour
{
    GameManager gameManager;
    public int currentQuestNumber;
    public Quest[] quest;
    [HideInInspector] public QuestSystem questSystem;
    DialogueSystem dialogueSystem;

    public string normalDialogue;

    bool hasQuest;

    public GameObject questMark;

    void Start()
    {
        if (quest.Length > 0)
        {
            questMark.SetActive(true);
            hasQuest = true;
        }
        else
        {
            hasQuest = false;
            questMark.SetActive(false);
        }

        gameManager = FindObjectOfType<GameManager>();
        questSystem = gameManager.questSystem;
        dialogueSystem = gameManager.dialogueSystem;
        currentQuestNumber = questSystem.currentMainQuestNumber;  
        
        if(questSystem.QuestActive)
        {
            questSystem.UpdateQuestObjective();
        }    
    }

    public void Interact()
    {
        if (currentQuestNumber >= quest.Length)
        {
            hasQuest = false;
            questMark.SetActive(false);
        }

        if (hasQuest)
        {

            if (!questSystem.QuestActive)
            {
                dialogueSystem.PlayDialogue(quest[currentQuestNumber].questDialogue.questInitDialogue);
                ActivateQuest();
            }
            else
            {
                CheckForCompletion();
                if (quest[currentQuestNumber].completed)
                {
                    if(currentQuestNumber == quest.Length-1) questMark.SetActive(false);

                    dialogueSystem.PlayDialogue(quest[currentQuestNumber].questDialogue.questCompleteDialogue);
                    questSystem.currentMainQuestNumber ++;
                    
                    currentQuestNumber = questSystem.currentMainQuestNumber;
                }
                else dialogueSystem.PlayDialogue(quest[currentQuestNumber].questDialogue.questActivatedDialogue);
            }
        }
        else dialogueSystem.PlayDialogue(normalDialogue);
        

    }

    public void ActivateQuest()
    {
        if (!questSystem.QuestActive)
        {   
            currentQuestNumber = questSystem.currentMainQuestNumber;
            questSystem.MainQuestList.Add(quest[currentQuestNumber]);
            quest[currentQuestNumber].activated = true;
            questSystem.QuestActive = true;
            questSystem.UpdateQuestObjective();
        }
    }

    public void CheckForCompletion()
    {
        print(currentQuestNumber);
        if (quest[currentQuestNumber].questObjective == Quest.QuestObjective.COLLECT)
        {
            if (questSystem.CheckForCollectedItems())
            {
                questSystem.QuestActive = false;
                quest[currentQuestNumber].completed = true;
                questSystem.ResetQuestObjective();
            }
        }

        if (quest[currentQuestNumber].questObjective == Quest.QuestObjective.KILL)
        {
            if (questSystem.CheckForEnemiesKilled())
            {
                questSystem.QuestActive = false;
                quest[currentQuestNumber].completed = true;
                questSystem.ResetQuestObjective();
            }
        }
    }
}
