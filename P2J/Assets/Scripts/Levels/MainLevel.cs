using System;
using UnityEngine;

public class MainLevel : MonoBehaviour
{
    private void Start()
    {
        UIManager.Instance.ShowPanel("ControlsUI");
    }
}
