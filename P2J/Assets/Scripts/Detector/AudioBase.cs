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
        if (index >= audioClips.Count) return;
        audioSource.PlayOneShot(audioClips[index]);
    }

    public void FadeSound()
    {

    }
}
