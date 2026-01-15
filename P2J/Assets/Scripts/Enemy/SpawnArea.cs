using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnArea : MonoBehaviour
{
    [SerializeField] EnemySpawnManager spawnManager;
    [SerializeField] private List<GameObject> spawn = new();
    [SerializeField] private LockableRoom lockableRoom;
    [SerializeField] float resetTime = 20.0f;
    private List<GameObject> enemies = new();
    private Coroutine coroutine;

    public void ResetEnemies()
    {
	if(enemies.Count != 0)
	{
        	foreach (var enemy in enemies)
        	{
            		Destroy(enemy);
        	}
	}
        SpawnEnemies();
    }

    public void SpawnEnemies()
    {
        foreach (var item in spawn) 
        {
            GameObject enemy = Instantiate(spawnManager.EnemyPrefabs[(int)Enum.Parse<EnemySpawnManager.enemyTypes>(item.tag)], item.transform.position, item.transform.rotation);
	    Debug.Log("eeeeeeee");
	    enemy.GetComponentInChildren<EnemyHealth>().MySpawnArea = this;
            enemies.Add(enemy);
        }
    }

    public void SpawnEnter()
    {
        if (coroutine != null) StopCoroutine(coroutine);
	if (lockableRoom != null) EnteredLockableRoom();
    }

    public void SpawnExit()
    {
        coroutine = StartCoroutine(SpawnCoroutine());
    }

    private IEnumerator SpawnCoroutine()
    {
        yield return new WaitForSeconds(resetTime);
        ResetEnemies();
        coroutine = null;
    }
    private void EnteredLockableRoom()
    {
	if (lockableRoom == null) return;
	lockableRoom.LockRoom();
	int loopCount = (int)(lockableRoom.EnemyAmnt / spawn.Count);
	for (int i = 0; i < loopCount; ++i)
	{
		SpawnEnemies();
	}
    }
    public void RemoveEnemy(GameObject enemyToRemove)
    {
	if(!enemies.Contains(enemyToRemove)) return;
	enemies.Remove(enemyToRemove);
	if(enemies.Count == 0 && lockableRoom != null)
	{
		lockableRoom.UnlockRoom();
	}
    }
}
