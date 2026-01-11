using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnManager : MonoBehaviour
{

    [SerializeField] private List<GameObject> enemyPrefabs = new();
    [SerializeField] private List<SpawnArea> spawnAreas = new();

    public List<GameObject> EnemyPrefabs => enemyPrefabs;

    public enum enemyTypes
    {
        GOOMBUH,
        CHASER,
        FLYING,
        BOSS
    }

    private void Start()
    {
        ResetEnemiesAll();
    }

    public void ResetEnemiesAll()
    {
        foreach (var spawnArea in spawnAreas) 
        {
            spawnArea.ResetEnemies();
        }
    }
}
