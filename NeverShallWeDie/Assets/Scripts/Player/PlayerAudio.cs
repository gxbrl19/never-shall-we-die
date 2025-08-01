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
    [SerializeField] EventReference watersplash;
    [SerializeField] EventReference swim;
    [SerializeField] EventReference dash;
    [SerializeField] EventReference parachute;

    [SerializeField] EventReference waterspin;
    [SerializeField] EventReference aircut;

    [SerializeField] GameObject healing;
    [SerializeField] GameObject grabing;

    bool _playLoop;

    Player _player;
    PlayerInputs _input;

    void Awake()
    {
        _player = GetComponent<Player>();
        _input = _player.GetComponent<PlayerInputs>();
    }

    private void Update()
    {
        PlayHealing();
        PlayGrab();
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

    public void PlayWaterSplash() //script
    {
        RuntimeManager.PlayOneShot(watersplash);
    }

    public void PlaySwim() //animação
    {
        RuntimeManager.PlayOneShot(swim);
    }
    public void PlayDash() //animação
    {
        RuntimeManager.PlayOneShot(dash);
    }

    public void PlayParachute() //script
    {
        RuntimeManager.PlayOneShot(parachute);
    }

    public void PlayWaterSpin() //animação
    {
        RuntimeManager.PlayOneShot(waterspin);
    }

    public void PlayAircut() //animação
    {
        RuntimeManager.PlayOneShot(aircut);
    }

    public void PlayGrab()
    {
        if (_player.isGrabing && _input.horizontal != 0)
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
        if (_player.isHealing)
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
