using UnityEngine;

public class HealthDoor : HealthBase
{
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
        //soundIndex = 0;
        onPlaySound.Invoke();
        audioSource.PlayOneShot(audioClips[0]);
        return true;
    }

    protected override void Death()
    {
        //soundIndex = 1;
        //onPlaySound.Invoke();
        audioSource.PlayOneShot(audioClips[0]);
        Destroy(gameObject);
    }
}
