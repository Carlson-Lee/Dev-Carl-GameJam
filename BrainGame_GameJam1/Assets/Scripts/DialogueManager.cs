using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class DialogueManager : MonoBehaviour
{
    public DialogueDataStruct dialogueData; //Reference to DialogueDataStruct script
    public string labelElementName = "DialogueLabel"; // Name or identifier of the Label element in UI Builder
    public AudioClip typingSound; //Typewriter audio clip
    public bool isTyping = false;

    private Label dialogueLabel; // Reference to the Label element
    private int currentDialogueIndex = 0; 
     private DialogueDataStruct.Dialogue[] currentDialogues; // Array to hold current dialogues
    private float typingSpeed = 0.05f; //Typing speed in seconds per character
    private AudioSource audioSource; //Reference to the AudioSource component
     // Flag to track if dialogue is currently being displayed  

    void Start()
    {
        // Find the Label element by its name
        var uiDoc = GetComponent<UIDocument>();

        var root = uiDoc.rootVisualElement;
        dialogueLabel = root.Q<Label>(labelElementName);
        

        // AudioSource component must be initialized before StartCoroutine
        // Get reference to AudioSource component
        audioSource = GetComponent<AudioSource>();
        // Assign audio clip to the AudioSource component
        audioSource.clip = typingSound;
        
        // Get the current dialogues based on the scene
        currentDialogues = dialogueData.GetCurrentDialogues();
        StartDialogue(0);
    }

    public void StartDialogue(int index)
    {
        currentDialogueIndex = index;
        StartCoroutine(DisplayDialogue(currentDialogueIndex));
    }

    public IEnumerator DisplayDialogue(int index)
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

                // Play Sound
                audioSource.Play();
                isTyping = true;

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

                audioSource.Stop();
                isTyping = false;
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

    //Update dialogue text
    public void ShowDialogue(string message)
    {
        UpdateLabel(message);
        gameObject.SetActive(true); // Show the dialogue menu
    }

    // Method to advance to the next dialogue
    public void NextDialogue()
    {
        
        //StartCoroutine(DisplayDialogue(currentDialogueIndex));

        if(currentDialogueIndex == 0)
        {
            gameObject.SetActive(false);
        }
        else
        {
            currentDialogueIndex++;
            StartCoroutine(DisplayDialogue(currentDialogueIndex));
        }
    }

    // Method to update the text of the Label element
    void UpdateLabel(string newText)
    {
        // Update the text of the Label element
        dialogueLabel.text = newText;
        //Debug.LogError(newText);
    }

    public void CloseDialogue()
    {
        gameObject.SetActive(false);
    }
}


