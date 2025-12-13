using UnityEngine;
using System.Collections.Generic;

public class Background : MonoBehaviour
{
    [SerializeField] private BackgroundDataAsset backgroundDataAsset;
    [SerializeField] private List<GameObject> layers;
    private List<Vector3> iniPos = new();
    private PlayerController playerController;
    private Vector3 playyerTransform;

    private void Start()
    {
        GameManager.Instance.Background = this;
        playerController = GameManager.Instance.PlayerController;

        for (int i = 0; i < layers.Count; i++)
        {
            iniPos.Add(layers[i].transform.position);
        }
    }

    private void FixedUpdate()
    {
        //transform.position = playerTransform.position;
        if (playerController.transform.position == playyerTransform) return;
        for (int i = 0; i < layers.Count; ++i) 
        {
            layers[i].transform.position = layers[i].transform.position + (new Vector3(backgroundDataAsset.LayerSpeeds[i], 0, 0) * -playerController.Rb.linearVelocity.normalized.x);
            //layers[i].transform.position = Vector2.MoveTowards(layers[i].transform.position, layers[i].transform.position + (new Vector2(0, 0) * playerController.MoveValue.normalized), backgroundDataAsset.TimeInterval);
        }
        playyerTransform = playerController.transform.position;
    }

    public void ResetPos()
    {
        for (int i = 0;i < layers.Count; ++i)
        {
            layers[i].transform.position = iniPos[i];
        }
    }

}
