using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

public class SideScrollerPlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float movementSpeed;
    [SerializeField] private float jumpForce;
    [SerializeField] public Vector2 movementInput;
    [SerializeField] private PlayerStates currentState; //Check for current animation needed

    [Header("CollisionDetection")]
    [SerializeField] private bool isGrounded;
    [SerializeField] private bool isTouchingWall;
    [SerializeField] private bool isFalling;
    //Walls
    [SerializeField] public float wallCheckDistance;
    [SerializeField] private LayerMask wallLayer;
    //Ground
    [SerializeField] private Transform groundCheck;
    [SerializeField] private Vector2 groundCheckSize;
    [SerializeField] public float groundCheckDistance;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float wallPushModifier;

    [Header("Components")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Animator animator;
    [SerializeField] private SpriteRenderer sr;
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private ResetPlayerPosition resetTrigger;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        playerInput = GetComponent<PlayerInput>();

        // Switch to SideScroller action map
        playerInput.SwitchCurrentActionMap("SideScroller");

        //Set all variables
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

    private void Update()
    {
        UpdateSpriteDirection();
        UpdateAnimationState();

        if (Input.GetKey(KeyCode.R))
        {
            RespawnAtCheckpoint();
        }
    }

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


    /// <summary>
    /// Saves the players input direction for movement
    /// </summary>
    /// <param name="movementValue"></param>
    private void OnMove(InputValue movementValue) { movementInput = movementValue.Get<Vector2>(); }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="jumpValue"></param>
    private void OnJump(InputValue jumpValue)
    {
        if (jumpValue.isPressed && isGrounded)
        {
            rb.AddForce(Vector2.up * jumpForce);
        }
    }

    public void Move(InputAction.CallbackContext context)
    {
        movementInput = context.ReadValue<Vector2>();
    }

    public void Jump(InputAction.CallbackContext context)
    {
        bool jumpPressed = context.performed;
        bool jumpReleased = context.canceled;

        if (jumpPressed && isGrounded)
        {
            rb.AddForce(Vector2.up * jumpForce);
        }

        if (jumpReleased && rb.velocity.y > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, 1f);
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

    private void RespawnAtCheckpoint()
    {
        transform.position = resetTrigger.respawnPosition;
    }
}