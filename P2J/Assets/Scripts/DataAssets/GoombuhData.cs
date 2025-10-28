using UnityEngine;

[CreateAssetMenu(fileName = "GoombuhData", menuName = "Managers/GoombuhData")]

public class GoombuhData : ScriptableObject
{
    [SerializeField] private float goombuhSpeed = 0.5f;
    [SerializeField] private float goombuhDamage = 0.5f;
    [SerializeField] private float goombuhKnockback = 5f;
    [SerializeField] private bool isGoombuhKnockback = true;

    public float GoombuhSpeed => goombuhSpeed;
    public float GoombuhDamage => goombuhDamage;
    public float GoombuhKnockback => goombuhKnockback;
    public bool IsGoombuhKnockback => isGoombuhKnockback;
}
