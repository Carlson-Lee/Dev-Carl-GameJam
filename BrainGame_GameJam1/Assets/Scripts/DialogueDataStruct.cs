using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DialogueDataStruct : MonoBehaviour
{
    [System.Serializable]

    public class Dialogue
    {
        public string[] lines;  // Array of dialogue lines
    }

    public Dialogue[] dialogues; //Array to hold all dialogues
    public Dialogue[] miniGameDialogues; // Array to hold dialogues for the mini-game
    public Dialogue endGameDialogue;

    public Dialogue[] GetCurrentDialogues()
    {
        // Example: Check current scene or game state and return appropriate dialogues
        string currentSceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;

        if (currentSceneName == "MiniGame")
        {
            return miniGameDialogues;
        }
        else
        {
            return dialogues;
        }
    }

    public Dialogue GetEndGameDialogue()
    {
        return endGameDialogue;
    }
    
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
            }

        };

        // Initialize mini-game dialogues
        miniGameDialogues = new Dialogue[]
        {
            // Dialogue for movement instructions
            new Dialogue
            {
                lines = new string[]
                {
                    "Welcome to the Mini-Game!",
                    "Instructions:",
                    "Use the ↑   ↓   ←   → keys to move around.",
                    "Double tap ↑ to jump higher",
                    "Collect 7 coins to exchange your soul."
                }
            }
        };

        endGameDialogue = new Dialogue
        {
            lines = new string[]
            {
                "You have collected enough coins to exchange for your soul!"
            }
        };
    }
}
