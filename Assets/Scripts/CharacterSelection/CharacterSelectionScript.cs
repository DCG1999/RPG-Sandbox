using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CharacterSelectionScript : MonoBehaviour
{
    GameManager gameManager;
    [SerializeField] private List<GameObject> characterClasses;

    private GameObject lastSelectedCharacter;

    [SerializeField] TextMeshProUGUI nameText;
    [SerializeField] TextMeshProUGUI descriptionText;
    [SerializeField] TextMeshProUGUI strengthText;
    [SerializeField] TextMeshProUGUI agilityText;
    [SerializeField] TextMeshProUGUI staminaText;
    [SerializeField] TextMeshProUGUI magicText;
    [SerializeField] TextMeshProUGUI luckText;

    Button confirmButton;
    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();

        confirmButton = GameObject.Find("Confirm_Btn").GetComponent<Button>();
        confirmButton.onClick.AddListener(gameManager.NextScene);
        confirmButton.gameObject.SetActive(false);

    }
    public void LoadCharacterClassInfo(GameObject charclass)
    {

        if (lastSelectedCharacter)
        {
            lastSelectedCharacter.SetActive(false);
        }
        charclass.SetActive(true);
        lastSelectedCharacter = charclass;
        
        DisplayCharacterInfo(charclass);
        
    }

    public void DisplayCharacterInfo(GameObject _selectedcharacter)
    {
        CharacterClasses selectedCharacterInfo = _selectedcharacter.GetComponent<PlayerStateManager>().characterClass;
        nameText.text = selectedCharacterInfo.Class.ToString();
        descriptionText.text = selectedCharacterInfo.description;
        strengthText.text = "Strength : " + selectedCharacterInfo.strength.ToString();
        agilityText.text = "Agility : " + selectedCharacterInfo.agilty.ToString();
        staminaText.text = "Stamina : " + selectedCharacterInfo.stamina.ToString();
        luckText.text = "Luck : " + selectedCharacterInfo.luck.ToString();
        magicText.text = "Magic : " + selectedCharacterInfo.magic.ToString();

        gameManager.SetCharacterClass(selectedCharacterInfo);
    }
}
