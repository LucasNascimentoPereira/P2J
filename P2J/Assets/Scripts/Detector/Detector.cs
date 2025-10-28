using UnityEngine;
using UnityEngine.Events;

public class Detector : MonoBehaviour
{
    [SerializeField] private UnityEvent OnEntered;
    [SerializeField] private UnityEvent OnExit;
    [SerializeField] private UnityEvent OnStay;
    [SerializeField] private string detectorTag;

    private Collider2D _collider;

    public Collider2D Collider => _collider;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag(detectorTag)) return;
        _collider = other;
        OnEntered.Invoke();
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag(detectorTag)) return;
        //_collider = null;
        OnExit.Invoke();
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (!other.CompareTag(detectorTag)) return;
        _collider = other;
        OnStay.Invoke();
    }
}
