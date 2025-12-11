using UnityEngine;
using System.Collections.Generic;

public class Background : MonoBehaviour
{
    [SerializeField] private BackgroundDataAsset backgroundDataAsset;
    [SerializeField] private List<GameObject> layers;
    private PlayerController playerController;

    private void Start()
    {
        playerController = GameManager.Instance.PlayerController;
    }

    private void FixedUpdate()
    {
        //transform.position = playerTransform.position;
        for (int i = 0; i < layers.Count; ++i) 
        {
            layers[i].transform.position = layers[i].transform.position + (new Vector3(backgroundDataAsset.LayerSpeeds[i], 0, 0) * -playerController.MoveValue.normalized.x);
            //layers[i].transform.position = Vector2.MoveTowards(layers[i].transform.position, layers[i].transform.position + (new Vector2(0, 0) * playerController.MoveValue.normalized), backgroundDataAsset.TimeInterval);

        }
    }

}
