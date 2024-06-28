using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;

    private int score = 0;
    public int requiredCoins = 7;
    public TextMeshProUGUI scoreText;
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
        if (score >= requiredCoins)
        {
            ShowEndGameDialogue();
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
        // Show the end game dialogue
        StartCoroutine(dialogueManager.DisplaySpecificDialogue(dialogueDataStruct.endGameDialogue));
    }

    public void EndGame()
    {
        Application.Quit();
    }

}
