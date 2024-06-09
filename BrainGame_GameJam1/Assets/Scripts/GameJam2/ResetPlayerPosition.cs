using UnityEngine;

public class ResetPlayerPosition : MonoBehaviour
{
    public Vector2 respawnPosition = Vector2.zero; //Start initial respawn at level start point

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.transform.position = respawnPosition;
        }
    }
}
