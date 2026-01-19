using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class HealthPlayerBase : HealthBase
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private PlayerData playerData;
    [SerializeField] private SpriteRenderer spriteRenderer;
    private bool isInvencible;
    [SerializeField] private Animator animator;

    protected override void Awake()
    {
        base.Awake();
        GameManager.Instance.HealthPlayer = this;
        onDamage.AddListener(UIManager.Instance.ChangeHealth);
        onDefeat.AddListener(GameManager.Instance.LevelReset);
    }

    public void LevelReset()
    {
        currentHealth = maxHealth;
    }

    public override void TakeDamage(GameObject damageDealer, bool isDamage, float damage, float force)
    {
        if (isInvencible) return;
        base.TakeDamage(damageDealer, isDamage, damage);
        rb.AddForce(-rb.transform.right * force, ForceMode2D.Force);
        StartCoroutine(Invencibility());
    }

    protected override void Death()
    {
        Debug.Log(gameObject);
        rb.linearVelocity = Vector2.zero;
	    animator.SetTrigger("PlayerDefeated");
        GameManager.Instance.Invoke("LevelReset", 1.2f);
    }
    
    private IEnumerator Invencibility()
    {
        isInvencible = true;
        StartCoroutine(InvencibilityEffect());
        yield return new WaitForSeconds(playerData.PlayerInvencibilityTime);
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
