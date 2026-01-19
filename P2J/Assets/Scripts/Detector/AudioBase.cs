using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(AudioSource))]

public class AudioBase : MonoBehaviour
{
    [SerializeField] protected List<AudioClip> audioClips;
    [SerializeField] protected AudioSource audioSource;

    private void Awake()
    {
        audioSource.playOnAwake = false;
    }

    public void PlaySound(int index)
    {
        if (index >= audioClips.Count || index < 0) return;
        audioSource.PlayOneShot(audioClips[index]);
    }

    public void PlaySoundRange(string range)
    {
        string[] rangeParts = range.Split('-');

        int rangeStart = int.Parse(rangeParts[0]);
        int rangeEnd = int.Parse(rangeParts[1]);

        if (rangeStart >= audioClips.Count || rangeStart < 0) return;
        if (rangeEnd >= audioClips.Count || rangeEnd < rangeStart) return;

        audioSource.PlayOneShot(audioClips[Random.Range(rangeStart, rangeEnd)]);
    }
}
