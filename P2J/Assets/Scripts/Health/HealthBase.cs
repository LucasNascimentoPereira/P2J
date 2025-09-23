using UnityEngine;
using UnityEngine.Events;

public class HealthBase : MonoBehaviour
{
    [SerializeField] protected float maxHealth = 100;
    [SerializeField] protected float minHealth = 0;
    
    protected float currentHealth;
    private bool _isDefeated = false;

    [SerializeField] private UnityEvent onDefeat;
    [SerializeField] private UnityEvent onDamage;

    [SerializeField] protected AudioSource audioSource;

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
        currentHealth = CurrentHealth + delta;
        currentHealth = Mathf.Clamp(CurrentHealth, minHealth, MaxHealth);
        onDamage.Invoke();
        if (CurrentHealth <= minHealth) Death();
    }

    public virtual bool TakeDamage(GameObject damageObject, float damage)
    {
        return false;
    }

    public virtual bool TakeDamage(GameObject damageDealer, float damage, bool knockBack, float force)
    {
        return false;
    }
    
    protected virtual void Death()
    {
        _isDefeated = true;
        UIManager.Instance.ShowPanel("LevelReset");
        onDefeat.Invoke();
    }


}
