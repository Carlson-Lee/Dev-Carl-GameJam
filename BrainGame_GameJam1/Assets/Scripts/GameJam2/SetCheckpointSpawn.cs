/*
 *  File: SetCheckpointSpawn.cs
 *  Author: Devon
 *  Purpose: Updates spawn location when passing checkpoint
 *           Enables a light for clear indication the checkpoint has been passed
 */

using UnityEngine;

public class SetCheckpointSpawn : MonoBehaviour
{
    [SerializeField] private ResetPlayerPosition playerReset;
    [SerializeField] private GameObject light;
    private BoxCollider2D collider;

    /// <summary>
    /// Sets a new spawn position and turns on light for checkpoints
    /// </summary>
    /// <param name="collision">Player Collider</param>
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
