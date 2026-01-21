using UnityEngine;
using UnityEngine.Events;

public class DashAbilityUnlock : InteractableBase
{
    protected override void OnEnable()
    {
        base.OnEnable();
    }

    protected override void Start()
    {
        base.Start();
        Debug.Log("regi");
        _onInteract.AddListener(GameManager.Instance.UnlockAbility);
    }
}