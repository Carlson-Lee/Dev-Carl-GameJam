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
    }

    public Dialogue[] dialogues; //Array to hold all dialogues

    public Dialogue GetDialogue(int index)
    {
        if (index >= 0 && index < dialogues.Length)
        {
            return dialogues[index];
        }
        else
        {
            Debug.LogError("Dialogue index out of range.");
            return null;
        }
        
    }
                    //     "You woke up in a strange land,",
                    // "You notice something is missing",
                    // "Use the ↑   ↓   ←   → keys to move around" 
}
