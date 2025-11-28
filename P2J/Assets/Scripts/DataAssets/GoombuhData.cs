using UnityEngine;

[CreateAssetMenu(fileName = "GoombuhData", menuName = "Managers/GoombuhData")]

public class GoombuhData : ScriptableObject
{
    [SerializeField] private float goombuhSpeed = 0.5f;
    [SerializeField] private float goombuhDamage = 0.5f;
    [SerializeField] private float goombuhKnockback = 5f;
    [SerializeField] private float knockbackTime = 0.2f;
    [SerializeField] private bool isGoombuhKnockback = true;
    [SerializeField] private GameObject coin;
    [SerializeField] private float coinKnockback;
    [SerializeField] private int coinNumber;

    public float GoombuhSpeed => goombuhSpeed;
    public float GoombuhDamage => goombuhDamage;
    public float GoombuhKnockback => goombuhKnockback;
    public float KnockbackTime => knockbackTime;
    public bool IsGoombuhKnockback => isGoombuhKnockback;
    public GameObject Coin => coin;
    public float CoinKnockback => coinKnockback;
    public int CoinNumber => coinNumber;
}
