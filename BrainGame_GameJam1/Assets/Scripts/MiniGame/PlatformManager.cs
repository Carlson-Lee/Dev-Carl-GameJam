using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformManager : MonoBehaviour
{
    public GameObject platformPrefab;
    public Transform playerTransform;
    public float segmentWidth = 5f;
    public int spawnDistance = 10;
    public float destroyDistance = 10f;

    private float lastSpawnX;
    private List<GameObject> activePlatforms = new List<GameObject>();
    private bool canSpawn = true;

    void Start()
    {
        lastSpawnX = playerTransform.position.x;
        SpawnSegment();
    }

    void Update()
    {
        if (canSpawn && playerTransform.position.x > lastSpawnX - spawnDistance)
        {
            SpawnSegment();
        }

        DestroySegmentsBehind();
    }

    void SpawnSegment()
    {
        Vector3 spawnPosition = new Vector3(lastSpawnX + segmentWidth, transform.position.y - 5.5f, transform.position.z);
        GameObject newPlatform = Instantiate(platformPrefab, spawnPosition, Quaternion.identity);
        activePlatforms.Add(newPlatform); // Add to list of active platforms
        lastSpawnX += segmentWidth;
        canSpawn = false;
        StartCoroutine(WaitForPlayerMove());
    }

    IEnumerator WaitForPlayerMove()
    {
        yield return new WaitUntil(() => playerTransform.position.x > lastSpawnX - segmentWidth);
        canSpawn = true;
    }

    void DestroySegmentsBehind()
    {
        for (int i = activePlatforms.Count - 1; i >= 0; i--)
        {
            if (activePlatforms[i].transform.position.x < playerTransform.position.x - destroyDistance)
            {
                Destroy(activePlatforms[i]);
                activePlatforms.RemoveAt(i);
            }
        }
    }

}
