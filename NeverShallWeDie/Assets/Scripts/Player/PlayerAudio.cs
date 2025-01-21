using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudio : MonoBehaviour
{
    [Header("Movement")]
    public AudioClip[] _katanas;
    public AudioClip[] _hits;
    public AudioClip _jump;
    public AudioClip _healing;
    public AudioClip _roll;

    [Header("Water")]
    public AudioClip _splash;
    public AudioClip _swim;

    [Header("Equipment")]
    public AudioClip _parachute;
    public AudioClip _slide;

    [Header("Skills")]
    public AudioClip _aircut;
    public AudioClip _waterspin;

    bool _playLoop;

    [HideInInspector] public AudioSource _audioSource;
    Player _player;

    void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        _player = GetComponent<Player>();
    }

    private void Update()
    {
        Healing();
    }

    public void PlayAudio(string audio)
    {
        if (Time.timeScale == 0f) { return; }

        switch (audio)
        {
            case "katana": //animação
                _audioSource.volume = 1f;
                int katanas = Random.Range(0, 3);
                _audioSource.PlayOneShot(_katanas[katanas]);
                break;
            case "jump": //script
                _audioSource.volume = 1f;
                _audioSource.PlayOneShot(_jump);
                break;
            case "roll": //animação
                _audioSource.volume = 1f;
                _audioSource.PlayOneShot(_roll);
                break;
            case "hit": //script
                _audioSource.volume = 1f;
                int hits = Random.Range(0, 2);
                _audioSource.PlayOneShot(_hits[hits]);
                break;
            case "swin": //animação
                _audioSource.volume = 1f;
                _audioSource.PlayOneShot(_swim);
                break;
            case "aircut": //animação
                _audioSource.volume = 1f;
                _audioSource.PlayOneShot(_aircut);
                break;
            case "waterspin": //animação
                _audioSource.volume = 1f;
                _audioSource.PlayOneShot(_waterspin);
                break;
            case "splash": //script
                _audioSource.volume = 1f;
                _audioSource.PlayOneShot(_splash);
                break;
            case "parachute": //script
                _audioSource.volume = 0.6f;
                _audioSource.PlayOneShot(_parachute);
                break;
            case "slide": //script
                _audioSource.volume = 0.6f;
                _audioSource.PlayOneShot(_slide);
                break;
        }
    }

    void Healing()
    {
        if (_player._healing)
        {
            if (!_playLoop)
            {
                _playLoop = true;
                _audioSource.clip = _healing;
                _audioSource.loop = true;
                _audioSource.Play();
            }
        }
        else
        {
            _audioSource.loop = false;
            _playLoop = false;
        }
    }
}
