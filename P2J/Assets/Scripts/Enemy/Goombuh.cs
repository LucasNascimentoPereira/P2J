using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class Goombuh : MonoBehaviour
{
    [SerializeField] private GoombuhData goombuhData;
    [Header("Positions of the limis")]
    [SerializeField] private List<Transform> patrolPoints;
    [SerializeField] private Rigidbody2D rb;
    private int patrolIndex = 0;
    private Vector2 dir = Vector2.zero;
    [SerializeField] private Detector detector;
    [SerializeField] private SpriteRenderer spriteRenderer;

    private void Start()
    {
        Move();
    }

    private void Move()
    {
        dir = patrolPoints[patrolIndex].transform.position - gameObject.transform.position;
        dir = dir.normalized;
    }

    private void FixedUpdate()
    {   
        spriteRenderer.flipX = dir.x < 0;
        rb.linearVelocity = new Vector2(dir.x, 0) * goombuhData.GoombuhSpeed;
    }
    public void ChangeTarget(int index)
    {
        patrolIndex = index;
        Move();
    }

    public void Damage()
    {
        if (detector.Collider.TryGetComponent(out HealthPlayerBase healthPlayer))
        {
            healthPlayer.TakeDamage(gameObject, true, goombuhData.GoombuhDamage, goombuhData.GoombuhKnockback);
        }
    }


}
