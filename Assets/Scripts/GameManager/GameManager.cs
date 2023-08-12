using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private CharacterClasses playerClass;
    [HideInInspector]public QuestSystem questSystem;
    [HideInInspector]public DialogueSystem dialogueSystem;
    [HideInInspector]public InventorySystem inventorySytem;
    private GameObject inventory;

    private GameObject player;

    int sceneID = 0; 

    public enum GameStates { MENU, CHARACTERSELECTION, GAME, PAUSE, WIN, LOSE, INVENTORY};
    private static GameStates GameState;
    public static GameStates gameState { get { return GameState; } }

    public struct PlayerStats
    {
        public float agility;
        public float stamina;
        public float strength;
        public float magic;
        public float luck;
    }

    public PlayerStats playerStats;

    [HideInInspector]public List<ItemData> equippedItems;
    [HideInInspector]public List<string> deadEnemies;

    public void Awake()
    {
        dialogueSystem = GameObject.Instantiate(Resources.Load( "Dialogue/DialogueCanvas") as GameObject).GetComponent<DialogueSystem>();
        dialogueSystem.gameObject.SetActive(false);
        DontDestroyOnLoad(dialogueSystem.gameObject);
        questSystem = new QuestSystem();
        inventorySytem = new InventorySystem();
        equippedItems = new List<ItemData>();
        deadEnemies = new List<string>();
        GameState = GameStates.MENU;
    }

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void NextScene()
    {
        sceneID++;
        Debug.Log("SceneID : " + sceneID);
        SceneManager.LoadScene(sceneID);
        ChangeGameState();
    }

    public void LoadSameScene()
    {
        SceneManager.LoadScene(sceneID);
        ChangeGameState();
    }

    public void LoadScene(int _sceneID)
    {
        sceneID = _sceneID;
        SceneManager.LoadScene(sceneID);
        ChangeGameState();
    }

    void ChangeGameState()
    {
        switch(sceneID)
        {
            case 0: UpdateGameStateFunctions(GameStates.MENU); break;
            case 1: UpdateGameStateFunctions(GameStates.CHARACTERSELECTION); break;  
                // add win lose cases after number of scenes are decided.
            default:
                UpdateGameStateFunctions(GameStates.GAME);
                break;
        }
            
    }

    public void UpdateGameStateFunctions (GameStates _gameState)
    {
        GameState = _gameState;

        switch(GameState)
        {
            case GameStates.MENU:
                break;
            case GameStates.CHARACTERSELECTION: break;
            case GameStates.GAME: break;
            case GameStates.INVENTORY: break;
            case GameStates.WIN: break;
            case GameStates.LOSE:
                Invoke("LoseGamePopup", 5f);
                break;
            case GameStates.PAUSE:
                Time.timeScale = 0;
                break;

        }
    }
    public void LoseGamePopup()
    {

        GameObject loseGamePopup = GameObject.Instantiate(Resources.Load("LoseScreen/LoseScreen") as GameObject);
        loseGamePopup.SetActive(true);
    }
   
    
    public void SetCharacterClass(CharacterClasses _selectedCharacter)
    {
        playerClass = _selectedCharacter;
    }

    public void SpawnPlayer(Transform _t_spawn)
    {
        player = Instantiate(playerClass.playerPrefab);
        player.transform.position = _t_spawn.position;
        player.transform.rotation = _t_spawn.rotation;
        player.gameObject.name = playerClass.name;
        player.tag = "Player";

        player.GetComponent<PlayerStateManager>().InitializePlayer();
        InitializeStats();
    }

    public void DisplayInventory(bool _display)
    {
        if (_display)
        {
            if (inventory == null)
            {
                inventory = Instantiate(Resources.Load("Inventory/Inventory")) as GameObject;
                DontDestroyOnLoad(inventory);
            }
        }
        inventorySytem.UpdateInventory(inventory, _display);
        inventory.SetActive(_display);
    }

    public void InitializeStats()
    {
        playerStats.strength = playerClass.strength;
        playerStats.stamina = playerClass.stamina;
        playerStats.agility = playerClass.agilty;
        playerStats.magic = playerClass.magic;
        playerStats.luck = playerClass.luck;

        Debug.Log("str : " + playerStats.strength);
    }

    public void UpdateStats(Item item, bool setMode)
    {
        if (setMode)
        {
            foreach (StatToEnhance s in item.itemData.statToEnhance)
            {
                switch (s.statToEnhance)
                {
                    case StatToEnhance.stat.AGILITY:
                        playerStats.agility += s.statEnhance;
                            break;
                    case StatToEnhance.stat.STRENGHT:
                        playerStats.strength += s.statEnhance;
                        break;
                    case StatToEnhance.stat.LUCK:
                        playerStats.luck += s.statEnhance;
                        break;
                    case StatToEnhance.stat.MAGIC:
                        playerStats.magic += s.statEnhance;
                        break;
                }
            }
        }

        else
        {
            foreach (StatToEnhance s in item.itemData.statToEnhance)
            {
                switch (s.statToEnhance)
                {
                    case StatToEnhance.stat.AGILITY:
                        playerStats.agility -= s.statEnhance;
                        break;
                    case StatToEnhance.stat.STRENGHT:
                        playerStats.strength -= s.statEnhance;
                        break;
                    case StatToEnhance.stat.LUCK:
                        playerStats.luck -= s.statEnhance;
                        break;
                    case StatToEnhance.stat.MAGIC:
                        playerStats.magic -= s.statEnhance;
                        break;
                }
            }
        }
       
    }
}


