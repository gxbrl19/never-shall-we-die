using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudio : MonoBehaviour
{
    [Header("Katana")] public AudioClip[] _katanas;
    [Header("Hit")] public AudioClip[] _hits;
    [Header("Jump")] public AudioClip _jump;
    [Header("Roll")] public AudioClip _roll;
    [Header("Water")] public AudioClip _splash;
    [Header("Water")] public AudioClip _swim;
    public AudioClip _parachute;
    public AudioClip _slide;

    [Header("Skills")] public AudioClip _aircut;
    [Header("Skills")] public AudioClip _waterspin;

    AudioSource _audioSource;

    void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    public void PlayAudio(string audio)
    {
        if (Time.timeScale == 0f) { return; }

        switch (audio)
        {
            case "katana": //animação
                int katanas = Random.Range(0, 3);
                _audioSource.PlayOneShot(_katanas[katanas]);
                break;
            case "jump": //script
                _audioSource.PlayOneShot(_jump);
                break;
            case "roll": //animação
                _audioSource.PlayOneShot(_roll);
                break;
            case "hit": //script
                int hits = Random.Range(0, 2);
                _audioSource.PlayOneShot(_hits[hits]);
                break;
            case "swin": //animação
                _audioSource.PlayOneShot(_swim);
                break;
            case "aircut": //animação
                _audioSource.PlayOneShot(_aircut);
                break;
            case "waterspin": //animação
                _audioSource.PlayOneShot(_waterspin);
                break;
            case "splash": //script
                _audioSource.PlayOneShot(_splash);
                break;
            case "parachute": //script
                _audioSource.PlayOneShot(_parachute);
                break;
        }
    }
}
