using UnityEngine;

public class SimpleDestructible : MonoBehaviour
{
    [SerializeField] private ParticleSystem particle;
    [SerializeField] private SpriteRenderer sprite;


    public void DestroyDestructible()
    {
        sprite.enabled = false;
        particle.Play();
        Destroy(gameObject, particle.main.duration);
    }
}
