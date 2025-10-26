using UnityEngine;
using UnityEngine.InputSystem;
using System;
using Unity.VisualScripting;

public class PlayerController : MonoBehaviour
{
    InputAction moveAction;
    InputAction jumpAction;
    InputAction meleeAction;
    InputAction dashAction;

    [SerializeField] private float groundSpeed = 5f;
    [SerializeField] private float jumpForce = 11f;
    [SerializeField] private float defaultGravity = 2f;
    [SerializeField] private float accelerationFactorGround = 0.15f;
    [SerializeField] private float deccelerationFactorGround = 0.38f;
    [SerializeField] private float accelerationFactorAir = 0.08f;
    [SerializeField] private float deccelerationFactorAir = 0.15f;
    [SerializeField] private float maxJumpDuration = 0.3f;
    [SerializeField] private float coyoteTime = 0.1f;
    [SerializeField] private float jumpBufferTime = 0.1f;
    [SerializeField] private float dashForce = 10f;
    [SerializeField] private float dashCooldown = 1.0f;
    [SerializeField] private float dashDuration = 0.2f;
    [SerializeField] private LayerMask enemyLayer = 64;
    [SerializeField] private LayerMask groundLayer = 8;
    [SerializeField] private bool dashUnlocked = false;

    private BoxCollider2D col;
    private Rigidbody2D rb;
    private bool onGround;
    private bool jumpReleased;
    private bool dashReleased;
    private float currentAccelerationFactor;
    private float currentDeccelerationFactor;
    private float jumpTime = -1f;
    private float jumpPressTime = -1f;
    private float dropTime = -1f;
    private float groundDashTime = -1f;
    private float dashTime = -1f;
    private int airDashCount = 0;
    private Vector2 meleeDirection;
    private Vector2 dashDirection;
    private bool playerDirection;

    private IInteractable interactable = null;

    private void Start()
    {
        moveAction = InputSystem.actions.FindAction("Move");
        jumpAction = InputSystem.actions.FindAction("Jump");
        meleeAction = InputSystem.actions.FindAction("Attack");
        dashAction = InputSystem.actions.FindAction("Dash");
        col = GetComponent<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = defaultGravity;
        jumpReleased = true;
        dashReleased = true;
        meleeDirection = Vector2.right * 0.5f;
        playerDirection = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (IsOnGround())
        {
            onGround = true;
            airDashCount = 0;
            if (jumpPressTime != -1f && Time.fixedTime - jumpPressTime < jumpBufferTime)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
                jumpTime = jumpPressTime;
                jumpPressTime = -1f;
            }
        }
        else
        {
            onGround = false;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (IsOnGround())
        {
            onGround = true;
            airDashCount = 0;
        }
        else
        {
            onGround = false;
            if (jumpReleased)
            {
                dropTime = Time.fixedTime;
            }
        }
    }

    private static bool InBetween(float value, float min, float max)
    {
        return value > min && value < max;
    }

    private bool IsOnGround()
    {
        return col.IsTouchingLayers(groundLayer);
    }

    void FixedUpdate()
    {
        Vector2 moveValue = moveAction.ReadValue<Vector2>();
        processMovement(moveValue);
        processDirection(moveValue);
        rb.gravityScale = defaultGravity;

        if (jumpAction.IsPressed())
        {
            tryToJump();
            jumpReleased = false;
        }
        else
        {
            jumpReleased = true;
        }
        if (jumpReleased)
        {
            jumpTime = -1f;
        }

        if (meleeAction.IsPressed())
        {
            attackWithMelee(meleeDirection);
        }

        if (dashUnlocked && dashAction.IsPressed())
        {
            dash(moveValue, playerDirection);
            dashReleased = false;
        }
        else
        {
            dashReleased = true;
        }
        if (Time.fixedTime - dashTime < dashDuration)
        {
            rb.gravityScale = 0f;
            rb.linearVelocity = new Vector2(dashForce * dashDirection.x, dashForce * dashDirection.y);
        }
    }

    void processMovement(Vector2 moveValue)
    {
        if (onGround)
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
                // If velocity exceeds max velocity, apply decceleration
                if (rb.linearVelocity.x > 0f)
                {
                    rb.linearVelocity = new Vector2(rb.linearVelocity.x - groundSpeed * currentDeccelerationFactor + moveValue.x * groundSpeed * currentAccelerationFactor, rb.linearVelocity.y);
                }
                else if (rb.linearVelocity.x < 0f)
                {
                    rb.linearVelocity = new Vector2(rb.linearVelocity.x + groundSpeed * currentDeccelerationFactor + moveValue.x * groundSpeed * currentAccelerationFactor, rb.linearVelocity.y);
                }
            }
        }
    }

    void tryToJump()
    {
        if (onGround && jumpReleased)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            jumpTime = Time.fixedTime;
        }
        else if (jumpTime != -1f && Time.fixedTime - jumpTime < maxJumpDuration)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }
        else if (dropTime != -1f && Time.fixedTime - dropTime < coyoteTime && jumpReleased)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            jumpTime = Time.fixedTime;
            dropTime = -1f;
        }
        else if (jumpReleased)
        {
            jumpPressTime = Time.fixedTime;
        }
    }

    void attackWithMelee(Vector2 meleeDirection)
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(rb.position + meleeDirection, 0.5f, enemyLayer);
        Debug.Log(hitEnemies.Length + " enemies hit");
        foreach (Collider2D enemy in hitEnemies)
        {
            Debug.Log(enemy.gameObject);
            if (enemy.TryGetComponent(out HealthBase enemyHealth))
            {
                //enemy taking damage code goes here (I think)
            }
        }
    }

    void processDirection(Vector2 moveValue) {
        if (moveValue.x > 0)
        {
            meleeDirection = Vector2.right * 0.5f;
            playerDirection = true;

        }
        else if (moveValue.x < 0)
        {
            meleeDirection = Vector2.left * 0.5f;
            playerDirection = false;
        }
    }

    public void RegisterInteractable(IInteractable envInteractable)
    {
        interactable = envInteractable;
    }

    private void Interact()
    {
        interactable?.Interact();
    }

    public void DashAbilityUnlock()
    {
        Debug.Log("Unlocked dash");
        dashUnlocked = true;
    }

    public void JumpAbilityUnlock()
    {
        Debug.Log("Unlocked jump");
    }

    private void dash(Vector2 moveValue, bool playerDirecton)
    {
        if (dashReleased)
        {
            if (onGround && (dashTime == -1f || Time.fixedTime - dashTime > dashCooldown))
            {
                if (moveValue == Vector2.zero)
                {
                    if (playerDirecton)
                    {
                        rb.linearVelocity = new Vector2(dashForce, 0);
                        dashDirection = Vector2.right;
                    }
                    else
                    {
                        rb.linearVelocity = new Vector2(-dashForce, 0);
                        dashDirection = Vector2.left;
                    }
                }
                else
                {
                    rb.linearVelocity = new Vector2(dashForce * moveValue.x, dashForce * moveValue.y);
                    dashDirection = moveValue;
                }
                dashTime = Time.fixedTime;
                groundDashTime = Time.fixedTime;
            }
            else if (!onGround && airDashCount == 0)
            {
                if (moveValue == Vector2.zero)
                {
                    if (playerDirecton)
                    {
                        rb.linearVelocity = new Vector2(dashForce, 0);
                        dashDirection = Vector2.right;
                    }
                    else
                    {
                        rb.linearVelocity = new Vector2(-dashForce, 0);
                        dashDirection = Vector2.left;
                    }
                }
                else
                {
                    rb.linearVelocity = new Vector2(dashForce * moveValue.x, dashForce * moveValue.y);
                    dashDirection = moveValue;
                }
                dashTime = Time.fixedTime;
                airDashCount++;
            }
        }
    }
}
