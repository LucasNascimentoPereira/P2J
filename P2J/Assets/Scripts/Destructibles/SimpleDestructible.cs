using UnityEngine;

public class SimpleDestructible : MonoBehaviour
{
    [SerializeField] private ParticleSystem particle;
    [SerializeField] private SpriteRenderer sprite;
    [SerializeField] private Collider2D _collider2D;


    public void DestroyDestructible()
    {
        sprite.enabled = false;
        particle.Play();
        _collider2D.enabled = false;
        Destroy(gameObject, particle.main.duration);
    }
}
