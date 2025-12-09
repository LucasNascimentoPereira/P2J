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


    [SerializeField] private float groundSpeed = 3.34f;
    [SerializeField] private float jumpForce = 20f;
    [SerializeField] private Vector2 jumpForceWall = new Vector2(12f, 5f);
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
    [SerializeField] private float meleeCooldown = 0.3f;
    /*[SerializeField]*/ private float meleeSlowDuration = 0.2f;
    /*[SerializeField]*/ private Vector2 maxSpeedDuringMelee = new Vector2(2f, 3f);
    [SerializeField] private Animator _animatorController;
    private int animatorHorizontal = Animator.StringToHash("Horizontal");
    private int animatorVertical = Animator.StringToHash("Vertical");
    private int animatorJump = Animator.StringToHash("IsOnGround");
    private int animatorAttackRight = Animator.StringToHash("IsAttackingRight");
    private int animatorAttackLeft = Animator.StringToHash("IsAttackingLeft");
    private int animatorFacingRight = Animator.StringToHash("IsFacingRight");
    private int animatorJumpStart = Animator.StringToHash("JumpStarted");


    private CapsuleCollider2D col;
    private Rigidbody2D rb;
    private bool onGround;
    private bool onLeftWall;
    private bool onRightWall;
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
    private float attackTime = -1f;
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
        col = GetComponent<CapsuleCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = defaultGravity;
        jumpReleased = true;
        dashReleased = true;
        meleeReleased = true;
        playerDirectionIsRight = true;
        meleeDirection = Vector2.right * 0.5f;

        _onPlaySound.AddListener(PlaySound);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        checkForGround();
        checkForWalls();
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        checkForGround();
        checkForWalls();
    }

    private static bool InBetween(float value, float min, float max)
    {
        return value > min && value < max;
    }

    private void checkForGround()
    {
        if (IsOnGround())
        {
            onGround = true;
            airDashCount = 0;
            //check for jump buffering
            if (jumpPressTime != -1f && Time.fixedTime - jumpPressTime < jumpBufferTime)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
                jumpTime = jumpPressTime;
                jumpPressTime = -1f;
                _animatorController.SetTrigger(animatorJumpStart);
            }
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

    private void checkForWalls()
    {
        if (IsOnLeftWall())
        {
            onLeftWall = true;
        }
        else
        {
            onLeftWall = false;
        }
        if (IsOnRightWall())
        {
            onRightWall = true;
        }
        else
        {
            onRightWall = false;
        }
    }

    private bool IsOnGround()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(col.bounds.center, col.bounds.size * 0.9f, 0f, Vector2.down, 0.1f, groundLayer);
        return raycastHit.collider != null;
    }

    private bool IsOnLeftWall()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(col.bounds.center, col.bounds.size * 0.9f, 0f, Vector2.left, 0.1f, groundLayer);
        return raycastHit.collider != null;
    }

    private bool IsOnRightWall()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(col.bounds.center, col.bounds.size * 0.9f, 0f, Vector2.right, 0.1f, groundLayer);
        return raycastHit.collider != null;
    }

    private void Update()
    {
        Vector2 lookValue = lookAction.ReadValue<Vector2>();
        bool lookValueChanged = lookAction.triggered;
        checkForGround();
        checkForWalls();
        if (interactAction.WasPressedThisFrame())
        {
            Interact();
        }
        if (lookValueChanged && lookValue != Vector2.zero && Time.time > attackTime + meleeCooldown)
        {
            if (meleeReleased)
            {
                attackWithMelee(playerDirectionIsRight, lookValue);
                if(lookValue.x > 0.1f)
                {
                    _animatorController.SetTrigger(animatorAttackRight);
                }
                if(lookValue.x < -0.1f)
                {
                    _animatorController.SetTrigger(animatorAttackLeft);
                }
                if(lookValue.y > 0.1f)
                {
                }
                if(lookValue.y < -0.1f)
                {
                }
            }
            meleeReleased = false;
        }
        else
        {
            meleeReleased = true;
        }
        _animatorController.SetFloat(animatorVertical, rb.linearVelocityY);
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
                //attackWithMelee(playerDirectionIsRight, lookValue);
            }
            //meleeReleased = false;
        }
        else
        {
            //meleeReleased = true;
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

        if (Time.fixedTime - attackTime < meleeSlowDuration)
        {
            if (rb.linearVelocity.x > maxSpeedDuringMelee.x)
            {
                rb.linearVelocityX = maxSpeedDuringMelee.x;
            }
            else if (rb.linearVelocity.x < -maxSpeedDuringMelee.x)
            {
                rb.linearVelocityX = -maxSpeedDuringMelee.x;
            }
            if (rb.linearVelocity.y > maxSpeedDuringMelee.y)
            {
                rb.linearVelocityY = maxSpeedDuringMelee.y;
            }
            else if (rb.linearVelocity.y < -maxSpeedDuringMelee.y)
            {
                rb.linearVelocityY = -maxSpeedDuringMelee.y;
            }
        }
        _animatorController.SetFloat(animatorHorizontal, moveValue.x);
        _animatorController.SetFloat(animatorVertical, rb.linearVelocityY);
        _animatorController.SetBool(animatorJump, onGround);
        _animatorController.SetBool(animatorFacingRight, playerDirectionIsRight);
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
        //regular jump
        if (onGround && jumpReleased)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            jumpTime = Time.fixedTime;
            _animatorController.SetTrigger(animatorJumpStart);
        }
        //wall jump
        else if (onLeftWall && jumpReleased)
        {
            rb.linearVelocity = jumpForceWall;
            jumpTime = Time.fixedTime;
        }
        else if (onRightWall && jumpReleased)
        {
            rb.linearVelocity = new Vector2(-jumpForceWall.x, jumpForceWall.y);
            jumpTime = Time.fixedTime;
        }
        //variable jump height
        else if (jumpTime != -1f && Time.fixedTime - jumpTime < maxJumpDuration)
        {
            rb.gravityScale = defaultGravity / gravityResistanceOnJump;
        }
        //check for coyote time
        else if (dropTime != -1f && Time.fixedTime - dropTime < coyoteTime && jumpReleased)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            jumpTime = Time.fixedTime;
            dropTime = -1f;
            _animatorController.SetTrigger(animatorJumpStart);
        }
        //store time for jump buffering
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
        attackTime = Time.fixedTime;
        soundIndex = 0;
        //_onPlaySound.Invoke();
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
