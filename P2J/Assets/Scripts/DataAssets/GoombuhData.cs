using UnityEngine;

[CreateAssetMenu(fileName = "GoombuhData", menuName = "Managers/GoombuhData")]

public class GoombuhData : ScriptableObject
{
    [SerializeField] private float goombuhSpeed = 0.5f;

    public float GoombuhSpeed => goombuhSpeed;
}
