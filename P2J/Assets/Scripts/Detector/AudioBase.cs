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
        audioSource.outputAudioMixerGroup = AudioManager.Instance.DefaultAudioMixer.FindMatchingGroups("Sfx")[0];
    }

    public void PlaySound(int index)
    {
        if (index >= audioClips.Count || index < 0) return;
        audioSource.PlayOneShot(audioClips[index]);
	Debug.Log("playing audio mothafucka");
    }

    public void PlaySoundRange(string range)
    {
        int rangeStart = range[0];
        int rangeEnd = range[1];
        if (rangeStart >= audioClips.Count || rangeStart < 0) return;
        if (rangeEnd >= audioClips.Count || rangeEnd < rangeStart) return;
        audioSource.PlayOneShot(audioClips[Random.Range(rangeStart, rangeEnd)]);
    }
}
