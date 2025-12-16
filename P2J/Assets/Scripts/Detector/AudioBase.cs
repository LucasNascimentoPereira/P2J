using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(AudioSource))]

public class AudioBase : MonoBehaviour
{
    [SerializeField] protected StringAudioDictionary audioDictionary = new();
    [SerializeField] protected List<AudioClip> audioClips;
    protected AudioSource audioSource;
    private float sometime = 0.0f;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.playOnAwake = false;
        //audioSource.outputAudioMixerGroup = AudioManager.Instance.DefaultAudioMixer.FindMatchingGroups("Sfx")[0];
    }

    public void PlaySound()
    {
        //audioSource.PlayOneShot(audioClips[index]);
        //audioSource.PlayOneShot(audioDictionary.GetValueOrDefault("eee"));
    }

    public void StopSound()
    {

    }

    public void FadeSound()
    {

    }

    private void Update()
    {
        sometime += Time.time;
        if (sometime > 1000.0f) {
            PlaySound();
            sometime = 0.0f;
        }
    }
}
