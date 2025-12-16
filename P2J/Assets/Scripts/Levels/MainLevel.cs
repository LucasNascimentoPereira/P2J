using System;
using UnityEngine;

public class MainLevel : MonoBehaviour
{
    [SerializeField] private Camera _camera;

    private void Start()
    {
        UIManager.Instance.CameraReference(_camera);
        UIManager.Instance.ShowPanel("HUD");
        AudioManager.Instance.PlayMusic(1);
    }
}
