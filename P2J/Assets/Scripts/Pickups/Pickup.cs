using UnityEngine;
using UnityEngine.Events;

public class Pickup : MonoBehaviour
{
    [SerializeField] protected UnityEvent _onPickup = new();
    
    protected virtual void Awake()
    {
        
    }

    public virtual void PickUp()
    {
        _onPickup.Invoke();
        Destroy(gameObject);
    }
}
