using UnityEngine;
using UnityEngine.Events;

public class DashAbilityUnlock : InteractableBase
{
    protected override void OnEnable()
    {
        base.OnEnable();
        //_onInteract.AddListener(GameManager.Instance.UnlockAbility);
    }

    protected override void Start()
    {
        base.Start();
        Debug.Log("regi");
        _onInteract.AddListener(GameManager.Instance.UnlockAbility);
    }
}