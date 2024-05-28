using UnityEngine;
using UnityEngine.InputSystem;

public class SideScrollerPlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float movementSpeed = 1f;
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private Vector2 movementInput;
    [SerializeField] private bool isGrounded = false;

    [Header("Components")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Animator animator;
    [SerializeField] private SpriteRenderer sr;
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private BoxCollider2D playerCollider;

    [Header("Ground Check")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckDistance = 0.2f;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private PlayerStates currentState; //Check for current animation needed

    [SerializeField] private float wallCheckDistance = 0.2f;
    [SerializeField] private LayerMask wallLayer;
    [SerializeField] private bool isTouchingWall = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        playerInput = GetComponent<PlayerInput>();
        playerCollider = GetComponent<BoxCollider2D>();

        // Switch to SideScroller action map
        playerInput.SwitchCurrentActionMap("SideScroller");
    }

    private void Update()
    {
        UpdateMovement();
        UpdateAnimationState();
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(movementInput.x * movementSpeed, rb.velocity.y);
        RaycastHit2D hit = Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, groundLayer);

        if (hit.collider != null)
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }

        // Wall Check
        RaycastHit2D wallHitRight = Physics2D.Raycast(transform.position, Vector2.right, wallCheckDistance, wallLayer);
        RaycastHit2D wallHitLeft = Physics2D.Raycast(transform.position, Vector2.left, wallCheckDistance, wallLayer);
        isTouchingWall = wallHitRight.collider != null || wallHitLeft.collider != null;

        // Movement
        if (!isTouchingWall && isGrounded)
        {
            rb.velocity = new Vector2(movementInput.x * movementSpeed, rb.velocity.y);
        }
        
        if (isTouchingWall)
        {
            playerCollider.enabled = false;
        }
        else
        {
            playerCollider.enabled = true;
        }
    }

    private void UpdateMovement()
    {
        if (movementInput.x != 0)
        {
            sr.flipX = movementInput.x < 0;
        }
    }

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

    private void OnMove(InputValue movementValue)
    {
        movementInput = movementValue.Get<Vector2>();
    }

    private void OnJump(InputValue jumpValue)
    {
        Debug.Log("Jump input received");
        if (jumpValue.isPressed && isGrounded)
        {
            Debug.Log("Jumping");
            rb.AddForce(Vector2.up * jumpForce);
        }
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
}