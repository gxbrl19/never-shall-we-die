using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMusic : MonoBehaviour
{
    public static BackgroundMusic instance;

    [Header("Theme")]
    public AudioClip _kingdomTheme;
    public AudioClip _shipTheme;
    public AudioClip _forestTheme;
    public AudioClip _mizutonTheme;
    public AudioClip _cemeteryTheme;
    public AudioClip _prisonTheme;
    public AudioClip _mansionTheme;
    public AudioClip _bossTheme;

    [Header("Intros")]
    public AudioClip _kingdomIntro;
    public AudioClip _shipIntro;
    public AudioClip _forestIntro;
    public AudioClip _mizutonIntro;
    public AudioClip _cemeteryIntro;
    public AudioClip _prisonIntro;
    public AudioClip _mansionIntro;
    public AudioClip _bossIntro;

    [Header("Components")]
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

    public void ChangeMusic(AudioClip audioClip, AudioClip introClip)
    {
        _audioSource.clip = audioClip;
        _audioSource.Play();
        _audioSource.Stop();

        _audioSourceIntro.enabled = true;
        _audioSourceIntro.clip = introClip;

        // Pegar o tempo DSP atual
        double startTime = AudioSettings.dspTime;

        // Agendar a intro para começar imediatamente
        _audioSourceIntro.PlayScheduled(startTime);

        // Garantir um cálculo preciso do tempo de transição
        double introDuration = introClip.length; // Usar a duração exata do áudio
        double secondClipStartTime = startTime + introDuration;

        // Configurar e agendar a música principal
        _audioSource.clip = audioClip;
        _audioSource.PlayScheduled(secondClipStartTime);

        Debug.Log($"Music: {introClip.name}, Intro Duration: {introDuration}, Scheduled Start: {secondClipStartTime}");
    }

    public void BossMusic()
    {
        _audioSource.Stop();
        _audioSourceIntro.enabled = true;
        _audioSourceIntro.clip = _bossIntro;
        double startTime = AudioSettings.dspTime;
        _audioSourceIntro.PlayScheduled(startTime);
        double secondClipStartTime = startTime + _audioSourceIntro.clip.length;
        _audioSource.clip = _bossTheme;
        _audioSource.PlayScheduled(secondClipStartTime);
    }

    public void FinishBoss()
    {
        //_audioSource.Stop();
        _audioSourceIntro.enabled = false;
        _audioSource.clip = _forestTheme; //TODO: pegar a musica da ilha atual
        _audioSource.Play();
    }
}
