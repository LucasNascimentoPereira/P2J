using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnManager : MonoBehaviour
{

    [SerializeField] private List<GameObject> enemyPrefabs = new();
    [SerializeField] private List<GameObject> spawnAreas = new();
    public enum enemyTypes
    {
        GOOMBUH,
        CHASER,
        FLYING,
        BOSS
    }


    public void ResetEnemies()
    {

    }

    public void ResetEnemiesAll()
    {

    }
}
