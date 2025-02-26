using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class PlayerAudio : MonoBehaviour
{
    [SerializeField] EventReference katana;
    [SerializeField] EventReference jump;
    [SerializeField] EventReference roll;
    [SerializeField] EventReference hit;
    [SerializeField] EventReference death;

    [SerializeField] GameObject healing;
    [SerializeField] GameObject grabing;

    [Header("Movement")]
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
    PlayerInputs _input;

    void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        _player = GetComponent<Player>();
        _input = _player.GetComponent<PlayerInputs>();
    }

    private void Update()
    {
        PlayHealing();
        PlayGrab();
    }

    public void PlayAudio(string audio)
    {
        if (Time.timeScale == 0f) { return; }

        switch (audio)
        {
            case "jump": //script
                _audioSource.volume = 1f;
                _audioSource.PlayOneShot(_jump);
                break;
            case "roll": //animação
                _audioSource.volume = 1f;
                _audioSource.PlayOneShot(_roll);
                break;
            case "swin": //animação
                _audioSource.volume = 0.3f;
                _audioSource.PlayOneShot(_swim);
                break;
            case "splash": //script
                _audioSource.volume = 0.3f;
                _audioSource.PlayOneShot(_splash);
                break;
            case "aircut": //animação
                _audioSource.volume = 1f;
                _audioSource.PlayOneShot(_aircut);
                break;
            case "waterspin": //animação
                _audioSource.volume = 1f;
                _audioSource.PlayOneShot(_waterspin);
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

    public void PlayKatana() //animação
    {
        RuntimeManager.PlayOneShot(katana);
    }

    public void PlayJump() //script
    {
        RuntimeManager.PlayOneShot(jump);
    }

    public void PlayRoll() //animação
    {
        RuntimeManager.PlayOneShot(roll);
    }

    public void PlayHit() //script
    {
        RuntimeManager.PlayOneShot(hit);
    }

    public void PlayDeath() //script
    {
        RuntimeManager.PlayOneShot(death);
    }

    public void PlayGrab()
    {
        if (_player._isGrabing && _input.horizontal != 0)
        {
            if (!_playLoop)
            {
                _playLoop = true;
                grabing.SetActive(true);
            }
        }
        else
        {
            grabing.SetActive(false);
            _playLoop = false;
        }
    }

    public void PlayHealing()
    {
        if (_player._healing)
        {
            if (!_playLoop)
            {
                _playLoop = true;
                healing.SetActive(true);
            }
        }
        else
        {
            healing.SetActive(false);
            _playLoop = false;
        }
    }
}
