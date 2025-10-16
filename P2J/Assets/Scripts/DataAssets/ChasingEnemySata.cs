using UnityEngine;

[CreateAssetMenu(fileName = "ChasingEnemySata", menuName = "Managers/ChasingEnemySata")]

public class ChasingEnemySata : ScriptableObject
{
    [SerializeField] private float chaseSpeed = 1.0f;

    public float ChaseSpeed => chaseSpeed;
} 
