using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMusic : MonoBehaviour
{
    public static BackgroundMusic instance;

    public AudioClip _kingdomTheme;
    public AudioClip _shipTheme;
    public AudioClip _forestTheme;
    public AudioClip _mizutonTheme;
    public AudioClip _cemeteryTheme;
    public AudioClip _prisonTheme;
    public AudioClip _mansionTheme;
    public AudioClip _bossTheme;

    public AudioSource _audioSourceIntro;
    [HideInInspector] public AudioSource _audioSource;
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
        BackgroundMusic.instance._audioSource.enabled = true;
        //_audioSource.Stop();
        _audioSource.clip = audioClip;
        _audioSource.Play();
    }

    public void BossMusic()
    {
        _audioSource.Stop();
        _audioSourceIntro.enabled = true;
        double startTime = AudioSettings.dspTime;
        _audioSourceIntro.PlayScheduled(startTime);
        double secondClipStartTime = startTime + _audioSourceIntro.clip.length;
        _audioSource.clip = _bossTheme;
        _audioSource.PlayScheduled(secondClipStartTime);


        //_audioSource.Play();
    }

    public void FinishBoss()
    {
        //_audioSource.Stop();
        _audioSourceIntro.enabled = false;
        _audioSource.clip = _forestTheme; //TODO: pegar a musica da ilha atual
        _audioSource.Play();
    }
}
