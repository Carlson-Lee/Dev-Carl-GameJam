using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public string labelElementName = "DialogueLabel"; // Name or identifier of the Label element in UI Builder
    private TMPro.TMP_Text dialogueLabel; // Reference to the Label element

    void Start()
    {
        // Find the Label element by its name
        GameObject labelObject = GameObject.Find(labelElementName);
        if (labelObject != null)
        {
            dialogueLabel = labelObject.GetComponent<TMPro.TMP_Text>();
        }
        else
        {
            Debug.LogError("Label element not found with name: " + labelElementName);
        }
    }

    public void UpdateLabel(string newText)
    {
        // Update the text of the Label element
        if (dialogueLabel != null)
        {
            dialogueLabel.text = newText;
        }
        else
        {
            Debug.LogError("Label element is not assigned or found.");
        }
    }
}


