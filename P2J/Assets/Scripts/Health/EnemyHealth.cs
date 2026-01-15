using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class EnemyHealth : HealthBase
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Collider2D _collider2D;
    [SerializeField] private GameObject enemy;
    private SpawnArea _mySpawnArea;
    public SpawnArea MySpawnArea { get => _mySpawnArea; set => _mySpawnArea = value; }

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
        onPlaySoundDamage.Invoke();
        return true;
    }

    public override bool TakeDamage(GameObject damageDealer, bool isDamage, float damage, float force)
    {
        if (damageDealer == null) return false;
        CalculateHealth(damage);
        rb.AddForce(-rb.linearVelocity.normalized * force, ForceMode2D.Force);
        onPlaySoundDamageRange.Invoke();
        //onParticle.Invoke();
        return true;
    }

    public override bool TakeDamage(GameObject damageDealer, bool isDamage, float damage, float force, Vector2 dir)
    {
        if (damageDealer == null) return false;
        CalculateHealth(damage);
        rb.AddForce(dir.normalized * force, ForceMode2D.Impulse);
        onPlaySoundDamageRange.Invoke();
        //onParticle.Invoke();
        return true;
    }

    protected override void Death()
    {
        onPlaySoundDefeatRange.Invoke();
        onDefeat.Invoke();
	Debug.Log("eeeeeee");
	if (_mySpawnArea != null)
	{
		Debug.Log("eeeeeeeeeeee");
		_mySpawnArea.RemoveEnemy(transform.parent.gameObject);
	}
	Destroy(enemy, 1.0f);
    }
}
