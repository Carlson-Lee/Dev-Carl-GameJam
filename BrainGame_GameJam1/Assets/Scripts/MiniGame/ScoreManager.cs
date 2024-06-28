using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;
    private SceneController sceneController;


    private int score = 0;
    public int requiredCoins = 7;
    public TextMeshProUGUI scoreText;
    public GameObject dialogueMenuPrefab;
    public DialogueManager dialogueManager;
    public DialogueDataStruct dialogueDataStruct;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        sceneController = SceneController.Instance;
    }

    void Start()
    {
        // Ensure the dialogue manager is assigned in the Inspector
        if (dialogueManager == null)
        {
            Debug.LogError("DialogueManager is not assigned in the ScoreManager!");
        }
        
        // Initialize the score display
        UpdateScoreText();
    }

    public void AddScore(int amount)
    {
        score += amount;
        UpdateScoreText();

        // Check if score reaches the required number of coins
        if (score == requiredCoins)
        {
            ShowEndGameDialogue();
            EndGame();
        }

    }

    void UpdateScoreText()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + score.ToString();
        }
        else
        {
            Debug.LogError("Score TextMeshProUGUI is not assigned in the ScoreManager!");
        }
    }

    void ShowEndGameDialogue()
    {
        dialogueMenuPrefab.SetActive(true);
        if (dialogueManager != null && dialogueDataStruct != null)
    {
        DialogueDataStruct.Dialogue endGameDialogue = dialogueDataStruct.GetEndGameDialogue();
        if (endGameDialogue != null)
        {
            StartCoroutine(dialogueManager.DisplaySpecificDialogue(endGameDialogue));
        }
        else
        {
            Debug.LogError("End game dialogue not found in DialogueDataStruct!");
        }
    }
    else
    {
        Debug.LogError("DialogueManager or DialogueDataStruct is not properly assigned!");
    }
    }

    public void EndGame()
    {
        sceneController.LoadMainMenu();
    }

}
