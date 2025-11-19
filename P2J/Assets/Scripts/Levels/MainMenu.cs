using System;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Camera _camera;
   private void Start()
   {
        UIManager.Instance.CameraReference(_camera);
      Debug.Log("uimanagerchange menu");
        UIManager.Instance.PreviousMenu = null;
      UIManager.Instance.ShowPanel("MainMenu");
   }
}
