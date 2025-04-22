using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class Drawbridge : MonoBehaviour
{
    [SerializeField] Animator _animMechanism;
    [SerializeField] Animator _animation;

    [Header("FMOD Events")]
    //[SerializeField] EventReference portalOpen;

    Player _player;

    private void Awake()
    {
        _player = FindObjectOfType<Player>();
    }

    private void Start()
    {
        //verifica se já foi acionado
        bool finish = GameManager.instance._drawbridge == 1;
        _animation.SetBool("Finish", finish);
    }

    public void EnabledBridge()
    {
        GameManager.instance._drawbridge = 1;
        _animation.SetBool("Enabled", true);
        _animMechanism.SetBool("Enabled", true);
        //RuntimeManager.PlayOneShot(portalOpen);
        Invoke("FinishEnabled", 2.02f);
    }

    public void FinishEnabled()
    {
        _player.EnabledControls();
        _animation.SetBool("Finish", true);
        _animMechanism.SetBool("Finish", true);
    }
}
