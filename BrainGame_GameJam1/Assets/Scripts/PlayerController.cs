using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
    public Tilemap TM_Ground; //Base
    public Tilemap TM_GroundDetails; //Other Base extras
    public Tilemap TM_CollisionObjects; //Collision objects tiles
    public Tilemap TM_CollisionDetails;
    public Tilemap TM_TopLevelDetails;
    public Tilemap TM_OverPlayerDetails;
    public Tilemap TM_OverPlayerDetails_Additional;

    [Header("Colour Change Size/Offsets")]
    public float tileChangeOffsetX;
    public float tileChangeOffsetY;
    public int colourGridOffset;

    /// <summary>
    /// Initializes the player's movement settings and components when the game starts.
    /// </summary>
    void Start()
    {
        //References for Rigidbody2D and storing collision data
        rb = GetComponent<Rigidbody2D>();
        castCollisions = new List<RaycastHit2D>();
        animator = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();

        //Set default values
        movementSpeed = 1f;
        collisionOffset = 0f; //Change to alter how close player sprite can get to tile collision borders
        tileChangeOffsetX = 0f;
        tileChangeOffsetY = 0.1f;
        colourGridOffset = 3;

        //Set values for movement limits over time
        moveThreshold = movementSpeed * Time.fixedDeltaTime;   //Move along path with velocity (= ..speed/..time)
        moveThresholdOffset = moveThreshold + collisionOffset; //Check movement path (extended) for collisions
    }

    /// <summary>
    /// Handles player inputs to move position
    /// </summary>
    /// <param name="movementValue">Players movement velocity</param>
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

    /// <summary>
    /// Updates the color of the tiles in a grid around the player
    /// </summary>
    private void UpdateTileColor()
    {
        UpdateTilemapColor(TM_Ground);
        UpdateTilemapColor(TM_GroundDetails);
        UpdateTilemapColor(TM_CollisionObjects);
        UpdateTilemapColor(TM_CollisionDetails);
        UpdateTilemapColor(TM_TopLevelDetails);
        UpdateTilemapColor(TM_OverPlayerDetails);
        UpdateTilemapColor(TM_OverPlayerDetails_Additional);
    }

    /// <summary>
    /// Update color for a specific tilemap
    /// </summary>
    /// <param name="currentTilemap">Tilemap being updated from BW to Colour</param>
    private void UpdateTilemapColor(Tilemap currentTilemap)
    {
        Vector3Int playerCellPosition = currentTilemap.WorldToCell(transform.position - new Vector3(tileChangeOffsetX, tileChangeOffsetY, 0)); // Adjusting for half tile offset

        // Iterate through a grid around the player
        for (int xOffset = -colourGridOffset; xOffset <= colourGridOffset; xOffset++)
        {
            for (int yOffset = -colourGridOffset; yOffset <= colourGridOffset; yOffset++)
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