using UnityEngine;

public class Coin : Pickup
{

    protected override void Awake()
    {
        _onPickup.AddListener(GameManager.Instance.AddCoins);
    }
}
