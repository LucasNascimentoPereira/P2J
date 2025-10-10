using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
   public void ChangeSpawnPoint()
    {
        Debug.Log("SpawnPoint");
        GameManager.Instance.CurrentSpawnPoint = gameObject;
    }
}
