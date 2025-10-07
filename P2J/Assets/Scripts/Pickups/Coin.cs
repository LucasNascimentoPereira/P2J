using UnityEngine;

public class Coin : Pickup
{
    public override void PickUp()
    {
        GameManager.Instance.AddCoins();
        Destroy(gameObject);
    }
}
