/*
 *  File: EndGameManager.cs
 *  Author: Devon
 *  Purpose: Sets the values in the end game panel as a summary (time, respawns, collectibles)
 */

using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EndGameManager : MonoBehaviour
{
    [Header("Raw Images")]
    [SerializeField] private RawImage[] UICollectibles = new RawImage[3]; //Items picked up during gameplay
    [SerializeField] private RawImage[] EndPanelCollectibles = new RawImage[3]; //Items to show on end screen

    [Header("Text Objects")]
    public TMP_Text timerText; //Time taken summary text
    public TMP_Text respawnCountText; //Respawns summary text
    public int respawnCount = 0; 
    private float timer;
    public bool gameover; //Used to stop time on gameover

    /// <summary>
    /// Start the game with a reset timer
    /// </summary>
    void Start()
    {
        timer = 0f;
        gameover = false;
    }

    /// <summary>
    /// If the game hasn't ended, keep incrementing the timer value
    /// </summary>
    void Update()
    {
        if (!gameover)
        {
            timer += Time.deltaTime;
        }
    }

    /// <summary>
    /// Display the correct images in end game panel depending what was picked up and show in gameplay UI
    /// </summary>
    public void CheckItemsAndShowGameOver()
    {
        for (int i = 0; i < UICollectibles.Length; i++)
        {
            if (UICollectibles[i].enabled)
            {
                EndPanelCollectibles[i].enabled = true;
                Debug.Log("Enabling collectible", EndPanelCollectibles[i]);
            }
        }
        ShowGameOver();
    }

    /// <summary>
    /// Stop the timer and format the time as 00:00:00, display respawn count 
    /// </summary>
    public void ShowGameOver()
    {
        gameover = true;

        // Update the UI elements
        System.TimeSpan timeSpan = System.TimeSpan.FromSeconds(timer);// Format the timer into minutes, seconds, and milliseconds
        string formattedTime = string.Format("{0:D2}:{1:D2}:{2:D2}", (int)timeSpan.TotalMinutes, timeSpan.Seconds, timeSpan.Milliseconds / 10);
        timerText.text = formattedTime;
        respawnCountText.text = respawnCount.ToString();
    }
}
