using UnityEngine;

public class Liquid : MonoBehaviour
{
    [SerializeField] private int damage = 1000;
    [SerializeField] private Detector detector;
    public void Damage()
    {
        if (!detector.gameObject.TryGetComponent(out HealthPlayerBase healthPlayerBase)) return;
        healthPlayerBase.TakeDamage(gameObject, true, damage);
    }
}
