using UnityEngine;

public class RestingArea : InteractableBase
{
    protected override void OnEnable()
    {
        base.OnEnable();
    }

    protected override void Start()
    {
        base.Start();
        _onInteract.AddListener(GameManager.Instance.RestingArea);
    }
}
