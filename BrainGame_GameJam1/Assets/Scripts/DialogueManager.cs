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
    private float typingSpeed = 0.05f; //Typing speed in seconds per character
    

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
        StartCoroutine(DisplayDialogue(currentDialogueIndex));

    }

    IEnumerator DisplayDialogue(int index)
    {
        // Check if the index is within the bounds of the dialogues array
        if (index >= 0 && index < dialogueData.dialogues.Length)
        {
            DialogueDataStruct.Dialogue dialogue = dialogueData.dialogues[index];
            
            // Check if the dialogue is not null
            if (dialogue != null)
            {

                // Clear the dialogue label
                UpdateLabel("");

                // Loop through each character of the dialogue text
                foreach (string line in dialogue.lines)
                {
                    foreach (char letter in line)
                    {
                        dialogueLabel.text += letter;
                        yield return new WaitForSeconds(typingSpeed);
                    }
                    // Add the next character to the dialogue label
                    dialogueLabel.text += "\n";
                    // Wait for a short delay before showing the next character
                    yield return new WaitForSeconds(typingSpeed);
                }
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
        StartCoroutine(DisplayDialogue(currentDialogueIndex));
    }

    // Method to update the text of the Label element
    void UpdateLabel(string newText)
    {
        // Update the text of the Label element
        dialogueLabel.text = newText;
        //Debug.LogError(newText);
    }
}


