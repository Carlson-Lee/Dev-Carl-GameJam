using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{

    [Header("Movement Settings")]
    public Vector2 movementInput;
    public float movementSpeed;

    [Header("Filters")]
    public ContactFilter2D movementFilter;

    [Header("Collisions")]
    public Rigidbody2D rb;
    public List<RaycastHit2D> castCollisions;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        movementSpeed = 1f;
        castCollisions = new List<RaycastHit2D>();
    }

    //Handles player inputs to move (x,y) position
    void OnMove(InputValue movementValue)
    {
        movementInput = movementValue.Get<Vector2>();
    }

    private bool TryMove(Vector2 direction)
    {
        bool canMove;

        int count = rb.Cast( //Check for collisions in input direction
            movementInput,   //Direction to check collisions
            movementFilter,  //Check which collisions can occur
            castCollisions,  //Stored collisions from raycast
            movementSpeed * Time.fixedDeltaTime //Amount to cast for collision check
            );
        if (count == 0) //== //Only move if there is no collision
        {
            rb.MovePosition(rb.position + (direction * movementSpeed * Time.fixedDeltaTime));
            canMove = true;
        }
        else { canMove = false; }
        return canMove;
    }

    private void FixedUpdate()
    {
        //Wait for user input before moving
        if (movementInput != Vector2.zero)
        {
            bool success = TryMove(movementInput);

            if (!success) //Object has collision, will try to move on seperate axis to slide around
            {
                success = TryMove(new Vector2(movementInput.x, 0)); //Test for movement only on x-input-axis

                if (!success)
                {
                    success = TryMove(new Vector2(0, movementInput.y)); //Test for movement only on y-input-axis
                }
            }
            
        }
    }

}