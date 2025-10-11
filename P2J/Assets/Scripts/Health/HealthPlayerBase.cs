using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class HealthPlayerBase : HealthBase
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private PlayerData playerData;
    [SerializeField] private SpriteRenderer spriteRenderer;
    private bool isInvencible;

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
        if (isInvencible) return false;
        if (damageDealer == null) return false;
        CalculateHealth(damage);
        UIManager.Instance.ChangeHealth(isDamage, (int)currentHealth);
        //audioSource.PlayOneShot();
        StartCoroutine(Invencibility());
        return true;
    }

    public override bool TakeDamage(GameObject damageDealer, bool isDamage, float damage, float force)
    {
        if (isInvencible) return false;
        if (damageDealer == null) return false;
        CalculateHealth(damage);
        rb.AddForce(-rb.transform.right * force, ForceMode2D.Force);
        UIManager.Instance.ChangeHealth(isDamage, (int)currentHealth);
        //audioSource.PlayOneShot();
        StartCoroutine(Invencibility());
        return true;
    }

    protected override void Death()
    {
        Debug.Log(gameObject);
        GameManager.Instance.LevelReset(gameObject);
    }
    
    private IEnumerator Invencibility()
    {
        isInvencible = true;
        StartCoroutine(InvencibilityEffect());
        yield return playerData.PlayerInvencibilityTime;
        isInvencible = false;
    }

    private IEnumerator InvencibilityEffect()
    {
        int currentBlinks = 0;
        while (currentBlinks < playerData.PlayerInvencibilityBlinks)
        {
            spriteRenderer.enabled = false;
            yield return new WaitForSeconds(playerData.PlayerInvencibilityTime / (playerData.PlayerInvencibilityBlinks * 2));
            spriteRenderer.enabled = true;
            currentBlinks++;
            yield return new WaitForSeconds(playerData.PlayerInvencibilityTime / (playerData.PlayerInvencibilityBlinks * 2));
        }
        yield return null;
    }

}
