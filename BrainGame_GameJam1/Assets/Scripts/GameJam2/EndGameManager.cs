using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EndGameManager : MonoBehaviour
{
    [Header("Raw Images")]
    [SerializeField] private RawImage[] UICollectibles = new RawImage[3];
    [SerializeField] private RawImage[] EndPanelCollectibles = new RawImage[3];

    [Header("Text Objects")]
    public TMP_Text timerText;
    public TMP_Text respawnCountText;
    public int respawnCount = 0;

    private float timer;
    public bool gameover;

    void Start()
    {
        timer = 0f;
        gameover = false;
    }

    void Update()
    {
        if (!gameover)
        {
            timer += Time.deltaTime;
        }
    }

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
