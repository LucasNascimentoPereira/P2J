using UnityEngine;
using UnityEngine.Events;

public class InteractableBase : MonoBehaviour, IInteractable
{

    protected PlayerController controller;
    [SerializeField] protected UnityEvent _onInteract = new();

    protected virtual void OnEnable()
    {
        controller = GameManager.Instance.PlayerController;
    }

    public void RegisterInteractable()
    {
        if (controller == null) return;
        controller.RegisterInteractable(this);
    }

    public void UnregisterInteractable()
    {
        if (controller == null) return;
        controller.RegisterInteractable(null);
    }

    public void Interact()
    {
        _onInteract.Invoke();
    }
}
