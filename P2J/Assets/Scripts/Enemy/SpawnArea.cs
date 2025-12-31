using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnArea : MonoBehaviour
{
    [SerializeField] EnemySpawnManager spawnManager;
    [SerializeField] private List<GameObject> spawn = new();
    [SerializeField] float resetTime = 20.0f;
    private List<GameObject> enemies = new();
    private Coroutine coroutine;

    public void ResetEnemies()
    {
        foreach (var enemy in enemies)
        {
            Destroy(enemy);
        }
        SpawnEnemies();
    }

    public void SpawnEnemies()
    {
        foreach (var item in spawn) 
        {
            GameObject enemy = Instantiate(spawnManager.EnemyPrefabs[(int)Enum.Parse<EnemySpawnManager.enemyTypes>(gameObject.tag)], item.transform.position, item.transform.rotation);
            enemies.Add(enemy);
        }
    }

    public void SpawnEnter()
    {
        if (coroutine != null) StopCoroutine(coroutine);
    }

    public void SpawnExit()
    {
        coroutine = StartCoroutine(SpawnCoroutine());
    }

    private IEnumerator SpawnCoroutine()
    {
        yield return null;
        coroutine = null;
    }
}
