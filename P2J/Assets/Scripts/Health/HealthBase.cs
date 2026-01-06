using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HealthBase : MonoBehaviour
{
    [SerializeField] protected float maxHealth = 100;
    [SerializeField] protected float minHealth = 0;
    
    protected float currentHealth;
    private bool _isDefeated = false;

    [SerializeField] protected UnityEvent onDefeat;
    [SerializeField] protected UnityEvent onDamage;
    [SerializeField] protected UnityEvent onPlaySoundDamage = new();
    [SerializeField] protected UnityEvent onPlaySoundDamageRange = new();
    [SerializeField] protected UnityEvent onPlaySoundDefeat = new();
    [SerializeField] protected UnityEvent onPlaySoundDefeatRange = new();
    [SerializeField] protected UnityEvent onParticle = new();

    [SerializeField] protected List<ParticleSystem> particleSystemList;
    protected int particleIndex;

    public float MaxHealth => maxHealth;
    public float CurrentHealth => currentHealth;
    public bool IsDefeated => _isDefeated;

    protected virtual void Awake()
    {
        currentHealth = MaxHealth;
        onParticle.AddListener(PlayParticle);
    }
    
    protected void CalculateHealth(float delta)
    {
        if (_isDefeated) return;
        currentHealth = CurrentHealth - delta;
        currentHealth = Mathf.Clamp(CurrentHealth, minHealth, MaxHealth);
        onDamage.Invoke();
        if (CurrentHealth <= minHealth) Death();
    }
    
    public virtual bool TakeDamage(GameObject damageObject, bool isDamage, float damage)
    {
        return false;
    }

    public virtual bool TakeDamage(GameObject damageDealer, bool isDamage, float damage, float force)
    {
        return false;
    }
    public virtual bool TakeDamage(GameObject damageDealer, bool isDamage, float damage, float force, Vector2 dir)
    {
        return false;
    }

    protected virtual void Death()
    {
        _isDefeated = true;
        //UIManager.Instance.ShowPanel("LevelReset");
        onDefeat.Invoke();
    }

    protected virtual void PlayParticle()
    {
        if (!particleSystemList[particleIndex]) return;
      //  particleSystemList[particleIndex].Play();
    }


}
