using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HealthBase : MonoBehaviour
{
    [SerializeField] protected float maxHealth = 100;
    [SerializeField] protected float minHealth = 0;
    
    protected float currentHealth;
    protected bool _isDefeated = false;

    [SerializeField] protected UnityEvent onDefeat = new();
    [SerializeField] protected UnityEvent onDamage = new();

    public float MaxHealth => maxHealth;
    public float CurrentHealth => currentHealth;
    public bool IsDefeated => _isDefeated;

    protected virtual void Awake()
    {
        currentHealth = MaxHealth;
    }
    
    protected void CalculateHealth(float delta)
    {
        if (_isDefeated) return;
        currentHealth = CurrentHealth - delta;
        currentHealth = Mathf.Clamp(CurrentHealth, minHealth, MaxHealth);
        if (CurrentHealth <= minHealth) 
        {
            Death();
        }
        else 
        { 
            onDamage.Invoke(); 
        }
    }
    
    public virtual void TakeDamage(GameObject damageDealer, bool isDamage, float damage)
    {
        if (damageDealer == null || _isDefeated) return;
        CalculateHealth(damage);
    }

    public virtual void TakeDamage(GameObject damageDealer, bool isDamage, float damage, float force)
    {
        TakeDamage(damageDealer, isDamage, damage);
    }
    public virtual void TakeDamage(GameObject damageDealer, bool isDamage, float damage, float force, Vector2 dir)
    {
        TakeDamage(damageDealer, isDamage, damage);
    }

    protected virtual void Death()
    {
        _isDefeated = true;
        onDefeat.Invoke();
    }


}
