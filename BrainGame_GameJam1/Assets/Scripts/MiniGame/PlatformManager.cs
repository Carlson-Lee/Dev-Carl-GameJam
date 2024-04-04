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
    private GameObject lastSpawnedPlatform;
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
        Instantiate(platformPrefab, spawnPosition, Quaternion.identity);
        lastSpawnX += segmentWidth;
        lastSpawnedPlatform = platformPrefab;
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
        GameObject[] platforms = GameObject.FindGameObjectsWithTag("Platform");

        foreach (GameObject platform in platforms)
        {
            if (platform.transform.position.x < playerTransform.position.x - spawnDistance)
            {
                Destroy(platform);
            }
        }
    }

}
