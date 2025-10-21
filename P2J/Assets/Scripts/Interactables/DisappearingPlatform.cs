using System.Collections;
using UnityEngine;

public class DisappearingPlatform : MonoBehaviour
{
    [SerializeField] private BoxCollider2D boxCollider2D;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private float interval = 0.1f;
    [SerializeField] private float timeInterval = 0.1f;
    private bool isGrounded;

    private void OnBecameVisible()
    {
        StartCoroutine(ChangeTransparency());
    }
    private void OnBecameInvisible()
    {
        StopCoroutine(ChangeTransparency());
    }
    public void ChangeIsGrounded(bool value)
    {
        isGrounded = value;
        Debug.Log(isGrounded);
    }

    private IEnumerator ChangeTransparency()
    {
        while (true)
        {
            spriteRenderer.color = isGrounded ? new Color(1.0f, 1.0f, 1.0f, Mathf.Clamp(spriteRenderer.color.a - interval, 0.0f, 1.0f))
               : new Color(1.0f, 1.0f, 1.0f, Mathf.Clamp(spriteRenderer.color.a + interval, 0.0f, 1.0f));
            if (spriteRenderer.color.a <= 0.0f)
            {
                boxCollider2D.enabled = false;
            }
            if (spriteRenderer.color.a >= 1.0f)
            {
                boxCollider2D.enabled = true;
            }
            yield return new WaitForSeconds(timeInterval);
        }
    }
}
