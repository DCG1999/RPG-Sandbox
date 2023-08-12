using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueSystem : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI dialogueText;
    string dialogueToPlay;
    public bool doneTyping;
    public float typeSpeed;

    // Start is called before the first frame update
    void Start()
    {
        doneTyping = false;
        dialogueText.text = "";
    }

    public void PlayDialogue(string _dialogueToPlay)
    {
        ResetDialogue();
        doneTyping = false;
        dialogueToPlay = _dialogueToPlay;
        StartCoroutine(TypeDialogue());
    }

    public void ResetDialogue() => dialogueText.text = "";

    IEnumerator TypeDialogue()
    {
        foreach(char c in dialogueToPlay.ToCharArray())
        {
            dialogueText.text += c;
            yield return new WaitForSeconds(typeSpeed);
        }
        doneTyping = true;
    }
}
