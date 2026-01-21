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
    [SerializeField] private float fadeInTime = 1.0f;
    [Tooltip("Time it takes to fade out")]
    [Range(0f, 10f)]
    [SerializeField] private float fadeOutTime = 1.0f;

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
	Debug.Log("PlayMusic" + music);
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
        float volumeInter = fadeOutTime / (_myAudioSource.volume * 100);
        while (enlapedTime < fadeOutTime)
        {
            _myAudioSource.volume -= volumeInter;
	    enlapedTime += Time.deltaTime; 
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
        float volumeInter = fadeInTime / (_myAudioSource.volume * 100);
        while (enlapedTime < fadeInTime)
        {
            _myAudioSource.volume += volumeInter;
	    enlapedTime += Time.deltaTime;
            yield return null;
        }
        _coroutine = null;
    }
}
