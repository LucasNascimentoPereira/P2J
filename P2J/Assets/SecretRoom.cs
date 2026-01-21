using System.Collections;
using UnityEngine;

public class SecretRoom : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _spriteRend;
    [SerializeField] private float _fadeTime;
    private bool _isUnlocked = false;

    public void Unlock()
    {
        if (_isUnlocked) return;
        _isUnlocked = true;
        StartCoroutine(FadeOut());
    }

    IEnumerator FadeOut()
    {
        float alpha = _spriteRend.color.a;

        while (alpha > 0)
        {
            alpha -= Time.deltaTime * (1/_fadeTime);
            _spriteRend.color = new Color(_spriteRend.color.r, _spriteRend.color.g, _spriteRend.color.b, alpha);
            yield return null;
        }

    }
}
