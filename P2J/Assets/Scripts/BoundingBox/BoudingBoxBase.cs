using System;
using UnityEngine;
using UnityEngine.Events;

public abstract class BoudingBoxBase : MonoBehaviour
{
    [SerializeField]
    protected Collider2D boudingCollider2D;
    [SerializeField]
    protected UnityEvent OnEnter;
    [SerializeField]
    protected UnityEvent OnExit;
    [SerializeField]
    protected String objectTag;

    public Collider2D BoudingCollider2D { get => boudingCollider2D; set => boudingCollider2D = value; }

    protected virtual void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag(objectTag)) return;
        OnExit.Invoke();
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(objectTag)) return;
        OnEnter.Invoke();
    }

    public virtual void ToggleEnabled()
    {
        ChangeEnabled(!boudingCollider2D.enabled);
    }

    public virtual void ChangeEnabled(bool value)
    {
        boudingCollider2D.enabled = value;
    }
}
