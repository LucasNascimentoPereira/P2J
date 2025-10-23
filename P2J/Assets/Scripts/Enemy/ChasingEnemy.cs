using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChasingEnemy : MonoBehaviour
{

    [Header("Components")]
    [SerializeField] private ChasingEnemySata chasingEnemySata;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Detector detector;
    [SerializeField] private SpriteRenderer spriteRenderer;

    [Header("RayCasts")]
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
    private List<RaycastHit2D> hitLeft = new();
    private List<RaycastHit2D> hitRight = new();
    private Vector2 _dir = Vector2.zero;
    private bool _detectedPlayer = false;
    private GameObject _player = null;
    private bool _isGrounded = true;
    private bool _isJumping = false;
    private ChasingEnemyBaseState chasingEnemyBaseState;
    private EnemyStates enemyState = EnemyStates.IDLE;

    public Rigidbody2D Rb { get => rb; set => rb = value; }
    public Vector2 Dir { get => _dir; set => _dir = value; }
    public ChasingEnemySata ChasingEnemySata => chasingEnemySata;
    public GameObject Player => _player;
    public bool IsJumping { get => _isJumping; set => _isJumping = value; }
    public bool DetectedPlayerCharacter { get => _detectedPlayer; set => _detectedPlayer = value; }

    public enum EnemyStates
    {
        IDLE,
        LUNGING,
        RESTING,
        JUMPING,
        INVISIBLE
    }

    private void Start()
    {
        _player = GameManager.Instance.HealthPlayer.gameObject;
        ChangeState(EnemyStates.IDLE);
    }
    private void OnBecameVisible()
    {
        //ChangeState(EnemyStates.IDLE);
    }
    private void OnBecameInvisible()
    {
        //ChangeState(EnemyStates.INVISIBLE);
    }

    public void ChangeState(EnemyStates state)
    {
        switch (state){
            case EnemyStates.IDLE:
                enemyState = EnemyStates.IDLE;
                chasingEnemyBaseState = new ChasingEnemyIdle();
                break;
            case EnemyStates.LUNGING:
                enemyState = EnemyStates.LUNGING;
                chasingEnemyBaseState = new ChasingEnemyLunging();
                break;
            case EnemyStates.RESTING:
                enemyState = EnemyStates.RESTING;
                chasingEnemyBaseState = new ChasingEnemyResting();
                Debug.Log(enemyState);
                break;
            case EnemyStates.INVISIBLE:
                enemyState = EnemyStates.INVISIBLE;
                chasingEnemyBaseState = new ChasingEnemyInvisible();
                break;
            case EnemyStates.JUMPING:
                enemyState = EnemyStates.JUMPING;
                chasingEnemyBaseState = new ChasingEnemyJump();
                break;
            default:
                break;
        }
        chasingEnemyBaseState.BeginState(this);
    }

    public void DetectedPlayer()
    {
        _detectedPlayer = true;
        chasingEnemyBaseState.ExitState();
    }

    public void NotDetectedPlayer()
    {
        _detectedPlayer = false;
        chasingEnemyBaseState.ExitState();
    }

    private void FixedUpdate()
    {
        if (chasingEnemyBaseState == null) return;
        chasingEnemyBaseState.UpdateState();
        spriteRenderer.flipX = _dir.x < 0;


        Physics2D.Linecast(castLeft.position, castLeftLimit.position, contactFilter, hitLeft);
        Physics2D.Linecast(castRight.position, castRightLimit.position, contactFilter, hitRight);
        _isGrounded = Physics2D.Linecast(castGround.position, castGroundLimit.position, groundLayer);
        Debug.DrawLine(castGround.position, castGroundLimit.position, Color.red);
        Debug.DrawLine(castLeft.position, castLeftLimit.position, Color.red);
        Debug.DrawLine(castRight.position, castRightLimit.position, Color.red);

        if (enemyState == EnemyStates.JUMPING && _isGrounded)
        {
            chasingEnemyBaseState.ExitState();
        }

        if ((hitLeft.Count != 0 || hitRight.Count != 0) && _isGrounded && enemyState != EnemyStates.IDLE && enemyState != EnemyStates.JUMPING)
        {
            ChangeState(EnemyStates.JUMPING);
        }

    }
    
    public void Damage()
    {
        chasingEnemyBaseState.ExitState();
        Debug.Log(enemyState);
        if (detector.Collider.TryGetComponent(out HealthPlayerBase healthPlayer))
        {
            healthPlayer.TakeDamage(gameObject, true, chasingEnemySata.Damage, chasingEnemySata.KnockBack);
        }
    }

    public void Move()
    {
        if (enemyState != EnemyStates.IDLE) return;
        _dir = patrolPoints[patrolIndex].transform.position - gameObject.transform.position;
        _dir = _dir.normalized;
    }


    public void ChangeTarget(int index)
    {
        patrolIndex = index;
        Move();
    }

    private IEnumerator IdleTime(float time)
    {
        yield return new WaitForSeconds(time);
        chasingEnemyBaseState.ExitState();
    }
    public void BeginIdleTime(float time)
    {
        StartCoroutine(IdleTime(time));
    }
    public void EndIdleTime() 
    {
        StopCoroutine(IdleTime(0.0f));
    }
}