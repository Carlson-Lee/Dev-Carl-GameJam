using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f; // Adjust the player's movement speed
    public float jumpForce = 10f; // Force applied when jumping
    public Rigidbody2D rb; // Reference to the player's Rigidbody component
    public SpriteRenderer spriteRenderer;
    public Transform groundCheck; // Position of the ground check object
    public float groundCheckRadius = 0.1f; // Radius for ground check
    public LayerMask platformLayer; // Layer(s) to consider as ground

    private Vector2 movement; // Stores the player's movement direction
    private bool isGrounded; // Flag to track if the player is grounded


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        // Ground check
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.1f, platformLayer);

        // Input
        if (Input.GetKeyDown(KeyCode.UpArrow) && isGrounded)
        {
            Debug.Log("Jump key pressed");
            Jump();
        }

        // Flip sprite based on movement direction
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        if (horizontalInput < 0)
        {
            spriteRenderer.flipX = true;
        }
        else if (horizontalInput > 0)
        {
            spriteRenderer.flipX = false;
        }

    }

    void FixedUpdate()
    {
        // Movement
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        MovePlayer(horizontalInput);

        // Apply gravity manually for more control over jump height
        if (!isGrounded)
        {
            rb.velocity += Vector2.down * Physics2D.gravity.y * Time.fixedDeltaTime;
        }
    }

    void MovePlayer(float horizontalInput)
    {
        // Apply horizontal movement
        Vector2 movementVelocity = new Vector2(horizontalInput * moveSpeed * Time.fixedDeltaTime, rb.velocity.y);
        rb.velocity = movementVelocity;
    }

    void Jump()
    {
        // Apply jump force
        Debug.Log("Jumping");
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
    }
}
