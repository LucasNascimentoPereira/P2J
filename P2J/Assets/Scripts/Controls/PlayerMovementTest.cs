using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class PlayerMovementTest : MonoBehaviour
{
    InputAction moveAction;
    InputAction jumpAction;
    private static Action Jump;

    [SerializeField] private float groundSpeed = 5f;
    [SerializeField] private float jumpForce = 11f;
    [SerializeField] private float defaultGravity = 2f;
    [SerializeField] private float accelerationFactorGround = 0.02f;
    [SerializeField] private float deccelerationFactorGround = 0.05f;
    [SerializeField] private float accelerationFactorAir = 0.01f;
    [SerializeField] private float deccelerationFactorAir = 0.01f;

    private BoxCollider2D col;
    private Rigidbody2D rb;
    private float currentAccelerationFactor;
    private float currentDeccelerationFactor;
    private bool jumpPressed;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        moveAction = InputSystem.actions.FindAction("Move");
        jumpAction = InputSystem.actions.FindAction("Jump");
        col = GetComponent<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = defaultGravity;
        jumpPressed = false;
    }

    private void OnEnable()
    {
        // Subscribe the TryToJump function to the Jump action
        Jump += TryToJump;
    }

    private void OnDisable()
    {
        Jump -= TryToJump;
    }

    // Update is called once per frame
    void Update()
    {
        if (jumpAction.WasPressedThisFrame())
        {
            jumpPressed = true;
            Jump?.Invoke();
        } else if (jumpAction.WasReleasedThisFrame())
        {
            jumpPressed = false;
        }
    }

    private void FixedUpdate()
    {
        Vector2 moveValue = moveAction.ReadValue<Vector2>();
        processMovement(moveValue);

        if (!jumpPressed && rb.linearVelocityY > 0)
        {
            // If the player has released the jump button
            // And we are still going up
            // Reduce the y velocity by multiplying with 0.2
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * 0.2f);
        }

        if (rb.linearVelocityY < 0)
        {
            // If the player is falling, make gravity stronger
            rb.gravityScale = defaultGravity * 2.5f;
        } else
        {
            // If the player is going up, use the default gravity
            rb.gravityScale = defaultGravity;
        }
    }

    void processMovement(Vector2 moveValue)
    {
        if (IsOnGround())
        {
            currentAccelerationFactor = accelerationFactorGround;
            currentDeccelerationFactor = deccelerationFactorGround;
        }
        else
        {
            currentAccelerationFactor = accelerationFactorAir;
            currentDeccelerationFactor = deccelerationFactorAir;
        }

        // If player stops moving, apply decceleration
        if (moveValue.x == 0f && rb.linearVelocity.x != 0)
        {
            if (rb.linearVelocity.x > 0f)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x - groundSpeed * currentDeccelerationFactor, rb.linearVelocity.y);
                if (rb.linearVelocity.x < 0f)
                {
                    rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);
                }
            }
            else if (rb.linearVelocity.x < -0f)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x + groundSpeed * currentDeccelerationFactor, rb.linearVelocity.y);
                if (rb.linearVelocity.x > 0f)
                {
                    rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);
                }
            }
            else
            {
                rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);
            }
        }

        // If player starts moving in the opposite direction, apply decceleration and acceleration
        else if (moveValue.x * rb.linearVelocity.x < 0)
        {
            if (rb.linearVelocity.x > 0f)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x - groundSpeed * currentDeccelerationFactor + moveValue.x * groundSpeed * currentAccelerationFactor, rb.linearVelocity.y);
            }
            else if (rb.linearVelocity.x < 0f)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x + groundSpeed * currentDeccelerationFactor + moveValue.x * groundSpeed * currentAccelerationFactor, rb.linearVelocity.y);
            }
        }
        else
        {
            // If player is moving, apply acceleration
            if (Math.Abs(rb.linearVelocity.x) < groundSpeed)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x + moveValue.x * groundSpeed * currentAccelerationFactor, rb.linearVelocity.y);
            }
            else
            {
                rb.linearVelocity = new Vector2(moveValue.x * groundSpeed, rb.linearVelocity.y);
            }
        }
    }

    private void TryToJump()
    {
        if (IsOnGround())
        {
            // If the player is on the ground, then we jump
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }
    }

    private bool IsOnGround()
    {
        return col.IsTouchingLayers(8);
    }
}
