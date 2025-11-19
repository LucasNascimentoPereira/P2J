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
        soundIndex = 0;
        //onPlaySound.Invoke();
        //audioSource.PlayOneShot();
        return true;
    }

    public override bool TakeDamage(GameObject damageDealer, bool isDamage, float damage, float force)
    {
        if (damageDealer == null) return false;
        CalculateHealth(damage);
        rb.AddForce(-rb.linearVelocity.normalized * force, ForceMode2D.Force);
        soundIndex = 0;
        //onPlaySound.Invoke();
        //audioSource.PlayOneShot();
        return true;
    }

    public bool TakeDamage(GameObject damageDealer, bool isDamage, float damage, float force, Vector2 dir)
    {
        if (damageDealer == null) return false;
        CalculateHealth(damage);
        rb.AddForce(-rb.linearVelocity.normalized * force, ForceMode2D.Force);
        soundIndex = 0;
        //onPlaySound.Invoke();
        //audioSource.PlayOneShot();
        return true;
    }

    protected override void Death()
    {
        soundIndex = 1;
        //onPlaySound.Invoke();
        Destroy(gameObject);
    }
}
