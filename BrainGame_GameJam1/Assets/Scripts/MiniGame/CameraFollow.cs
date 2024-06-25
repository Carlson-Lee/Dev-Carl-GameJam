using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;        // Reference to the player's transform
    public Vector2 minBounds;       // Minimum X and Y bounds of the level
    public Vector2 maxBounds;       // Maximum X and Y bounds of the level
    public float smoothTime = 0.3f; // Smooth time for camera movement
    private Vector3 velocity = Vector3.zero; // Velocity for SmoothDamp

    void FixedUpdate()
    {
        if (player != null)
        {
            // Calculate target position with player's position and current camera position's Z
            Vector3 targetPosition = new Vector3(player.position.x, player.position.y, transform.position.z);

            // Smoothly move the camera towards the target position
            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);

            // Clamp the camera position to stay within level boundaries
            float clampedX = Mathf.Clamp(transform.position.x, minBounds.x, maxBounds.x);
            float clampedY = Mathf.Clamp(transform.position.y, minBounds.y, maxBounds.y);
            transform.position = new Vector3(clampedX, clampedY, transform.position.z);
        }
    }
}
