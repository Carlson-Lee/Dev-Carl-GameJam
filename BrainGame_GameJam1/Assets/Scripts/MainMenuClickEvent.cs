using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;

public class MainMenuClickEvent : MonoBehaviour
{
    UIDocument buttonDocument;
    Button StartButton;
    Button ExitButton;

    void OnEnable()
    {
        buttonDocument = GetComponent<UIDocument>();

        //Testing if the button is found
        if(buttonDocument == null)
        {
            Debug.LogError("No button document found");
        }

        StartButton = buttonDocument.rootVisualElement.Q("StartButton") as Button;
        ExitButton = buttonDocument.rootVisualElement.Q("ExitButton") as Button;

        if(StartButton != null)
        {
            Debug.Log("Start button found");
            StartButton.clicked += StartButtonClick;
        }

        if(ExitButton != null)
        {
            Debug.Log("Exit button found");
            ExitButton.clicked += ExitButtonClick;
        }

    }

    void StartButtonClick()
    {
        Debug.Log("Start button clicked");
        StartGame();
    }

    void ExitButtonClick()
    {
        Debug.Log("Exit button clicked");
        ExitGame();
    }

    public void StartGame()
    {
        // Load the game scene (assuming it's the next scene in the build settings)
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void ExitGame()
    {

        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }

    
}
