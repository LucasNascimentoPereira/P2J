using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
   public void ChangeSpawnPoint()
    {
        GameManager.Instance.CurrentSpawnPoint = gameObject;
    }
}
