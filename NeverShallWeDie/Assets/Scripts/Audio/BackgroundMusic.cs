using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMusic : MonoBehaviour
{
    public static BackgroundMusic instance;

    public AudioClip _nswdTheme;
    public AudioClip _kingdomTheme;
    public AudioClip _shipTheme;
    public AudioClip _forestTheme;
    public AudioClip _minesTheme;
    public AudioClip _mizutonTheme;
    public AudioClip _cemeteryTheme;
    public AudioClip _prisonTheme;
    public AudioClip _mansionTheme;
    public AudioClip _bossTheme;

    AudioSource _audioSource;
    [HideInInspector] public AudioClip _deadSound;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        _audioSource = gameObject.GetComponent<AudioSource>();
    }

    public void ChangeMusic(AudioClip audioClip)
    {
        _audioSource.Stop();
        _audioSource.clip = audioClip;
        _audioSource.Play();
    }
}
