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
    private UnityEvent _onChangeHealth = new();
    private UnityEvent _onChangeStatus = new();

    protected override void Awake()
    {
        base.Awake();
        GameManager.Instance.HealthPlayer = this;
        _onChangeHealth.AddListener(UIManager.Instance.ChangeHealth);
        _onChangeStatus.AddListener(GameManager.Instance.LevelReset);
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
        _onChangeHealth.Invoke();
        StartCoroutine(Invencibility());
        onPlaySoundDamage.Invoke();
        //onParticle.Invoke();
        return true;
    }

    public override bool TakeDamage(GameObject damageDealer, bool isDamage, float damage, float force)
    {
        if (isInvencible) return false;
        if (damageDealer == null) return false;
        CalculateHealth(damage);
        rb.AddForce(-rb.transform.right * force, ForceMode2D.Force);
        _onChangeHealth.Invoke();
        StartCoroutine(Invencibility());
        onPlaySoundDamage.Invoke();
        //onParticle.Invoke();
        return true;
    }

    protected override void Death()
    {
        Debug.Log(gameObject);
        rb.linearVelocity = Vector2.zero;
	animator.SetTrigger("PlayerDefeated");
        onPlaySoundDefeatRange.Invoke();
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
