/*
 *  File: SideCamController.cs
 *  Author: Devon
 *  Purpose: Controls the offset of the camera when moving left/right
 *  
 *  Help from: https://www.youtube.com/watch?v=PA5DgZfRsAM modified some code from here for the lookAhead variable
 */

using UnityEngine;

public class SideCamController : MonoBehaviour
{
    [Header("Camera Distances")]
    [SerializeField] private float lookAheadDistance;
    [SerializeField] private float camMoveSpeed;
    public float lookAhead;

    [Header("Player Components and inputs")]
    [SerializeField] private Transform player;
    [SerializeField] private SideScrollerPlayerController playerController;
    [SerializeField] private Vector2 movementInput;

    /// <summary>
    /// Sets the correct camera offset depending on the players movement input  <br />
    ///  - Player will see more of the level in the direction they are moving   <br />
    ///  - e.g When moving right, player sprite will be positioned at the left of the camera
    /// </summary>
    void Update()
    {
        //Get the current direction the player is moving
        Vector2 movementInput = playerController.movementInput;
        float direction = movementInput.x;

        //Update the camera offseft depending on the movement direction
        //Move smoothly between positions
        lookAhead = Mathf.Lerp(lookAhead, lookAheadDistance * direction, Time.deltaTime * camMoveSpeed);
        transform.position = new Vector3(player.position.x + lookAhead, transform.position.y, transform.position.z);
    }
}
