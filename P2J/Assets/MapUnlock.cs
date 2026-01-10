using UnityEngine;
using UnityEngine.Events;

public class MapUnlock : MonoBehaviour
{
    private UnityEvent onUnlock;
    [SerializeField] private int index;

    private void Start()
    {
        onUnlock.AddListener(() => UIManager.Instance.UnlockMap(index));
    }

    public void UnlockMap()
    {
        onUnlock.Invoke();
    }
}
