using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    InputAction moveAction;
    InputAction jumpAction;

    public float groundSpeed = 5f;
    public float jumpForce = 5f;

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
        if (IsCollisionOnBottom(collision))
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
        onGround = false;
    }

    private bool IsCollisionOnBottom(Collision2D collision)
    {
        Bounds bounds = col.bounds;

        foreach (ContactPoint2D contact in collision.contacts)
        {
            if (contact.point.y < bounds.min.y + 0.1f)
            {
                return true;
            }
        }
        return false;
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
