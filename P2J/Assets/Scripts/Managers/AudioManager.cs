using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Accessibility;
using UnityEngine.Audio;

[RequireComponent(typeof(AudioSource))]

public class AudioManager : MonoBehaviour
{
      public static AudioManager Instance { get; private set; } = null;
    
    [SerializeField] private AudioMixer defaultAudioMixer = null;
    [SerializeField] private AudioSource _myAudioSource = null;

    [Tooltip("Time it taes to fade in")]
    [Range(0f, 10f)]
    [SerializeField] private float fadeInTime;
    [Tooltip("Time it takes to fade out")]
    [Range(0f, 10f)]
    [SerializeField] private float fadeOutTime;

    private Coroutine _coroutine;

    private int musicIndex;

    [SerializeField] private List<AudioClip> musics;

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
    }

    public void ChangeMasterVolume(float volumeValue)
    {
        defaultAudioMixer.SetFloat("MasterVolume", (volumeValue * 80) - 80);
    }

    public void ChangeSFXVolume(float volumeValue)
    {
        defaultAudioMixer.SetFloat("SfxVolume", (volumeValue * 80) - 80);
    }

    public void ChangeMusicVolume(float volumeValue)
    {
        defaultAudioMixer.SetFloat("MusicVolume", (volumeValue * 80) - 80);
    }

    public void PlayMusic(int music)
    {
        musicIndex = music;
        if(_coroutine != null)
        {
            StopCoroutine(_coroutine);
        }
        _coroutine = StartCoroutine(FadeOut());
    }

    private IEnumerator FadeOut()
    {
        float enlapedTime = 0.0f;
        float volumeInter = _myAudioSource.volume / fadeOutTime;
        while (enlapedTime < fadeOutTime)
        {
            _myAudioSource.volume -= volumeInter; 
            yield return null;
        }
        _coroutine = null;
        _myAudioSource.Stop();
        _myAudioSource.resource = musics[musicIndex];
        _myAudioSource.Play();
        _coroutine = StartCoroutine(FadeIn());
    }

    private IEnumerator FadeIn() 
    {
        float enlapedTime = 0.0f;
        float volumeInter = _myAudioSource.volume / fadeInTime;
        while (enlapedTime < fadeInTime)
        {
            _myAudioSource.volume += volumeInter;
            yield return null;
        }
        _coroutine = null;
    }
}
