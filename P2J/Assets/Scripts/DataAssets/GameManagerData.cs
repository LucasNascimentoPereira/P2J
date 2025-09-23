using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "GameManagerData", menuName = "Managers/GameManagerData")]

public class GameManagerData : ScriptableObject
{
    [Header("Purchase Values")]
    [SerializeField] private int turretMenuPrice;
}
