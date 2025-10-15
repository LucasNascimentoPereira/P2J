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
    private UnityEvent _onChangeHealth = new();
    private UnityEvent _onChangeStatus = new();

    public Rigidbody2D Rb => rb;

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
        _onChangeHealth.Invoke();
        //audioSource.PlayOneShot();
        StartCoroutine(Invencibility());
        return true;
    }

    protected override void Death()
    {
        Debug.Log(gameObject);
        _onChangeStatus.Invoke();
        Debug.Log(rb.linearVelocity);
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
