using UnityEngine;
using System.Collections.Generic;

public class ParticleBase : MonoBehaviour
{
    [SerializeField] protected List<ParticleSystem> _particleSystems;

    public void PlayParticle(int index)
    {
        _particleSystems[index].Play();
    }
}
