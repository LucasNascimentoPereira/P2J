using UnityEngine;
using System.Collections.Generic;

public class LockableRoom : MonoBehaviour
{
	private List<GameObject> locks = new();
	[SerializeField] private int enemyAmnt = 3;
	public int EnemyAmnt => enemyAmnt;
	private bool hasActivated;

	public void LockRoom()
	{
		if (hasActivated) return;
		hasActivated = true;
		foreach (GameObject roomLock in locks)
		{
			roomLock.SetActive(true);
		}
	}
	public void UnlockRoom()
	{
		foreach (GameObject roomLock in locks)
		{
			roomLock.SetActive(false);
		}
	}
	private void Start()
	{
		foreach(Transform child in transform)
		{
			locks.Add(child.gameObject);
		}
	}
}
