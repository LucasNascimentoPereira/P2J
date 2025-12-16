using UnityEngine;
using Unity.Cinemachine;

public class PlayerCamera : MonoBehaviour
{
    private Rigidbody2D _playerRb;
    private CinemachinePositionComposer _posComp;
    private GameObject player;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        _playerRb = player.GetComponent<Rigidbody2D>();
        _posComp = GetComponent<CinemachinePositionComposer>();
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 playerViewportPos = Camera.main.WorldToViewportPoint(player.transform.position);

        // I know it's hardcoded
        // But I don't care man...
        if (_playerRb.linearVelocityY < 0 && playerViewportPos.y < 0.27f)
        {
            _posComp.Damping.y = 0;
        } else
        {
            _posComp.Damping.y = 2;
        }
    }
}
