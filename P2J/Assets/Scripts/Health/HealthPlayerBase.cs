using UnityEngine;
using UnityEngine.EventSystems;

public class HealthPlayerBase : HealthBase
{

    protected override void Awake()
    {
        base.Awake();
    }

    public void LevelReset()
    {
        currentHealth = maxHealth;
    }

    public override bool TakeDamage(GameObject damageDealer, float damage)
    {
        if (damageDealer == null) return false;
        CalculateHealth(Mathf.Abs(damage) * -1);
        //audioSource.PlayOneShot();
        return true;
    }

    protected override void Death()
    {
        GameManager.Instance.LevelReset();
    }

}
