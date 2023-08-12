using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthXP  : MonoBehaviour
{
    // responsible to store all the player properties like health XP and level.
    // Handles Levelling Up Mechanism
    // Persistent class acctached to GameManager gameObject that doesn't destroy on load.
    // Data remains contant throught all the game scenes and is reset at restart.

    public static PlayerHealthXP Instance { get; private set; }
    GameManager gameManager;

    // will store all the relevant player properties
    public struct PlayerProperties
    {
        public float Health;
        public float XP;
        public int level;
    }

    public PlayerProperties playerProperties;


    [Header("Max Values")]
    public float maxHealth;
    public float maxXP;

    private float initialLevel;

    [Header("Increment Factor")]
    public float XPFactor = 5;

    public void Awake()
    {
        Instance = this;
        gameManager = this.gameObject.GetComponent<GameManager>();
        initialLevel = 1;      
        playerProperties.Health = maxHealth;
    }

    public void UpdateXP(float xPoints)
    {

        if (playerProperties.XP < 100)
        {
            playerProperties.XP += xPoints;
            FindObjectOfType<HUDManager>().XPBar.value = playerProperties.XP;
        }
        else LevelUp();
    }

    private void LevelUp()
    {
        playerProperties.XP = 0;
        playerProperties.level += 1;
        FindObjectOfType<HUDManager>().XPBar.value = playerProperties.XP;
        FindObjectOfType<HUDManager>().LevelText.text = playerProperties.level.ToString();
        gameManager.playerStats.agility += 1;
        gameManager.playerStats.strength += 1;
        gameManager.playerStats.stamina += 1;
        gameManager.playerStats.luck += 1;
        gameManager.playerStats.magic += 1;
    }

    public void IncreaseHealth(int extraHealth)
    {

        playerProperties.Health += extraHealth;
        if(playerProperties.Health > 100)
        {
            playerProperties.Health = 100;
        }
        FindObjectOfType<HUDManager>().healthBar.value = playerProperties.Health;
    }


}
