using System.Collections.Generic;
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
    [SerializeField] protected UnityEvent onPlaySound = new();
    [SerializeField] protected UnityEvent onParticle = new();

    [SerializeField] protected AudioSource audioSource;
    [SerializeField] protected List<AudioClip> audioClips;
    protected int soundIndex;

    [SerializeField] protected List<ParticleSystem> particleSystemList;
    protected int particleIndex;

    public float MaxHealth => maxHealth;
    public float CurrentHealth => currentHealth;
    public bool IsDefeated => _isDefeated;

    protected virtual void Awake()
    {
        currentHealth = MaxHealth;
        onPlaySound.AddListener(PlaySound);
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
        UIManager.Instance.ShowPanel("LevelReset");
        onDefeat.Invoke();
    }

    protected virtual void PlaySound()
    {
        if (!audioClips[soundIndex]) return;
        audioSource.PlayOneShot(audioClips[soundIndex]);
    }

    protected virtual void PlayParticle()
    {
        if (!particleSystemList[particleIndex]) return;
        particleSystemList[particleIndex].Play();
    }


}
