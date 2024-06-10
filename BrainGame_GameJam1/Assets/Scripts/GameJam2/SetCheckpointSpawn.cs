using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetCheckpointSpawn : MonoBehaviour
{
    [SerializeField] private ResetPlayerPosition playerReset;
    [SerializeField] private GameObject light;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerReset.respawnPosition = this.transform.position;
            light.SetActive(true);
        }
    }

}
