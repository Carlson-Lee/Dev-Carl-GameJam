using UnityEngine;

public class ResetPlayerPosition : MonoBehaviour
{
    public Vector2 respawnPosition = Vector2.zero; //Start initial respawn at level start point
    public EndGameManager endGameManager;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            endGameManager.respawnCount++;
            collision.transform.position = respawnPosition;
        }
    }
}
