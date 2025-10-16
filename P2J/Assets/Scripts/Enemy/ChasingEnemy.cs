using UnityEngine;

public class ChasingEnemy : MonoBehaviour
{
    [SerializeField] private ChasingEnemySata chasingEnemySata;
    [SerializeField] private Rigidbody2D rb;
    private Vector2 dir = Vector2.zero;
    private bool detectedPlayer = false;
    private GameObject player = null;

    public void DetectedPlayer()
    {
        if (player != null) return;
        player = GameManager.Instance.HealthPlayer.gameObject;
        detectedPlayer = true;
    }

    private void Update()
    {
        if (!detectedPlayer) return;
        dir = player.transform.position - gameObject.transform.position;
        rb.linearVelocity = new Vector2(dir.normalized.x, rb.linearVelocityY) * chasingEnemySata.ChaseSpeed; 
    }
}
