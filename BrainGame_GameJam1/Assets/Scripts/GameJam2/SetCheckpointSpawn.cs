using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetCheckpointSpawn : MonoBehaviour
{
    [SerializeField] private ResetPlayerPosition playerReset;
    [SerializeField] private GameObject light;

    private BoxCollider2D collider;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerReset.respawnPosition = this.transform.position;
            light.SetActive(true);
            
        }

        //Used to disable old checkpoints being triggered again
        collider = this.GetComponent<BoxCollider2D>();
        collider.enabled = false;
    }

}
