using System;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
   private void Start()
   {
      Debug.Log("uimanagerchange menu");
      UIManager.Instance.ShowPanel("MainMenu");
      
   }
}
