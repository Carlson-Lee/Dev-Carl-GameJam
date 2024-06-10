/*
 *  File: SideScrollerPlayerController.cs
 *  Author: Devon
 *  Purpose: Controls aspects ofthe player during gameplay
 *   - Animations
 *   - Ground and Wall Detection
 *   - Wall bounces
 *   - Jumping
 *   - Movement
 *   - Respawning
 *  
 *  AI assistance: Some help from ChatGPT in FixedUpdate for the wall collision raycasts
 */

using UnityEngine;
using UnityEngine.InputSystem;

public class SideScrollerPlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float movementSpeed; //Speed of the player
    [SerializeField] private float jumpForce; //How high the player can jump
    [SerializeField] public Vector2 movementInput; //Current keyboard input direction
    [SerializeField] private PlayerStates currentState; //Check for current animation needed

    [Header("CollisionDetection")]
    [SerializeField] private bool isGrounded; //Check for player standing on ground
    [SerializeField] private bool isTouchingWall; //Check for player hitting a wall
    [SerializeField] private bool isFalling; //Check for player falling after jump/wall bounce
    //Walls
    [SerializeField] public float wallCheckDistance; //Distance of raycast for wall bounce
    [SerializeField] private LayerMask wallLayer; //Objects to trigger wall bounce behaviour
    //Ground
    [SerializeField] private Transform groundCheck; //Object position for a grounded raycast
    [SerializeField] private Vector2 groundCheckSize; //Raycast size for ground check detection
    [SerializeField] public float groundCheckDistance; //Raycast distance from player to current ground object
    [SerializeField] private LayerMask groundLayer; //Objects to trigger grounded behaviour (i.e jumping)
    [SerializeField] private float wallPushModifier; //Amount the player gets bounced off the wall

    [Header("Components")]
    //Player Objects
    [SerializeField] private Rigidbody2D rb; 
    [SerializeField] private Animator animator; 
    [SerializeField] private SpriteRenderer sr;
    [SerializeField] private PlayerInput playerInput;
    //RespawnTrigger count used on final game panel
    [SerializeField] private ResetPlayerPosition resetTrigger;
    //Manages the end game state of the game
    [SerializeField] private EndGameManager endGameManager;

    /// <summary>
    /// Instantiates player variables at game start
    /// </summary>
    private void Start()
    {
        //Assign components
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        playerInput = GetComponent<PlayerInput>();
        playerInput.SwitchCurrentActionMap("SideScroller"); // Ensure correct switch to SideScroller action map

        //Set all variable default values
        movementSpeed = 1f;
        jumpForce = 10f;
        isGrounded = false;
        isTouchingWall = false;
        isFalling = false;
        wallCheckDistance = 0.05f;
        groundCheckDistance = 0.1f;
        wallPushModifier = 0.5f;
        groundCheckSize = new Vector2(0.2f, 0.1f);
    }

    /// <summary>
    /// Update any changes to the sprite (walking, idle, direction)  <br />
    /// Checks for key press to respawn the player
    /// </summary>
    private void Update()
    {
        UpdateSpriteDirection();
        UpdateAnimationState();

        if (Input.GetKeyDown(KeyCode.R))
        {
            RespawnAtCheckpoint();
        }
    }

    /// <summary>
    /// Performs a ground/wall check during gameplay
    /// </summary>
    private void FixedUpdate()
    {
        isGrounded = GroundCheck();
        isFalling = isGrounded ? false : isFalling; //If player is on ground, set falling to false

        //Check if player touching walls
        RaycastHit2D wallHitRight = Physics2D.Raycast(transform.position, Vector2.right, wallCheckDistance, wallLayer);
        RaycastHit2D wallHitLeft = Physics2D.Raycast(transform.position, Vector2.left, wallCheckDistance, wallLayer);
        isTouchingWall = wallHitRight.collider != null || wallHitLeft.collider != null;

        // Movement
        if (!isTouchingWall && !isFalling)
        {
            rb.velocity = new Vector2(movementInput.x * movementSpeed, rb.velocity.y);
        }
        else if (isTouchingWall)
        {
            if (wallHitRight.collider != null)
            {
                rb.velocity = new Vector2(-movementSpeed * wallPushModifier, rb.velocity.y); // Bounce to the left
            }
            else if (wallHitLeft.collider != null)
            {
                rb.velocity = new Vector2(movementSpeed * wallPushModifier, rb.velocity.y); // Bounce to the right
            }
            isFalling = true;
        }
    }

    /// <summary>
    /// Use overlap box to check area below player for ground layer => set bool
    /// </summary>
    /// <returns>Bool for if the player is currently grounded</returns>
    private bool GroundCheck()
    {
        Collider2D groundHit = Physics2D.OverlapBox(groundCheck.position, groundCheckSize, 0, groundLayer);
        return groundHit != null;
    }

    // No longer used but keeping code incase i revert back to 'send messages' input system behaviour
    /// <summary>
    /// Saves the players input direction for movement
    /// </summary>
    /// <param name="movementValue"></param>
    private void OnMove(InputValue movementValue) { movementInput = movementValue.Get<Vector2>(); }

    // No longer used but keeping code incase i revert back to 'send messages' input system behaviour
    /// <summary>
    /// Checks for jump input and adds force to player rb in upwards direction
    /// </summary>
    /// <param name="jumpValue"></param>
    private void OnJump(InputValue jumpValue)
    {
        if (jumpValue.isPressed && isGrounded)
        {
            rb.AddForce(Vector2.up * jumpForce);
        }
    }

    /// <summary>
    /// Invokes a Unity Event to move the player in a Vector2 direction
    /// </summary>
    /// <param name="context">Key press for movement (e.g A/D or arrow keys)</param>
    public void Move(InputAction.CallbackContext context)
    {
        movementInput = context.ReadValue<Vector2>();
    }

    /// <summary>
    /// Invokes a Unity Event to make the player jump in an upwards direction
    /// </summary>
    /// <param name="context">Key press for jumping (e.g W, up arrow, space)</param>
    public void Jump(InputAction.CallbackContext context)
    {
        //State of the key press
        bool jumpPressed = context.performed;
        bool jumpReleased = context.canceled;

        if (jumpPressed && isGrounded)
        {
            rb.AddForce(Vector2.up * jumpForce);
        }

        if (jumpReleased && rb.velocity.y > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, 1f); //Slow down the jump as the player gets higher
        }
    }

    /// <summary>
    /// Flips the sprite direction left/right depending on key input
    /// </summary>
    private void UpdateSpriteDirection()
    {
        if (movementInput.x != 0)
        {
            sr.flipX = movementInput.x < 0;
        }
    }

    /// <summary>
    /// Check if the player should be walking or idle based on key inputs
    /// </summary>
    private void UpdateAnimationState()
    {
        if (movementInput.x != 0)  //If an input is being pressed
        {
            CurrentState = PlayerStates.WALK;
            animator.SetFloat("xMove", movementInput.x);
            animator.SetBool("isGrounded", isGrounded);
            animator.SetFloat("yVelocity", rb.velocity.y);
        }
        else
        {
            CurrentState = PlayerStates.IDLE;
        }
    }

    /// <summary>
    /// Possible animation states for the player character in the animator.
    /// </summary>
    public enum PlayerStates { IDLE, WALK, ATTACK, DIE }

    /// <summary>
    /// Handles the switching and setting of the current animation state.
    /// </summary>
    PlayerStates CurrentState { set {
            currentState = value;
            switch (currentState)
            {   case PlayerStates.IDLE:   animator.Play("Idle");   break;
                case PlayerStates.WALK:   animator.Play("Walk");   break;
                case PlayerStates.ATTACK: animator.Play("Attack"); break;
                case PlayerStates.DIE:    animator.Play("Die");    break;
                default: break;
            }
        }
    }

    /// <summary>
    /// Moves the player position to a set trigger location  <br />
    /// Increments a counter to show on endgame panel
    /// </summary>
    private void RespawnAtCheckpoint()
    {
        endGameManager.respawnCount++;
        transform.position = resetTrigger.respawnPosition;
    }
}