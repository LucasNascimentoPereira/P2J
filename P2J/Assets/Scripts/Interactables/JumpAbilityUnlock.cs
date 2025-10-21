using UnityEngine;

public class JumpAbilityUnlock : InteractableBase
{
    protected override void OnEnable()
    {
        base.OnEnable();
        _onInteract.AddListener(controller.JumpAbilityUnlock);
    }
}