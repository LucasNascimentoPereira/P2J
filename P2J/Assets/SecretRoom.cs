using System.Collections;
using UnityEngine;

public class SecretRoom : MonoBehaviour
{
    private SpriteRenderer _spriteRend;
    [SerializeField]
    private float _fadeTime;
    private bool _hasBeenEntered = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _spriteRend = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !_hasBeenEntered)
        {
            StartCoroutine(FadeOut());
        }
    }

    IEnumerator FadeOut()
    {
        float alpha = 1;

        while (alpha > 0)
        {
            alpha -= Time.deltaTime * (1/_fadeTime);
            _spriteRend.color = new Color(_spriteRend.color.r, _spriteRend.color.g, _spriteRend.color.b, alpha);
            yield return null;
        }

        _hasBeenEntered = true;
    }
}
