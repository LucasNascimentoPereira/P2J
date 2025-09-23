using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
      public static AudioManager Instance { get; private set; } = null;
    
    [SerializeField] private AudioMixer defaultAudioMixer = null;
    private AudioSource _myAudioSource = null;

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

    private IEnumerator FadeSound(AudioClip audioClip)
    {
        while (_myAudioSource.volume >= 0.1f)
        {
            yield return new WaitForSeconds(0.1f);
            _myAudioSource.volume -= 0.1f;
        }
        _myAudioSource.Stop();
        _myAudioSource.loop = true;
        _myAudioSource.clip = audioClip;
        _myAudioSource.Play();
        while (_myAudioSource.volume <= 0.99f)
        {
            _myAudioSource.volume += 0.1f;
        }
        StopCoroutine(nameof(FadeSound));
    }
    
    public void PlayAudioClip(AudioClip audioClip, bool loop)
    {
        if (loop)
        {
            StartCoroutine(FadeSound(audioClip));
            /*myAudioSource.Stop();
            myAudioSource.loop = true;
            myAudioSource.clip = audioClip;
            myAudioSource.Play();*/
        }
        else
        {
            PlayAudioClip(audioClip);
        }
    }
    

    public void PlayAudioClip(AudioClip audioClip)
    {
        _myAudioSource.PlayOneShot(audioClip);
    }
    

    /*public void ChangeSong(string song)
    {
        switch(song)
        {
            case "MenuSong": 
                Debug.Log("menusong");
                myAudioSource.Stop();
                myAudioSource.PlayOneShot(_audioManagerData.mainMenuSong);
                myAudioSource.loop = true;
                break;
            case "GameplaySong":
                Debug.Log("gameplaysong");
                myAudioSource.Stop();
                myAudioSource.PlayOneShot(_audioManagerData.mainLevelSong);
                myAudioSource.loop = true;
                break;
            default:
                myAudioSource.PlayOneShot(_audioManagerData.mainMenuSong);
                break;
        }
            
        
    }*/
}
