using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudio : MonoBehaviour
{
    //sons nas animações que se repetem não funcionam, precisam ser chamados via script
    public AudioClip _jump;
    public AudioClip _damage;

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
            VolumeControl(sound);
            _audioSource.PlayOneShot(sound);
        }
    }

    void VolumeControl(AudioClip sound)
    {
        switch (sound.name)
        {
            case "sword_swish": //animação
                _audioSource.volume = .4f;
                break;
            case "jump": //script
                _audioSource.volume = .6f;
                break;
            case "damage_player": //script
                _audioSource.volume = .6f;
                break;
            case "swin": //animação
                _audioSource.volume = .8f;
                break;
            case "aircut": //animação
                _audioSource.volume = .8f;
                break;
            case "water_spin": //animação
                _audioSource.volume = .8f;
                break;
        }
    }
}
