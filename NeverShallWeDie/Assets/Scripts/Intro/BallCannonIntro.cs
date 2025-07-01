using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class BallCannonIntro : MonoBehaviour
{
    Animator _cannonAnimation;
    [SerializeField] EventReference _explosion1;
    [SerializeField] EventReference _explosion2;
    Player _player;

    void Awake()
    {
        _cannonAnimation = GetComponent<Animator>();
        _player = FindObjectOfType<Player>();
    }

    public void FireBall()
    {
        _cannonAnimation.SetTrigger("Fire");
    }

    public void PlayExplosion1() //chamado na animação
    {
        RuntimeManager.PlayOneShot(_explosion1);
    }

    public void PlayExplosion2() //chamado na animação
    {
        GameManager.instance._intro = 1;
        RuntimeManager.PlayOneShot(_explosion2);
        _player.EnabledControls();
    }
}
