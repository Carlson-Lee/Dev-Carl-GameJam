using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformManager : MonoBehaviour
{
    public GameObject platformPrefab;
    public Transform playerTransform;
    public float segmentWidth = 2f;
    public int segmentsAhead = 1;
    public float destroyDistance = 1f;

    private float lastSpawnX;
    private Queue<GameObject> spawnedSegments = new Queue<GameObject>();


   
    void Start()
    {

        lastSpawnX = playerTransform.position.x;
        SpawnInitialSegments();
    }

    // Update is called once per frame
    void Update()
    {
        float spawnPositionX = lastSpawnX + segmentWidth;


        if (playerTransform.position.x > spawnPositionX - (segmentWidth * segmentsAhead))
        {
            SpawnSegment(spawnPositionX);
            lastSpawnX = spawnPositionX;
        }

        DestroySegmentsBehind();
    }


    void SpawnInitialSegments()
    {
        for (int i = 0; i < segmentsAhead; i++)
        {
            SpawnSegment(lastSpawnX);
            lastSpawnX += segmentWidth;
        }
    }

    void SpawnSegment(float spawnX)
    {
        Vector3 spawnPosition = new Vector3(spawnX, transform.position.y, transform.position.z);
        GameObject newSegment = Instantiate(platformPrefab, spawnPosition, Quaternion.identity);
        spawnedSegments.Enqueue(newSegment);
    }

    void DestroySegmentsBehind()
    {
        while (spawnedSegments.Count > 0 && spawnedSegments.Peek().transform.position.x < playerTransform.position.x - destroyDistance)
        {
            GameObject segmentToDestroy = spawnedSegments.Dequeue();
            Destroy(segmentToDestroy);
        }
    }


}
