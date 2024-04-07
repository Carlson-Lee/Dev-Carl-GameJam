using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueTrigger : MonoBehaviour
{
    public DialogueManager dialogueManager; // Reference to the DialogueManager script
    public DialogueDataStruct dialogueDataStruct;
    public GameObject dialogueMenuPrefab; 
    public int dialogueIndexToTrigger = 0; //Index of the dialogue

    private bool dialogueTriggered = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !dialogueTriggered)
        {
            GameObject dialogueMenu = Instantiate(dialogueMenuPrefab);
            StartCoroutine(dialogueManager.DisplayDialogue(dialogueIndexToTrigger));
            dialogueTriggered = true;
            Debug.Log("Player has collided with the trigger GameObject.");
        }
    }

    void Update()
    {
        // Check for mouse click or space bar input
        if (!dialogueManager.isTyping && Input.GetMouseButtonDown(0) || !dialogueManager.isTyping && Input.GetKeyDown(KeyCode.Space))
        {
            // Trigger the next dialogue
            dialogueManager.NextDialogue();
        }
    }
}
