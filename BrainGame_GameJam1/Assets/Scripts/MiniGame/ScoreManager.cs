using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;
    public TMP_Text scoreText; // Reference to the UI Text element for score
    private int score = 0;
    public DialogueManager dialogueManager;

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
    }

    public void AddScore(int amount)
    {
        score += amount;
        scoreText.text = "Score: " + score.ToString();

        // Check if score reaches 7
        if (score >= 7)
        {
            dialogueManager.ShowDialogue("You have collected enough coins to exchange for your soul!");
            EndGame();
        }
    }

    public void EndGame()
    {
        Application.Quit();
    }

}
