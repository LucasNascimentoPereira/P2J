using System.Collections;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

public class HealthDoor : HealthBase
{
    public void LevelReset()
    {
        currentHealth = maxHealth;
    }
}
