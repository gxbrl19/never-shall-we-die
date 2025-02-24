using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class AudioHUD : MonoBehaviour
{
    public static AudioHUD instance;

    [SerializeField] EventReference _confirmBtn;
    [SerializeField] EventReference _backBtn;
    [SerializeField] EventReference _navigationBtn;
    [SerializeField] EventReference _selectBtn;

    public AudioSource _audioSource;

    [Header("Audio")]
    public AudioClip _btnClick;
    //public AudioClip _navigationBtn;
    public AudioClip _pauseBtn;

    [Header("Volume")]
    public float _clickVolume;
    public float _navigationVolume;
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

    public void PlayConfirmButton()
    {
        RuntimeManager.PlayOneShot(_confirmBtn);
    }

    public void PlayBackButton()
    {
        RuntimeManager.PlayOneShot(_backBtn);
    }

    public void PlayNavigationButton()
    {
        RuntimeManager.PlayOneShot(_navigationBtn);
    }

    public void PlaySelectButton()
    {
        RuntimeManager.PlayOneShot(_selectBtn);
    }

    public void PlaySound(AudioClip sound, float volume)
    {
        _audioSource.volume = volume;
        _audioSource.PlayOneShot(sound);
    }
}
