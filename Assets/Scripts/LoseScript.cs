using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoseScript : MonoBehaviour
{
    GameManager gameManager;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RestartGame()
    {

        gameManager.LoadScene(2);
        gameManager.questSystem.MainQuestList.Clear();
        gameManager.questSystem.currentMainQuestNumber = 0;
        gameManager.inventorySytem.inventory.Clear();
        gameManager.inventorySytem.itemDictionary.Clear();
        PlayerHealthXP.Instance.playerProperties.Health = 100;
        PlayerHealthXP.Instance.playerProperties.XP = 0;
        PlayerHealthXP.Instance.playerProperties.level = 1;
    }

    public void GoToMainMenu()
    {
        GameObject.Destroy(gameManager.dialogueSystem.gameObject);
        GameObject.Destroy(gameManager.gameObject);
        SceneManager.LoadScene(0);
    }
}
