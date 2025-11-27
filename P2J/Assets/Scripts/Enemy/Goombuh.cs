using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class Goombuh : MonoBehaviour
{
    [SerializeField] private GoombuhData goombuhData;
    [Header("Positions of the limis")]
    [SerializeField] private List<Transform> patrolPoints;
    [SerializeField] private Rigidbody2D rb;
    private int patrolIndex = 0;
    private Vector2 dir = Vector2.zero;
    [SerializeField] private Detector detector;

    private bool knockBack = false;
    private Coroutine coroutine;

    private void OnBecameVisible()
    {
        Move();
    }

    private void OnBecameInvisible()
    {
        dir = Vector2.zero;
    }

    private void Move()
    {
        dir = patrolPoints[patrolIndex].transform.position - gameObject.transform.position;
        dir = dir.normalized;
    }

    private void FixedUpdate()
    {
        //Rotate();
        if (knockBack) return;
        rb.linearVelocity = new Vector2(dir.x * goombuhData.GoombuhSpeed, rb.linearVelocityY);
        //animator.SetFloat(animatorHorizontal, rb.linearVelocityX);
    }

    private void Rotate()
    {
        if (transform.localEulerAngles.y != 180 && rb.linearVelocityX < 0)
        {
            transform.Rotate(0.0f, 180.0f, 0.0f);
        }
        else if (transform.localEulerAngles.y != 0 && rb.linearVelocityX > 0)
        {
            transform.Rotate(0.0f, -180.0f, 0.0f);
        }
    }

    public void ChangeTarget(int index)
    {
        patrolIndex = index;
        Move();
    }

    public void Damage()
    {
        if (detector.Collider.TryGetComponent(out HealthPlayerBase healthPlayer))
        {
            healthPlayer.TakeDamage(gameObject, true, goombuhData.GoombuhDamage, goombuhData.GoombuhKnockback);
        }
    }

    public void KnockBack()
    {
        if (coroutine == null)
        {
            coroutine = StartCoroutine(KnockBackTime());
        }
        else
        {
            StopCoroutine(coroutine);
            coroutine = StartCoroutine(KnockBackTime());
        }
    }

    private IEnumerator KnockBackTime()
    {
        knockBack = true;
        yield return new WaitForSeconds(goombuhData.KnockbackTime);
        knockBack = false;
        coroutine = null;
    }


}
