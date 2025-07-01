using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class BallCannonIntro : MonoBehaviour
{
    [SerializeField] GameObject _fadeIntro;
    [SerializeField] EventReference _explosion1;
    [SerializeField] EventReference _explosion2;
    Animator _cannonAnimation;
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
        _fadeIntro.SetActive(true);
        RuntimeManager.PlayOneShot(_explosion2);
        Invoke("FinishIntro", 3f);
    }

    void FinishIntro() //chamado no PlayExplosion2()
    {
        GameManager.instance._intro = 1;
        _player.EnabledControls();
    }
}
