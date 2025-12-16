using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Accessibility;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
      public static AudioManager Instance { get; private set; } = null;
    
    [SerializeField] private AudioMixer defaultAudioMixer = null;
    private AudioSource _myAudioSource = null;


    [SerializeField] private List<AudioClip> musics;

    public AudioSource MyAudioSource => _myAudioSource;
    public AudioMixer DefaultAudioMixer => defaultAudioMixer;

    private Coroutine _coroutine;

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

    public void PlayMusic(int music)
    {
        if(_coroutine != null)
        {

        }

        MyAudioSource.Stop();
        _myAudioSource.resource = musics[music];
        _myAudioSource.Play();
    }

    private IEnumerator FadeOut()
    {
        yield return null;
    }

    private IEnumerator FadeIn() 
    {
        yield return null;
    }
}
