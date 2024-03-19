using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueDataStruct : MonoBehaviour
{
    [System.Serializable]

    public class Dialogue
    {
        public string[] lines;  // Array of dialogue lines

        // Optional: You can include additional fields for speaker name, portraits, etc.
        // public string speakerName;
        // public Sprite speakerPortrait;

        // Optional: If your dialogues include choices, you can define a separate class for them
        // public DialogueChoice[] choices;
    }

    public Dialogue[] dialogues; //Array to hold all dialogues

    // Method to retrieve a dialogue by index
    public Dialogue GetDialogue(int index)
    {
        if (index >= 0 && index < dialogues.Length)
        {
            return dialogues[index];
        }
        else
        {
            Debug.LogError("Index out of range: " + index);
            return null;
        }
    }
}
