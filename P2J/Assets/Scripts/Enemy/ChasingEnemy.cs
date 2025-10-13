using UnityEngine;

public class ChasingEnemy : MonoBehaviour
{
    [SerializeField] private ChasingEnemySata chasingEnemySata;
    [SerializeField] private Rigidbody2D rb;
    private Vector2 dir = Vector2.zero;


    private void Update()
    {
        dir = gameObject.transform.position - GameManager.Instance.GetPlayerPosition();
        rb.linearVelocity = new Vector2(dir.normalized.x, 0) * chasingEnemySata.ChaseSpeed; 
    }
}
