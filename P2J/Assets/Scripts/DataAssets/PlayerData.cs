using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "Managers/PlayerData")]

public class PlayerData : ScriptableObject
{
   [SerializeField] private string playerName = "Player";
   [SerializeField] private float playerGroundSpeed = 10.0f;
   [SerializeField] private float playerJumpForce = 10.0f;
   [SerializeField] private float playerAirSpeed = 10.0f;
   [SerializeField] private float playerInvencibilityTime = 0.5f;
   [SerializeField] private int playerInvencibilityBlinks = 3;
   
   public string PlayerName => playerName;
   public float PlayerGroundSpeed => playerGroundSpeed;
   public float PlayerJumpForce => playerJumpForce;
   public float PlayerAirSpeed => playerAirSpeed;
   public float PlayerInvencibilityTime => playerInvencibilityTime;
   public int PlayerInvencibilityBlinks => playerInvencibilityBlinks;
}
