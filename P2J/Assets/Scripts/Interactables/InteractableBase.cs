using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;
using NUnit.Framework.Constraints;

public class InteractableBase : MonoBehaviour, IInteractable
{

    [SerializeField] protected UnityEvent _onInteract = new();
    

    protected virtual void Start()
    {
    }

    protected virtual void OnEnable()
    {
    }

    public void RegisterInteractable()
    {
        if (GameManager.Instance == null) return;
        GameManager.Instance.RegisterInteractable(gameObject);
    }

    public void UnregisterInteractable()
    {
        if (GameManager.Instance == null) return;
        GameManager.Instance.RegisterInteractable(null);
    }

    public void Interact()
    {
        _onInteract.Invoke();
    }
}
