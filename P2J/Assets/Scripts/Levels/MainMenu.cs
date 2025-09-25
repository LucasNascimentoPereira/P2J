using System;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
   private void Start()
   {
      UIManager.Instance.ShowPanel("MainMenu");
   }
}
