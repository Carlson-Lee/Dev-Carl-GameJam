using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class DialogueManager : MonoBehaviour
{
    public string labelElementName = "DialogueLabel"; // Name or identifier of the Label element in UI Builder
    private Label dialogueLabel; // Reference to the Label element

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
                UpdateLabel("NEW Dialogue");
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
    }

    // Method to update the text of the Label element
    void UpdateLabel(string newText)
    {
        // Update the text of the Label element
        dialogueLabel.text = newText;
    }
}


