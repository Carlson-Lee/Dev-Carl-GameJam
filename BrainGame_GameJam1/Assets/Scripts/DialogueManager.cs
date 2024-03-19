using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class DialogueManager : MonoBehaviour
{
    public DialogueDataStruct dialogueData; //Reference to DialogueDataStruct script
    public string labelElementName = "DialogueLabel"; // Name or identifier of the Label element in UI Builder
    private Label dialogueLabel; // Reference to the Label element
    private int currentDialogueIndex = 0;
    

    void Start()
    {
        // Find the Label element by its name
        var uiDoc = GetComponent<UIDocument>();

        if (uiDoc != null)
        {
            var root = uiDoc.rootVisualElement;
            dialogueLabel = root.Q<Label>(labelElementName);
            if (dialogueLabel != null)
            {
                Debug.Log("Label element found successfully: " + labelElementName);
                // Update the text of the Label element
                //UpdateLabel("NEW Dialogue");
            }
            else
            {
                Debug.LogError("Label element not found with name: " + labelElementName);
            }
        }
        else
        {
            Debug.LogError("UIDocument component not found on GameObject.");
        }

        //display the initial dialogue
        DisplayDialogue(currentDialogueIndex);
    }

    void DisplayDialogue(int index)
    {
        // Check if the index is within the bounds of the dialogues array
        if (index >= 0 && index < dialogueData.dialogues.Length)
        {
            DialogueDataStruct.Dialogue dialogue = dialogueData.dialogues[index];
            // Check if the dialogue is not null
            if (dialogue != null)
            {
                // Concatenate all lines of dialogue into a single string
                string fullDialogue = string.Join("\n", dialogue.lines);
                // Update the text of the Label element
                UpdateLabel(fullDialogue);
            }
            else
            {
                Debug.LogError("Dialogue is null.");
            }
        }
        else
        {
            Debug.LogError("Index is out of range.");
        }
    }

    // Method to advance to the next dialogue
    public void NextDialogue()
    {
        currentDialogueIndex++;
        DisplayDialogue(currentDialogueIndex);
    }

    // Method to update the text of the Label element
    void UpdateLabel(string newText)
    {
        // Update the text of the Label element
        dialogueLabel.text = newText;
        Debug.LogError(newText);
    }
}


