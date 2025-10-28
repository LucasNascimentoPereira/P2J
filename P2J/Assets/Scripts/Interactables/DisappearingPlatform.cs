using System.Collections;
using UnityEngine;

public class DisappearingPlatform : MonoBehaviour
{

    [SerializeField] private BoxCollider2D boxCollider2D;
    [SerializeField] private SpriteRenderer spriteRenderer;

    [Header("Timer values")]
    [Tooltip("This variable changes the amount the spriterenderer alpha value that increases over time")]
    [Range(0f, 1f)]
    [SerializeField] private float appearInterval = 0.1f;
    [Tooltip("This variable is the amount of time between alpha changes")]
    [Range(0f, 1f)]
    [SerializeField] private float appearTimeInterval = 0.1f;
    [Range(0f, 1f)]
    [SerializeField] private float disappearInterval = 0.1f;
    [Range (0f, 1f)]
    [SerializeField] private float disappearTimeInterval = 0.1f;
    private bool isGrounded;
    public void ChangeIsGrounded(bool value)
    {
        isGrounded = value;

    }

    private IEnumerator ChangeTransparency()
    {
        while (true)
        {
            spriteRenderer.color = isGrounded ? new Color(1.0f, 1.0f, 1.0f, Mathf.Clamp(spriteRenderer.color.a - appearInterval, 0.0f, 1.0f))
               : new Color(1.0f, 1.0f, 1.0f, Mathf.Clamp(spriteRenderer.color.a + appearInterval, 0.0f, 1.0f));
            if (spriteRenderer.color.a <= 0.0f)
            {
                boxCollider2D.enabled = false;
            }
            if (spriteRenderer.color.a >= 1.0f)
            {
                boxCollider2D.enabled = true;
            }
            yield return new WaitForSeconds(appearTimeInterval);
        }
    }
}
