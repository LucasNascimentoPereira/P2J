using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;

    private Vector2 dir;
    private float speed;
    private float damage;
    private float knockBack;


    public void Shoot(Vector2 bulletDir, float bulletSpeed, float bulletDamage, float bulletKnockBack)
    {
        dir = bulletDir;
        speed = bulletSpeed;
        damage = bulletDamage;
        knockBack = bulletKnockBack;
        rb.linearVelocity = dir.normalized * speed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision.gameObject.tag);
        switch (collision.gameObject.tag)
        {
            case "Player":
                if (!collision.gameObject.TryGetComponent(out HealthPlayerBase healthPlayerBase)) break;
                healthPlayerBase.TakeDamage(gameObject, true, damage, knockBack);
                break;
            case "Enemy":
                Debug.Log("enemy");
                break;
            case "Ground":
                Debug.Log("Ground");
                Destroy(gameObject);
                break;
            default :
                break;
        }
    }
}
