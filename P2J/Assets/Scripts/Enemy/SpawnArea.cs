using System.Collections.Generic;
using UnityEngine;

public class SpawnArea : MonoBehaviour
{
    [SerializeField] EnemySpawnManager spawnManager;
    [SerializeField] private List<GameObject> spawn = new();
}
