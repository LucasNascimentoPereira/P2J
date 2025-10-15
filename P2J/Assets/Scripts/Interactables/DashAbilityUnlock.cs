using UnityEngine;
using UnityEngine.Events;

public class DashAbilityUnlock : MonoBehaviour, IInteractable
{

    private PlayerController controller;
    private UnityEvent _onInteract;

    private void Start()
    {
        _onInteract.AddListener(GameManager.Instance.PlayerController.DashAbilityUnlock);
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
