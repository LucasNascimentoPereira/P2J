using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChasingEnemy : MonoBehaviour
{
    [SerializeField] private ChasingEnemySata chasingEnemySata;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Detector detector;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Transform castLeft;
    [SerializeField] private Transform castLeftLimit;
    [SerializeField] private Transform castRight;
    [SerializeField] private Transform castRightLimit;
    [SerializeField] private Transform castGround;
    [SerializeField] private Transform castGroundLimit;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private ContactFilter2D contactFilter;
    [Header("Positions of the limis")]
    [SerializeField] private List<Transform> patrolPoints;
    private int patrolIndex = 0;
    private Vector2 dir = Vector2.zero;
    private List<RaycastHit2D> hitLeft = new();
    private List<RaycastHit2D> hitRight = new();
    private Vector2 _dir = Vector2.zero;
    private bool _detectedPlayer = false;
    private bool _isLunge = false;
    private GameObject _player = null;
    private bool _isGrounded = true;


    private void Start()
    {
        _player = GameManager.Instance.HealthPlayer.gameObject;
        Move();
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

    private void FixedUpdate()
    {
        spriteRenderer.flipX = dir.x < 0;
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
        spriteRenderer.flipX = _dir.x < 0;
        Physics2D.Linecast(castLeft.position, castLeftLimit.position, contactFilter, hitLeft);
        Physics2D.Linecast(castRight.position, castRightLimit.position, contactFilter, hitRight);
        _isGrounded = Physics2D.Linecast(castGround.position, castGroundLimit.position, groundLayer);
        Debug.DrawLine(castGround.position, castGroundLimit.position, Color.red);
        Debug.DrawLine(castLeft.position, castLeftLimit.position, Color.red);
        Debug.DrawLine(castRight.position, castRightLimit.position, Color.red);
        
        if ((hitLeft.Count != 0 || hitRight.Count != 0) && _isGrounded)
        {
            Debug.Log(_isGrounded);
            rb.AddForce(new Vector2(0.0f, chasingEnemySata.Jump), ForceMode2D.Force);
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

    private void Move()
    {
        dir = patrolPoints[patrolIndex].transform.position - gameObject.transform.position;
        dir = dir.normalized;
    }
    public void ChangeTarget(int index)
    {
        patrolIndex = index;
        Move();
    }

}
