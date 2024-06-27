using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinGenerator : MonoBehaviour
{
    public GameObject coinPrefab; // Reference to the coin prefab
    public GameObject[] platforms; // Array to hold references to platforms

    public int coinsToGenerate = 7; // Number of coins to generate

    void Start()
    {
        // Shuffle platforms array
        ShufflePlatforms();
        GenerateCoins();
    }

    void ShufflePlatforms()
    {
        // Optional: Shuffle the platforms array using Fisher-Yates shuffle algorithm
        for (int i = platforms.Length - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            GameObject temp = platforms[i];
            platforms[i] = platforms[j];
            platforms[j] = temp;
        }
    }

    void GenerateCoins()
    {
        for (int i = 0; i < Mathf.Min(coinsToGenerate, platforms.Length); i++)
        {
            GameObject platform = platforms[i];
            GenerateCoinOnPlatform(platform);
        }
    }

    void GenerateCoinOnPlatform(GameObject platform)
    {
        // Calculate a position above the platform
        Vector3 coinPosition = platform.transform.position + Vector3.up * 1.5f; // Adjust Vector3.up as needed
        // Instantiate a coin prefab at the calculated position
        GameObject coin = Instantiate(coinPrefab, coinPosition, Quaternion.identity);
        coin.transform.parent = platform.transform;
    }
}
