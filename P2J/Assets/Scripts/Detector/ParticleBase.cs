using UnityEngine;
using System.Collections.Generic;

public class ParticleBase : MonoBehaviour
{
    [SerializeField] protected List<ParticleSystem> _particleSystems;

    public void PlayParticle(int index)
    {
        if (index >= _particleSystems.Count || index < 0) return;
        _particleSystems[index].Play();
    }

    public void PlayParticleRange(string range)
    {
        int rangeStart = range[0];
        int rangeEnd = range[1];

        if (rangeStart >= _particleSystems.Count || rangeStart < 0) return;
        if (rangeEnd >= _particleSystems.Count || rangeEnd < rangeStart) return;
        _particleSystems[Random.Range(rangeStart, rangeEnd)].Play();
    }
}
