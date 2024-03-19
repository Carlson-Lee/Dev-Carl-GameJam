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

    void Start()
    {
        //Instantiate the Dialogue
        Dialogue dialogue1 = new Dialogue();
        dialogue1.lines = new string[]
        {
            "Yuo've woken up in a strange land",
            "You feel something inside you has been lost"
        };
    }
}
