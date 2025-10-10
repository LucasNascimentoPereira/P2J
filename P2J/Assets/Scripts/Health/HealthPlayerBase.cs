using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class HealthPlayerBase : HealthBase
{
    [SerializeField] private Rigidbody2D rb;

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
        UIManager.Instance.ChangeHealth(isDamage, (int)currentHealth);
        //audioSource.PlayOneShot();
        return true;
    }

    public override bool TakeDamage(GameObject damageDealer, bool isDamage, float damage, float force)
    {
        if (damageDealer == null) return false;
        CalculateHealth(damage);
        rb.AddForce(-rb.transform.right * force, ForceMode2D.Force);
        Debug.Log(currentHealth);
        UIManager.Instance.ChangeHealth(isDamage, (int)currentHealth);
        //audioSource.PlayOneShot();
        return true;
    }

    protected override void Death()
    {
        Debug.Log(gameObject);
        GameManager.Instance.LevelReset(gameObject);
    }

}
