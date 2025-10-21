using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class EnemyHealth : HealthBase
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private SpriteRenderer spriteRenderer;

    protected override void Awake()
    {
        base.Awake();
    }

    public void LevelReset()
    {
        currentHealth = maxHealth;
    }

    public override bool TakeDamage(GameObject damageDealer, bool isDamage, float damage)
    {
        if (damageDealer == null) return false;
        CalculateHealth(damage);
        //audioSource.PlayOneShot();
        return true;
    }

    public override bool TakeDamage(GameObject damageDealer, bool isDamage, float damage, float force)
    {
        if (damageDealer == null) return false;
        CalculateHealth(damage);
        rb.AddForce(-rb.transform.right * force, ForceMode2D.Force);
        //audioSource.PlayOneShot();
        return true;
    }

    protected override void Death()
    {
        Destroy(gameObject);
    }
}
