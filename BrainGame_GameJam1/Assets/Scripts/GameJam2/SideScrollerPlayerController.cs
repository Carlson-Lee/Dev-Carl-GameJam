using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class SideScrollerPlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float movementSpeed;
    [SerializeField] private float jumpForce;
    [SerializeField] private Vector2 movementInput;
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

    [Header("Jump Settings")]
    [SerializeField] private float jumpHoldDuration = 0.5f; // Maximum time the jump can be held
    [SerializeField] private float jumpSpeed = 5f; // Jump speed
    private float lastGroundedAtTime = -1f;

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
        jumpForce = 150f;
        isGrounded = false;
        isTouchingWall = false;
        isFalling = false;
        wallCheckDistance = 0.05f;
        groundCheckDistance = 0.1f;
        wallPushModifier = 1f;
        groundCheckSize = new Vector2(0.2f, 0.1f);
    }

    private void Update()
    {
        UpdateSpriteDirection();
        UpdateAnimationState();

        // Ground Check using OverlapBox
        Collider2D groundHit = Physics2D.OverlapBox(groundCheck.position, groundCheckSize, 0, groundLayer);
        isGrounded = groundHit != null;

        // Track last grounded time
        if (isGrounded) lastGroundedAtTime = Time.time;

        // Handle jump
        if (Keyboard.current.spaceKey.isPressed && Time.time < lastGroundedAtTime + jumpHoldDuration)
        {
            rb.velocity = Vector2.up * jumpSpeed;
        }
    }

    private void FixedUpdate()
    {
        // Ground Check using OverlapBox
        Collider2D groundHit = Physics2D.OverlapBox(groundCheck.position, groundCheckSize, 0, groundLayer);
        isGrounded = groundHit != null;

        // Wall Check
        RaycastHit2D wallHitRight = Physics2D.Raycast(transform.position, Vector2.right, wallCheckDistance, wallLayer);
        RaycastHit2D wallHitLeft = Physics2D.Raycast(transform.position, Vector2.left, wallCheckDistance, wallLayer);
        isTouchingWall = wallHitRight.collider != null || wallHitLeft.collider != null;

        // Reset falling state if grounded
        if (isGrounded)
        {
            isFalling = false;
        }

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
                isFalling = true; // Set falling state
            }
            else if (wallHitLeft.collider != null)
            {
                rb.velocity = new Vector2(movementSpeed * wallPushModifier, rb.velocity.y); // Bounce to the right
                isFalling = true; // Set falling state
            }
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
}