using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
      public static AudioManager Instance { get; private set; } = null;
    
    [SerializeField] private AudioMixer defaultAudioMixer = null;
    private AudioSource _myAudioSource = null;

    [SerializeField] private AudioClip click;

    [SerializeField] private StringAudioDictionary _audioDictionary;

    public AudioSource MyAudioSource => _myAudioSource;
    public AudioMixer DefaultAudioMixer => defaultAudioMixer;

    private void Awake()
    {
        if(Instance is null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        _myAudioSource = GetComponent<AudioSource>();
    }

    public void ChangeMasterVolume(float volumeValue)
    {
        defaultAudioMixer.SetFloat("MasterVolume", volumeValue);
    }

    public void ChangeSFXVolume(float volumeValue)
    {
        defaultAudioMixer.SetFloat("SfxVolume", volumeValue);
    }

    public void ChangeMusicVolume(float volumeValue)
    {
        defaultAudioMixer.SetFloat("MusicVolume", volumeValue);
    }

    public void PlaySoundClick()
    {
        _myAudioSource.PlayOneShot(click);
    }
}
