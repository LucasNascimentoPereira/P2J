using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class DisappearingPlatform : MonoBehaviour
{

    [SerializeField] private BoxCollider2D boxCollider2D;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private UnityEvent onPlatform;
    [SerializeField] private Animator _myAnimator;
    private int animatorPlatformIdle = Animator.StringToHash("PlatformIdle");

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
    [Tooltip("Time it takes for the platform to begin appearing again")]
    [Range (0f, 10f)]
    [SerializeField] private float timeToAppear = 1f;
    private bool isDissappearing = false;

    public void DisappearPlatform()
    {
        if (isDissappearing) return;
        StartCoroutine(ChangeTransparency());
        onPlatform.Invoke();
	isDissappearing = true;
	_myAnimator.SetBool(animatorPlatformIdle, isDissappearing);
    }

    private IEnumerator ChangeTransparency()
    {
        while (boxCollider2D.enabled)
        {
            spriteRenderer.color = new Color(1.0f, 1.0f, 1.0f, Mathf.Clamp(spriteRenderer.color.a - disappearInterval, 0.0f, 1.0f));
            if (spriteRenderer.color.a <= 0.0f)
            {
                boxCollider2D.enabled = false;
            }
            yield return new WaitForSeconds(disappearTimeInterval);
        }
        yield return new WaitForSeconds (timeToAppear);
        while (!boxCollider2D.enabled)
        {
            spriteRenderer.color = new Color(1.0f, 1.0f, 1.0f, Mathf.Clamp(spriteRenderer.color.a + appearInterval, 0.0f, 1.0f));
            if(spriteRenderer.color.a >= 1.0f)
            {
                boxCollider2D.enabled = true;
            }
            yield return new WaitForSeconds(appearTimeInterval);
        }
        isDissappearing = false ;
	_myAnimator.SetBool(animatorPlatformIdle, isDissappearing);
    }
}
