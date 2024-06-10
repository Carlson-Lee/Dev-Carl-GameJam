/*
 *  File: ResetPlayerPosition.cs
 *  Author: Devon
 *  Purpose: Fall detection for when player falls off level. Respawns back at set position and increments respawn counter
 */

using UnityEngine;

public class ResetPlayerPosition : MonoBehaviour
{
    public Vector2 respawnPosition = Vector2.zero; //Start initial respawn at level start point
    public EndGameManager endGameManager; //Needed to increment respawn counter

    /// <summary>
    /// Checks for the player falling off the level
    /// </summary>
    /// <param name="collision">Player Collider</param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            endGameManager.respawnCount++; 
            collision.transform.position = respawnPosition; //Move the player back to respawn location
        }
    }
}
