using Unity.VisualScripting;
using UnityEngine;

public class PlatformEnemy : MonoBehaviour
{
    [SerializeField] private float graivtyyy = 10.0f;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float speed = 1.0f;
    [SerializeField] private float ray = 1.5f;
    [SerializeField] private float rotation = 10.0f;
    [SerializeField] private LayerMask groundLayer;

    private Vector2 dir;

    private void FixedUpdate()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, -transform.up * ray, groundLayer);
        Debug.DrawRay(transform.position, transform.up * ray, Color.green);

        if (hit.collider != null)
        {
            Vector2 surface = hit.normal;

            Quaternion targetRotation = Quaternion.FromToRotation(transform.up, surface) * transform.rotation;
            targetRotation = Quaternion.Lerp(transform.rotation, targetRotation, rotation * Time.fixedDeltaTime);

            rb.AddForce(-surface * graivtyyy);

        }
        rb.linearVelocity = Vector2.Lerp(rb.linearVelocity, transform.right * speed, 0.1f);
    }
}
