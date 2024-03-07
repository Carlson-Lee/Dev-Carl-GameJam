using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public Vector2 movementInput;
    public float movementSpeed;
    public float collisionOffset;
    public float moveThreshold;
    public float moveThresholdOffset;

    [Header("Filters")]
    public ContactFilter2D movementFilter; 

    [Header("Collisions")]
    public Rigidbody2D rb;
    public List<RaycastHit2D> castCollisions;

    /// <summary>
    /// Initializes the player's movement settings and components when the game starts.
    /// </summary>
    void Start()
    {
        //Set initial movement values
        movementSpeed = 1f;
        collisionOffset = 0f; //Gap between player and collision object

        //Set values for movement limits over time
        moveThreshold = movementSpeed * Time.fixedDeltaTime;   //Move along path with velocity (= ..speed/..time)
        moveThresholdOffset = moveThreshold + collisionOffset; //Check movement path (extended) for collisions

        //References for Rigidbody2D and storing collision data
        rb = GetComponent<Rigidbody2D>();
        castCollisions = new List<RaycastHit2D>();
    }

    //Handles player inputs to move position
    void OnMove(InputValue movementValue)
    {
        movementInput = movementValue.Get<Vector2>();
    }

    /// <summary>
    /// Attempts to move the player in the specified direction, considering collisions with the environment.
    /// </summary>
    /// <param name="direction">The direction in which to move the player.</param>
    /// <returns>True if the player successfully moves; otherwise, false.</returns>
    private bool TryMove(Vector2 direction)
    {
        bool canMove = false;

        //Count number of collisions when casting from player
        int count = rb.Cast(    //Check for collisions in input direction
            movementInput,      //Direction to check collisions
            movementFilter,     //Check which collisions can occur
            castCollisions,     //Stored collisions from raycast
            moveThresholdOffset //Amount to cast for collision check
            );

        if (count == 0) //Only move if there is no collision in cast
        {
            rb.MovePosition(rb.position + (direction * moveThreshold));
            canMove = true;
        }
        else //Collision in raycast
        {
            // Check if there's a collision along the x-axis
            int countX = rb.Cast(Vector2.right * direction.x, movementFilter, castCollisions, moveThresholdOffset);

            if (countX == 0) // No collision so move only on the x-axis
            {
                rb.MovePosition(rb.position + (Vector2.right * direction.x * moveThreshold));
                canMove = true;
            }

            // Check if there's a collision along the y-axis
            int countY = rb.Cast(Vector2.up * direction.y, movementFilter, castCollisions, moveThresholdOffset);

            if (countY == 0) // No collision so move only on the y-axis
            {
                rb.MovePosition(rb.position + (Vector2.up * direction.y * moveThreshold));
                canMove = true;
            }
        }
        return canMove; // Return whether the player successfully moved
    }

    /// <summary>
    /// Handles player movement based on user input, checking for collisions
    /// </summary>
    private void FixedUpdate()
    {
        //Wait for user input before moving
        if (movementInput != Vector2.zero)
        {
            bool canMove = TryMove(movementInput);

            if (!canMove) //Movement blocked by collision object
            {
                canMove = TryMove(new Vector2(movementInput.x, 0)); //Try movement only on x-input-axis

                if (!canMove)
                {
                    canMove = TryMove(new Vector2(0, movementInput.y)); //Try movement only on y-input-axis
                }
            }
        }
    }
}