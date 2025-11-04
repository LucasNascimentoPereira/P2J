using UnityEngine;

[CreateAssetMenu(fileName = "FlyingEnemySata", menuName = "Managers/FlyingEnemySata")]


public class FlyingEnemyData : ScriptableObject
{
    [SerializeField] private float idleSpeed = 1.0f;
    [SerializeField] private float lungeSpeed = 3.0f;
    [SerializeField] private float lungeTime = 1.5f;
    [SerializeField] private float restTime = 4.0f;
    [SerializeField] private float damage = 1.0f;
    [SerializeField] private float projectileSpeed = 1.0f;
    [SerializeField] private float knockBack = 1000.0f;

    public float IdleSpeed => idleSpeed;
    public float LungeSpeed => lungeSpeed;
    public float LungeTime => lungeTime;
    public float RestTime => restTime;
    public float Damage => damage;
    public float ProjectileSpeed => projectileSpeed;
    public float KnockBack => knockBack;
}
