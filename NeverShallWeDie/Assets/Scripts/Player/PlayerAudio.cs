using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudio : MonoBehaviour
{
    [Header("AudioClips")]
    public AudioClip _swordSwish;
    public AudioClip _jumpSound;
    public AudioClip _pistolSound;
    public AudioClip _damageSound;

    [Header("Volumes")]
    public float _swordVolume;
    public float _jumpVolume;
    public float _pistolVolume;
    public float _damageVolume;

    AudioSource _audioSource;
    Player _player;

    void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        _player = GetComponent<Player>();
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

    public void VolumeControl(AudioClip _sound)
    {
        if (_sound == _swordSwish)
        {
            _audioSource.volume = _swordVolume;
        }

        if (_sound == _jumpSound)
        {
            _audioSource.volume = _jumpVolume;
        }

        if (_sound == _pistolSound)
        {
            _audioSource.volume = _pistolVolume;
        }

        if (_sound == _damageSound)
        {
            _audioSource.volume = _damageVolume;
        }
    }
}
