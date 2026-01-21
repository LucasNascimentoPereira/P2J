using UnityEngine;
using System.Collections.Generic;


[CreateAssetMenu(fileName = "BackgroundDataAsset", menuName = "Managers/BackgroundDataAsset")]

public class BackgroundDataAsset : ScriptableObject
{
    [SerializeField] private List<float> layerSpeeds;
    [SerializeField] private Vector3 moveDelta;


    public List<float> LayerSpeeds => layerSpeeds;
    public Vector3 MoveDelta => moveDelta;
}
