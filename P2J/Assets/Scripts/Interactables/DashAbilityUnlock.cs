using UnityEngine;
using UnityEngine.Events;

public class DashAbilityUnlock : AbilityUnlockBase
{

    protected override void OnEnable()
    {
        base.OnEnable();
        _onInteract.AddListener(controller.DashAbilityUnlock);
    }
}