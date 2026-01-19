using System.Collections;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.Events;

public class EnemyHealth : HealthBase
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private GameObject enemy;
    [SerializeField] private float defeatTime = 1.5f;

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

    public override void TakeDamage(GameObject damageDealer, bool isDamage, float damage, float force, Vector2 dir)
    {
        base.TakeDamage(damageDealer, isDamage, damage, force, dir);
        rb.AddForce(dir.normalized * force, ForceMode2D.Impulse);
    }

    protected override void Death()
    {
        base.Death();
	    if (_mySpawnArea != null)
	    {
		    _mySpawnArea.RemoveEnemy(transform.parent.gameObject);
	    }
	    Destroy(enemy, defeatTime);
    }
}
