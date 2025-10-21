using UnityEngine;
using UnityEngine.Events;

public class AbilityUnlockBase : MonoBehaviour, IInteractable
{

    protected PlayerController controller;
    protected UnityEvent _onInteract = new();

    protected virtual void OnEnable()
    {
        controller = GameManager.Instance.PlayerController;
    }

    public void Interact()
    {
        _onInteract.Invoke();
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

}
