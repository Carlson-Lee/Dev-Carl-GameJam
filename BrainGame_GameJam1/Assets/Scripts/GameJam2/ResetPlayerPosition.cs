using UnityEngine;

public class ResetPlayerPosition : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            //Will need to change this if checkpoints are implemented
            collision.transform.position = Vector2.zero;
        }
    }
}
