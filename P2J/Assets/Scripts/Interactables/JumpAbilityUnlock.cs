using UnityEngine;

public class JumpAbilityUnlock : AbilityUnlockBase
{
    protected override void OnEnable()
    {
        base.OnEnable();
        _onInteract.AddListener(controller.JumpAbilityUnlock);
    }
}
