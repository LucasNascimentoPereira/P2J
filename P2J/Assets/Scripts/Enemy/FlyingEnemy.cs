using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEnemy : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private FlyingEnemyData chasingEnemySata;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Detector detector;
    [SerializeField] private Detector rangeDetector;
    [SerializeField] private Detector evadeDetector;
    private Detector patrolDetector;
    [SerializeField] private SpriteRenderer spriteRenderer;


    [Header("Positions of the limis")]
    [SerializeField] private List<Transform> patrolPoints;


    private int patrolIndex = 0;
    private Vector2 _dir = Vector2.zero;
    private bool _detectedPlayer = false;
    private bool _detectedPlayerEvade = false;
    private GameObject _player = null;
    //private bool _isGrounded = true;
    private bool _isJumping = false;
    private FlyingEnemyBaseState chasingEnemyBaseState;
    private EnemyStates enemyState = EnemyStates.IDLE;
    private Coroutine _timerCoroutine;

    public Rigidbody2D Rb { get => rb; set => rb = value; }
    public Vector2 Dir { get => _dir; set => _dir = value; }
    public FlyingEnemyData ChasingEnemySata => chasingEnemySata;
    public GameObject Player => _player;
    public bool IsJumping { get => _isJumping; set => _isJumping = value; }
    public bool DetectedPlayerCharacter { get => _detectedPlayer; set => _detectedPlayer = value; }
    public bool DetectedPlayerEvade { get => _detectedPlayerEvade; set => _detectedPlayerEvade= value; }

    public enum EnemyStates
    {
        IDLE,
        EVADE,
        RESTING,
        INVISIBLE,
        LUNGING,
        SHOOT
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
        switch (state)
        {
            case EnemyStates.IDLE:
                enemyState = EnemyStates.IDLE;
                chasingEnemyBaseState = new FlyingEnemIdle();
                break;
            case EnemyStates.EVADE:
                enemyState = EnemyStates.EVADE;
                chasingEnemyBaseState = new FlyingEnemyEvade();
                break;
            case EnemyStates.RESTING:
                enemyState = EnemyStates.RESTING;
                chasingEnemyBaseState = new FlyingEnemyResting();
                Debug.Log(enemyState);
                break;
            case EnemyStates.INVISIBLE:
                enemyState = EnemyStates.INVISIBLE;
                chasingEnemyBaseState = new FlyingEnemyInvisible();
                break;
            case EnemyStates.SHOOT:
                enemyState = EnemyStates.SHOOT;
                chasingEnemyBaseState = new FlyingEnemyShoot();
                break;
            case EnemyStates.LUNGING:
                enemyState=EnemyStates.LUNGING;
                chasingEnemyBaseState = new FlyingEnemyLunging();
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

    public void DetectedPlayerEvadeState()
    {
        _detectedPlayerEvade = !_detectedPlayerEvade;
    }

    public void NotInRange()
    {
        if (rangeDetector.Collider != null && rangeDetector.Collider.gameObject != gameObject) return;
        ChangeState(EnemyStates.IDLE);
    }

    private void FixedUpdate()
    {
        if (chasingEnemyBaseState == null) return;
        chasingEnemyBaseState.UpdateState();
        Rotate();

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

    public void Move()
    {
        if (enemyState != EnemyStates.IDLE) return;
        _dir = patrolPoints[patrolIndex].transform.position - gameObject.transform.position;
        _dir = _dir.normalized;
    }


    public void ChangeTarget(int index)
    {
        if (patrolDetector.Collider != null && patrolDetector.Collider.gameObject != gameObject) return;
        if(index != patrolIndex + 1 && index != 0) return;
        patrolIndex = index;
        Move();
    }

    public void PatrolDetector(Detector detector)
    {
        if (detector == null) return;
        ChangeState(EnemyStates.RESTING);
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

    public void ShootBullet()
    {
        GameObject bullet = Instantiate(chasingEnemySata.BulletPrefab, transform.position, transform.rotation);
        if(!bullet.TryGetComponent(out Bullet bulletScript)) return;
        bulletScript.Shoot(_player.transform.position - gameObject.transform.position, chasingEnemySata.BulletSpeed, chasingEnemySata.Damage, chasingEnemySata.KnockBack);
    }
}
