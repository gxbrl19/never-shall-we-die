using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudio : MonoBehaviour
{
    //sons nas animações que se repetem não funcionam, precisam ser chamados via script
    public AudioClip _jump;
    public AudioClip _damage;
    public AudioClip _splash;
    public AudioClip _parachute;
    public AudioClip _slide;

    AudioSource _audioSource;

    void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    public void PlayAudio(AudioClip sound)
    {
        if (Time.timeScale != 0f)
        {
            _audioSource.pitch = 1f;
            //VolumeControl(sound);
            _audioSource.PlayOneShot(sound);
        }
    }

    void VolumeControl(AudioClip sound)
    {
        switch (sound.name)
        {
            case "sword_swish": //animação
                _audioSource.volume = 1.3f;
                break;
            case "jump": //script
                _audioSource.volume = 1.8f;
                break;
            case "damage_player": //script
                _audioSource.volume = 1.4f;
                break;
            case "swin": //animação
                _audioSource.volume = 1.6f;
                break;
            case "aircut": //animação
                _audioSource.volume = 1.6f;
                break;
            case "water_spin": //animação
                _audioSource.volume = 1.5f;
                break;
            case "splash": //script
                _audioSource.volume = 1.3f;
                break;
            case "parachute": //script
                _audioSource.volume = .9f;
                break;
        }
    }
}
