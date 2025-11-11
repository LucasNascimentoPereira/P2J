using UnityEngine;
using UnityEngine.InputSystem;
using System;

using Unity.VisualScripting;
using UnityEngine.Events;
using System.Collections.Generic;


public class PlayerController : MonoBehaviour
{
    InputAction moveAction;
    InputAction jumpAction;
    InputAction meleeAction;
    InputAction dashAction;
    InputAction lookAction;

    InputAction interactAction;


    [SerializeField] private float groundSpeed = 10f;
    [SerializeField] private float jumpForce = 20f;
    [SerializeField] private float defaultGravity = 5f;
    [SerializeField] private float maxFallingSpeed = 50f;
    /*[SerializeField]*/ private float accelerationFactorGround = 0.15f;
    /*[SerializeField]*/ private float deccelerationFactorGround = 0.5f;
    /*[SerializeField]*/ private float accelerationFactorAir = 0.08f;
    /*[SerializeField]*/ private float deccelerationFactorAir = 0.15f;
    /*[SerializeField]*/ private float maxJumpDuration = 0.3f;
    /*[SerializeField]*/ private float gravityResistanceOnJump = 10f;
    /*[SerializeField]*/ private float postJumpDecceleration = 5f;
    [SerializeField] private float coyoteTime = 0.1f;
    [SerializeField] private float jumpBufferTime = 0.1f;
    [SerializeField] private float dashForce = 25f;
    [SerializeField] private float dashCooldown = 1f;
    [SerializeField] private float dashDuration = 0.2f;
    [SerializeField] private LayerMask enemyLayer = 64;
    [SerializeField] private LayerMask groundLayer = 8;
    [SerializeField] private bool dashUnlocked = false;
    [SerializeField] private int  meleeDamage = 1;
    [SerializeField] private float meleeKnockback = 10.0f;
    [SerializeField] private float meleeRange = 3.0f;

    private BoxCollider2D col;
    private Rigidbody2D rb;
    private bool onGround;
    private bool jumpReleased;
    private bool dashReleased;
    private bool meleeReleased;
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
    private bool playerDirectionIsRight;

    private UnityEvent _onPlaySound = new();
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private List<AudioClip> _audioClips;
    private int soundIndex;


    private void Start()
    {
        moveAction = InputSystem.actions.FindAction("Move");
        jumpAction = InputSystem.actions.FindAction("Jump");
        meleeAction = InputSystem.actions.FindAction("Attack");
        dashAction = InputSystem.actions.FindAction("Dash");
        lookAction = InputSystem.actions.FindAction("Look");
        interactAction = InputSystem.actions.FindAction("Interact");
        col = GetComponent<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = defaultGravity;
        jumpReleased = true;
        dashReleased = true;
        meleeReleased = true;
        playerDirectionIsRight = true;
        meleeDirection = Vector2.right * 0.5f;
        //playerDirection = true;

        _onPlaySound.AddListener(PlaySound);
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
            if (jumpReleased && onGround)
            {
                dropTime = Time.fixedTime;
            }
            onGround = false;
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

    private void Update()
    {
        if (interactAction.WasPressedThisFrame())
        {
            Interact();
        }
    }

    void FixedUpdate()
    {
        rb.gravityScale = defaultGravity;
        Vector2 moveValue = moveAction.ReadValue<Vector2>();
        Vector2 lookValue = lookAction.ReadValue<Vector2>();
        processMovement(moveValue);
        processDirection(moveValue);

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
        if (rb.linearVelocity.y > 0)
        {
            if ((jumpReleased /*&& Time.fixedTime - jumpTime <= maxJumpDuration*/)
                /*|| Time.fixedTime - jumpTime > maxJumpDuration*/)
            {
                rb.gravityScale = defaultGravity * postJumpDecceleration;
            }
        }

        if (meleeAction.IsPressed())
        {
            if (meleeReleased)
            {
                attackWithMelee(playerDirectionIsRight, lookValue);
            }
            meleeReleased = false;
        }
        else
        {
            meleeReleased = true;
        }

        if (dashUnlocked && dashAction.IsPressed())
        {
            dash(playerDirectionIsRight);
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

        if (rb.linearVelocity.y < -maxFallingSpeed)
        {
            rb.gravityScale = 0f;
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
            rb.gravityScale = defaultGravity / gravityResistanceOnJump;
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

    void attackWithMelee(bool playerDirection, Vector2 lookValue)
    {
        if (lookValue == Vector2.zero)
        {
            if (playerDirection)
            {
                meleeDirection = Vector2.right * meleeRange / 2f;
            }
            else
            {
                meleeDirection = Vector2.left * meleeRange / 2f;
            }
        } 
        else
        {
            meleeDirection = lookValue * meleeRange / 2f;
        }
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(rb.position + meleeDirection, meleeRange / 2f, enemyLayer);
        Debug.Log(hitEnemies.Length + " enemies hit");
        Debug.DrawLine(rb.position, rb.position + meleeDirection * 2f);
        foreach (Collider2D enemy in hitEnemies)
        {
            Debug.Log(enemy.gameObject);
            if (enemy.TryGetComponent(out HealthBase enemyHealth))
            {
                enemyHealth.TakeDamage(gameObject, true, meleeDamage, meleeKnockback);
            }
        }
        soundIndex = 0;
        _onPlaySound.Invoke();
    }

    void processDirection(Vector2 moveValue) {
        if (moveValue.x > 0)
        {
            playerDirectionIsRight = true;
        }
        else if (moveValue.x < 0)
        {
            playerDirectionIsRight = false;
        }
    }

    private void Interact()
    {
        if (!GameManager.Instance.Interactable) return;
        if (!GameManager.Instance.Interactable.TryGetComponent(out IInteractable interactable)) return;
        interactable.Interact();
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

    private void dash(bool playerDirecton)
    {
        if (dashReleased)
        {
            if (onGround && (groundDashTime == -1f || Time.fixedTime - groundDashTime > dashCooldown))
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
                dashTime = Time.fixedTime;
                groundDashTime = Time.fixedTime;
            }
            else if (!onGround && airDashCount == 0)
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
                dashTime = Time.fixedTime;
                airDashCount++;
            }
        }
    }

    private void PlaySound()
    {
        _audioSource.PlayOneShot(_audioClips[soundIndex]);
    }
}
