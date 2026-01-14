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
    [SerializeField] private Transform castLeft;
    [SerializeField] private Transform castLeftLimit;
    [SerializeField] private Transform castGround;
    [SerializeField] private Transform castGroundLimit;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private ContactFilter2D contactFilter;

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

    private int animatorHorizontal = Animator.StringToHash("ChasingHorizontal");
    private int animatorResting = Animator.StringToHash("ChasingEnemyResting");
    private int animatorLunge = Animator.StringToHash("ChasingEnemyLunge");
    [SerializeField] private Animator animatorChasing;


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

    public void ChangeState(EnemyStates state)
    {
        switch (state) {
            case EnemyStates.IDLE:
                chasingEnemyBaseState = new ChasingEnemyIdle();
		animatorChasing.SetBool(animatorResting, false);
                break;
            case EnemyStates.LUNGING:
                chasingEnemyBaseState = new ChasingEnemyLunging();
		animatorChasing.SetTrigger(animatorLunge);
                break;
            case EnemyStates.RESTING:
                chasingEnemyBaseState = new ChasingEnemyResting();
		animatorChasing.SetBool(animatorResting, true);
                break;
            case EnemyStates.INVISIBLE:
                chasingEnemyBaseState = new ChasingEnemyInvisible();
                break;
            case EnemyStates.JUMPING:
                chasingEnemyBaseState = new ChasingEnemyJump();
                break;
            default:
                break;
        }
        enemyState = state;
        chasingEnemyBaseState.BeginState(this);
    }

    public void ChasingEnemyResting(bool isResting)
    {
	    Debug.Log("is resting" + isResting);
	    animatorChasing.SetBool(animatorResting, isResting);
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
        animatorChasing.SetBool(animatorHorizontal, rb.linearVelocityX > 0);
        //Rotate();

        if (rb.linearVelocityX > 0)
        {
            Physics2D.Linecast(castRight.position, castRightLimit.position, contactFilter, hitRight);
        }
        else if (rb.linearVelocityX < 0)
        {
            Physics2D.Linecast(castLeft.position, castLeftLimit.position, contactFilter, hitRight);
        }
        _isGrounded = Physics2D.Linecast(castGround.position, castGroundLimit.position, groundLayer);
        Debug.DrawLine(castGround.position, castGroundLimit.position, Color.red);
        Debug.DrawLine(castRight.position, castRightLimit.position, Color.green);

        if (enemyState == EnemyStates.JUMPING && _isGrounded)
        {
            chasingEnemyBaseState.ExitState();
        }

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

    public void SpawnCoins()
    {
        for (int i = 0; i < chasingEnemySata.CoinNumber; ++i)
        {
            GameObject coin = Instantiate(chasingEnemySata.Coin, transform.position, transform.rotation);
            coin.GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.value, Random.value) * chasingEnemySata.CoinKnockback, ForceMode2D.Impulse);
        }
    }



}
