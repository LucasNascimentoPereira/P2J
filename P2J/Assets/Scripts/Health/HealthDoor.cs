using System.Collections;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

public class HealthDoor : HealthBase
{

    [SerializeField] private ParticleSystem doorParticleSystem;
    [SerializeField] private GameObject door;

    [Header("Variables to use noise")]
    [Tooltip("Interval between noise")]
    //[SerializeField] private float interval = 0.1f;
    //[SerializeField] private float noiseTime = 1.0f;
    //[Tooltip("Magnitude of noise")]
    //[Range(0.0f, 1.0f)]
    //[SerializeField] private float noise = 0.0f;
    private Coroutine _timerCoroutine;

    protected override void Awake()
    {
        base.Awake();
    }

    public void LevelReset()
    {
        currentHealth = maxHealth;
    }

    public override bool TakeDamage(GameObject damageDealer, bool isDamage, float damage, float force, Vector2 dir)
    {
        if (damageDealer == null) return false;
        CalculateHealth(damage);
        onPlaySoundDamage.Invoke();
        return true;
    }

    protected override void Death()
    {
        onPlaySoundDefeatRange.Invoke();
        Destroy(gameObject);
    }
}
