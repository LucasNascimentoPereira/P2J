using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    InputAction moveAction;
    InputAction jumpAction;

    public float groundSpeed = 5f;

    private Collider col;
    private Rigidbody2D rb;

    private void Start()
    {
        moveAction = InputSystem.actions.FindAction("Move");
        jumpAction = InputSystem.actions.FindAction("Jump");
        col = GetComponent<Collider>();
        rb = GetComponentInChildren<Rigidbody2D>();
    }

    void Update()
    {
        Vector2 moveValue = moveAction.ReadValue<Vector2>();
        rb.linearVelocity = new Vector2(moveValue.x * groundSpeed, rb.linearVelocity.y);

        if (jumpAction.triggered)
        {
            // your jump code here
        }
    }
}
