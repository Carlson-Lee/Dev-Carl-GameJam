using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EndGameManager : MonoBehaviour
{
    [Header("Raw Images")]
    public RawImage[] rawImagesToCheck = new RawImage[3];
    public RawImage[] collectibleImages = new RawImage[3];

    [Header("Text Objects")]
    public TMP_Text timerText;
    public TMP_Text respawnCountText;
    public int respawnCount = 0;

    private float timer;
    private bool gameover;

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
        foreach (var rawImage in rawImagesToCheck)
        {
            if (rawImage.enabled)
            {
                //enable corresponding collectible images on end panel if collected
                int index = System.Array.IndexOf(rawImagesToCheck, rawImage);
                if (index >= 0 && index < collectibleImages.Length)
                {
                    collectibleImages[index].enabled = true;
                }
                else
                {
                    collectibleImages[index].enabled = false;
                }
            }
        }
        ShowGameOver();
    }

    public void ShowGameOver()
    {
        gameover = true;

        // Update the UI elements
        timerText.text = timer.ToString("F2");
        respawnCountText.text = respawnCount.ToString();
    }
}
