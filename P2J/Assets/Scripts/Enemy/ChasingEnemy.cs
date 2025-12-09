using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ChasingEnemy : MonoBehaviour
{

    [Header("Components")]
    [SerializeField] private ChasingEnemySata chasingEnemySata;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Detector detector;
    [SerializeField] private Detector rangeDetector;
    private Detector patrolDetector;
    [SerializeField] private SpriteRenderer spriteRenderer;

    [Header("RayCasts")]
    [SerializeField] private Transform castRight;
    [SerializeField] private Transform castRightLimit;
    [SerializeField] private Transform castGround;
    [SerializeField] private Transform castGroundLimit;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private ContactFilter2D contactFilter;

    [SerializeField] protected UnityEvent onPlaySound;

    [SerializeField] protected AudioSource audioSource;
    [SerializeField] protected List<AudioClip> audioClips;
    protected int soundIndex;


    [Header("Positions of the limis")]
    [SerializeField] private List<Transform> patrolPoints;


    private int patrolIndex = 0;
    private List<RaycastHit2D> hitRight = new();
    private Vector2 _dir = Vector2.zero;
    private bool _detectedPlayer = false;
    private GameObject _player = null;
    private bool _isGrounded = true;
    private bool _isJumping = false;
    private ChasingEnemyBaseState chasingEnemyBaseState;
    private EnemyStates enemyState = EnemyStates.IDLE;
    private Coroutine _timerCoroutine;

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
        onPlaySound.AddListener(PlaySound);
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
        switch (state) {
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

    public void NotInRange()
    {
        if (rangeDetector.Collider.gameObject != null && rangeDetector.Collider.gameObject != gameObject) return;
        ChangeState(EnemyStates.IDLE);
    }

    private void FixedUpdate()
    {
        if (chasingEnemyBaseState == null) return;
        chasingEnemyBaseState.UpdateState();
        Rotate();

        Physics2D.Linecast(castRight.position, castRightLimit.position, contactFilter, hitRight);
        _isGrounded = Physics2D.Linecast(castGround.position, castGroundLimit.position, groundLayer);
        Debug.DrawLine(castGround.position, castGroundLimit.position, Color.red);
        Debug.DrawLine(castRight.position, castRightLimit.position, Color.green);

        if (enemyState == EnemyStates.JUMPING && _isGrounded)
        {
            chasingEnemyBaseState.ExitState();
        }

        //if (hitRight.Count != 0 && _isGrounded && enemyState != EnemyStates.IDLE && enemyState != EnemyStates.JUMPING)
        //{
            //ChangeState(EnemyStates.JUMPING);
        //}

        if (hitRight.Count != 0 && _isGrounded && enemyState != EnemyStates.JUMPING)
        {
            ChangeState(EnemyStates.JUMPING);
        }

    }

    private void Rotate()
    {
        if (transform.localEulerAngles.y != 180 && _dir.x < 0)
        {
            transform.Rotate(0.0f, 180.0f, 0.0f);
        }
        else if (transform.localEulerAngles.y != 0 && _dir.x > 0)
        {
            transform.Rotate(0.0f, -180.0f, 0.0f);
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
        onPlaySound.Invoke();
    }

    public void Move()
    {
        if (enemyState != EnemyStates.IDLE) return;
        _dir = patrolPoints[patrolIndex].transform.position - gameObject.transform.position;
        _dir = _dir.normalized;
    }


    public void ChangeTarget(int index)
    {
        if (patrolDetector.Collider != null && patrolDetector.Collider.gameObject != gameObject) return;
        patrolIndex = index;
        Move();
    }

    public void PatrolDetector(Detector detector)
    {
        if (detector == null) return;
        patrolDetector = detector;
    }

    private IEnumerator IdleTime(float time)
    {
        yield return new WaitForSeconds(time);
        _timerCoroutine = null;
        if (chasingEnemyBaseState != null)
        {
            chasingEnemyBaseState.ExitState();
        }
    }
    public void BeginIdleTime(float time)
    {
        EndIdleTime();
        _timerCoroutine = StartCoroutine(IdleTime(time));
    }
    public void EndIdleTime()
    {
        if (_timerCoroutine != null)
        {
            StopCoroutine(_timerCoroutine);
            _timerCoroutine = null;
        }
    }

    private void PlaySound()
    {
        audioSource.PlayOneShot(audioClips[soundIndex]);
    }
   

}