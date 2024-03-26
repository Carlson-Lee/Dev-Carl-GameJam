using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public DialogueManager dialogueManager; // Reference to the DialogueManager script

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
