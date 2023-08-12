using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HUDManager : MonoBehaviour
{
    // stamina bar
    public Slider healthBar;
    public Slider XPBar;
    public TextMeshProUGUI LevelText;
    public TextMeshProUGUI objectiveText;
    public GameObject Map;

    private void Awake()
    {
        PlayerHealthXP healthXP = FindObjectOfType<PlayerHealthXP>();
        healthBar.value = healthXP.playerProperties.Health;
        XPBar.value = healthXP.playerProperties.XP;
        LevelText.text = healthXP.playerProperties.level.ToString();
        NPCScript  npc =  FindObjectOfType<NPCScript>();
        objectiveText.text = npc.quest[npc.currentQuestNumber].objectiveInstruction;
    }
    public void TeleportToOtherVillage(int index)
    {
        GameManager gameManager = FindObjectOfType<GameManager>();
        gameManager.LoadScene(index);
    }

    public void DisplayMap()
    {
        Map.SetActive(true);
    }
}
