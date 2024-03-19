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
        //public string[] tutorial;

        // Optional: You can include additional fields for speaker name, portraits, etc.
        // public string speakerName;
        // public Sprite speakerPortrait;

        // Optional: If your dialogues include choices, you can define a separate class for them
        // public DialogueChoice[] choices;
    }

    public Dialogue[] dialogues; //Array to hold all dialogues

    void Start()
    {
        dialogues = new Dialogue[]
        {
            new Dialogue
            {
                lines = new string[]
                {
                    "You woke up in a strange land,",
                    "You notice something is missing",
                    "Use the up-down-left-right keys to move around" 
                }
            },

            new Dialogue
            {
                lines = new string[]
                {
                    "new dialogue test line",
                }
            }
        };
    }
}
