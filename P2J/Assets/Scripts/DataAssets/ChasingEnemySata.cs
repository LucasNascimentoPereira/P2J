using UnityEngine;

[CreateAssetMenu(fileName = "ChasingEnemySata", menuName = "Managers/ChasingEnemySata")]

public class ChasingEnemySata : ScriptableObject
{
    [SerializeField] private float chaseSpeed = 1.0f;
    [SerializeField] private float lungeSpeed = 3.0f;
    [SerializeField] private float lungeTime = 1.5f;
    [SerializeField] private float restTime = 4.0f;
    [SerializeField] private float damage = 1.0f;
    [SerializeField] private float knockBack = 1000.0f;
    [SerializeField] private float jump = 1000.0f;
    [SerializeField] private GameObject coin;
    [SerializeField] private float coinKnockback;
    [SerializeField] private int coinNumber;

    public float ChaseSpeed => chaseSpeed;
    public float LungeSpeed => lungeSpeed;
    public float LungeTime => lungeTime;
    public float RestTime => restTime;
    public float Damage => damage;
    public float KnockBack => knockBack;
    public float Jump => jump;
    public GameObject Coin => coin;
    public float CoinKnockback => coinKnockback;
    public int CoinNumber => coinNumber;

} 
