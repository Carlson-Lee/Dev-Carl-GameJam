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
                    "Use the ↑   ↓   ←   → keys to move around" 
                }
            },

            new Dialogue
            {
                lines = new string[]
                {
                    "You can interact with Objects with",
                    "      Space Bar",
                    "Lets go ahead and grab this light"
                }
            },

            new Dialogue
            {
                lines = new string[]
                {
                    "test"
                }
            },

            new Dialogue
            {
                lines = new string[]
                {
                    "2 You can interact with Objects with",
                    "      2 Space Bar",
                    "2 Lets go ahead and grab this light"
                }
            },

            new Dialogue
            {
                lines = new string[]
                {
                    "test 2"
                }
            },

            new Dialogue
            {
                lines = new string[]
                {
                    "test 2"
                }
            },

        };
    }
}
