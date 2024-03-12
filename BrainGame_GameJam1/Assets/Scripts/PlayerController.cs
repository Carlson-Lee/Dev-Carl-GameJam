using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

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

    [Header("Animation Components")]
    public PlayerStates currentState; //Check for current animation needed
    public SpriteRenderer sr;
    public Animator animator;

    [Header("Tilemap Components")]
    public Tilemap tilemap; //Base
    public Tilemap ex_tilemap; //Other Base extras
    public Tilemap ob_tilemap; //Collision objects tiles
    public float tileChangeOffsetX = 0.1f;
    public float tileChangeOffsetY = 0.1f;

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
        animator = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
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
        UpdateTileColor();
        FlipSprite(movementInput.x); //Check if sprite needs flipped
        AnimationCheck(movementInput.x, movementInput.y); //Check if animation state needs to change
    }

    /// <summary>
    /// Possible animation states for the player character in the animator.
    /// </summary>
    public enum PlayerStates
    { IDLE, WALK, ATTACK, DIE }

    /// <summary>
    /// Handles the switching and setting of the current animation state.
    /// </summary>
    PlayerStates CurrentState
    {
        set
        {
            currentState = value;

            switch (currentState)
            {
                case PlayerStates.IDLE:
                    animator.Play("Idle");
                    break;
                case PlayerStates.WALK:
                    animator.Play("Walk");
                    break;
                case PlayerStates.ATTACK:
                    animator.Play("Attack");
                    break;
                case PlayerStates.DIE:
                    animator.Play("Die");
                    break;
                default:
                    break;
            }
        }
    }

    /// <summary>
    /// Checks and changes direction of horizontal sprites for left/right movement
    /// </summary>
    /// <param name="x"></param>
    private void FlipSprite(float x)
    {
        if (x != 0 && x < 0)
        {
            sr.flipX = true;
        }
        else if (x != 0 && x > 0)
        {
            sr.flipX = false;
        }
    }

    /// <summary>
    /// Controls the switching of animation states based on the current input values.
    /// </summary>
    /// <param name="x">The horizontal movement input value (usually obtained from Input.GetAxis("Horizontal")).</param>
    /// <param name="z">The vertical movement input value (usually obtained from Input.GetAxis("Vertical")).</param>
    private void AnimationCheck(float x, float z)
    {
        if (x != 0 || z != 0) //If an input is being pressed
        {
            CurrentState = PlayerStates.WALK;
            animator.SetFloat("xMove", x);
            animator.SetFloat("zMove", -z);
        }
        else
        {
            CurrentState = PlayerStates.IDLE;
        }
    }

    // Updates the color of the tiles in a 3x3 grid around the player
    private void UpdateTileColor()
    {
        UpdateTilemapColor(tilemap);
        UpdateTilemapColor(ex_tilemap);
        UpdateTilemapColor(ob_tilemap);
    }

    // Update color for a specific tilemap
    private void UpdateTilemapColor(Tilemap currentTilemap)
    {
        Vector3Int playerCellPosition = currentTilemap.WorldToCell(transform.position - new Vector3(tileChangeOffsetX, tileChangeOffsetY, 0)); // Adjusting for half tile offset

        // Iterate through a 3x3 grid around the player
        for (int xOffset = -1; xOffset <= 1; xOffset++)
        {
            for (int yOffset = -1; yOffset <= 1; yOffset++)
            {
                Vector3Int cellPosition = playerCellPosition + new Vector3Int(xOffset, yOffset, 0);
                TileBase tile = currentTilemap.GetTile(cellPosition);

                if (tile != null)
                {
                    currentTilemap.SetTile(cellPosition, null);
                }
            }
        }
    }
}