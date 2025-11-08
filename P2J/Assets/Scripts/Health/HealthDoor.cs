using System.Collections;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

public class HealthDoor : HealthBase
{

    [SerializeField] private ParticleSystem doorParticleSystem;
    [SerializeField] private GameObject door;
    [SerializeField] private List<GameObject> doorBits;
    [SerializeField] private int index = 0;

    [Header("Variables to use noise")]
    [Tooltip("Interval between noise")]
    [SerializeField] private float interval = 0.1f;
    [SerializeField] private float noiseTime = 1.0f;
    [Tooltip("Magnitude of noise")]
    [Range(0.0f, 1.0f)]
    [SerializeField] private Vector2 noise = Vector2.one;
    private Coroutine _timerCoroutine;

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
        onPlaySound.Invoke();
        return true;
    }

    protected override void Death()
    {
        //soundIndex = 1;
        onPlaySound.Invoke();
        Destroy(gameObject);
    }

    protected override void PlaySound()
    {
        base.PlaySound();
        doorParticleSystem.Play();
        if (_timerCoroutine != null)
        {
            StopCoroutine(_timerCoroutine);
            _timerCoroutine = null;
        }
        else
        {
            _timerCoroutine = StartCoroutine(Noise());
        }
        ++index;
        if (!doorBits[index].TryGetComponent(out SpriteRenderer spriteRenderer)) return;
        spriteRenderer.enabled = false;
    }

    private IEnumerator Noise()
    {
        float enlapedTime = 0.0f;
        while (enlapedTime < noiseTime) {
            door.transform.position = new Vector2(Random.value, Random.value) * noise;
            enlapedTime += interval;
            yield return interval;
        }
    }
}
