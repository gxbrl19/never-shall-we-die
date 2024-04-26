using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioHUD : MonoBehaviour
{
    public static AudioHUD instance;

    public AudioSource _audioSource;

    [Header("Audio")]
    public AudioClip _btnClick;
    public AudioClip _pauseBtn;

    [Header("Volume")]
    public float _clickVolume;
    public float _pauseVolume;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(instance);
        }
        else
        {
            Destroy(gameObject);
        }

        _audioSource = GetComponent<AudioSource>();
    }

    public void SoundClick(string clickType)
    {
        if (clickType == "Menu")
        {
            PlaySound(_btnClick, _clickVolume);
        }

        if (clickType == "Pause")
        {
            PlaySound(_pauseBtn, _pauseVolume);
        }
    }

    public void PlaySound(AudioClip sound, float volume)
    {
        _audioSource.volume = volume;
        _audioSource.PlayOneShot(sound);
    }
}
