using System.Collections;
using UnityEngine;

public class DoorInteractable : InteractableBase
{
    [SerializeField] private Animator doorAnimator;
    private int doorAnimation = Animator.StringToHash("doorAnimation");
    protected override void OnEnable()
    {
        base.OnEnable();
        _onInteract.AddListener(UnlockDoor);
    }

    private void UnlockDoor()
    {
        doorAnimator.SetTrigger(doorAnimation);
    }
}
