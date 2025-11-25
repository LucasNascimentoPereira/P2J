using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

public class InteractableBase : MonoBehaviour, IInteractable
{

    [SerializeField] protected UnityEvent _onInteract = new();
    [SerializeField] protected AudioSource audioSource;
    [SerializeField] protected List<AudioClip> audioClips;

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

    protected void PlaySound()
    {

    }
}
