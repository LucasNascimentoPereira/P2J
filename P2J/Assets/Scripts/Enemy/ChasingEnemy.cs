using System;
using System.Collections;
using UnityEngine;

public class ChasingEnemy : MonoBehaviour
{
    [SerializeField] private ChasingEnemySata chasingEnemySata;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Detector detector;
    private Vector2 _dir = Vector2.zero;
    private bool _detectedPlayer = false;
    private bool _isLunge = false;
    private GameObject _player = null;


    private void Start()
    {
        _player = GameManager.Instance.HealthPlayer.gameObject;
    }

    public void DetectedPlayer()
    {
        _detectedPlayer = !_detectedPlayer;
        if (_detectedPlayer)
        {
            StartCoroutine(LungeAttackRest());
        }
        else
        {
            StopCoroutine(LungeAttackRest());
        }
    }

    private void Update()
    {
        if (!_detectedPlayer) return;
        if (!_isLunge)
        {
            _dir = _player.transform.position - gameObject.transform.position;
            rb.linearVelocity = new Vector2(_dir.normalized.x, rb.linearVelocityY) * chasingEnemySata.ChaseSpeed; 
        }
        else
        {
            _dir = _player.transform.position - gameObject.transform.position;
            rb.linearVelocity = new Vector2(_dir.normalized.x, rb.linearVelocityY) * chasingEnemySata.LungeSpeed;
        }
        
    }

    private void Lunge()
    {
        _isLunge = false;
        StartCoroutine(LungeAttackRest());
    }

    private IEnumerator LungeAttackRest()
    {
        yield return new WaitForSeconds(chasingEnemySata.RestTime);
        _isLunge  = true;
        StartCoroutine(LungeAttack());
    }

    private IEnumerator LungeAttack()
    {
        yield return new WaitForSeconds(chasingEnemySata.LungeTime);
        _isLunge  = false;
    }
    
    public void Damage()
    {
        if (detector.Collider.TryGetComponent(out HealthPlayerBase healthPlayer))
        {
            healthPlayer.TakeDamage(gameObject, true, chasingEnemySata.Damage, chasingEnemySata.KnockBack);
        }
    }
}
