using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    InputAction moveAction;
    InputAction jumpAction;

    [SerializeField] private float groundSpeed = 5f;
    [SerializeField] private float jumpForce = 5f;

    private BoxCollider2D col;
    private Rigidbody2D rb;
    private bool onGround;

    private void Start()
    {
        moveAction = InputSystem.actions.FindAction("Move");
        jumpAction = InputSystem.actions.FindAction("Jump");
        col = GetComponent<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (IsOnGround())
        {
            onGround = true;
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
        }
        else
        {
            onGround = false;
        }
    }

    private static bool InBetween(float value, float min, float max)
    {
        return value > min && value < max;
    }

    private bool IsOnGround()
    {
        return col.IsTouchingLayers(8);
    }

    void Update()
    {
        Vector2 moveValue = moveAction.ReadValue<Vector2>();
        rb.linearVelocity = new Vector2(moveValue.x * groundSpeed, rb.linearVelocity.y);

        if (jumpAction.triggered && onGround)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }
    }
}
