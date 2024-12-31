using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateBoss : MonoBehaviour
{
    [SerializeField] AudioClip _moveSound;
    [SerializeField] Animator _animation;
    [SerializeField] Animator _animationMechanism;

    Player _player;

    private void Awake()
    {
        _player = FindObjectOfType<Player>();
    }

    private void Start()
    {
        //verifica se já foi acionado
        bool finish = GameManager.instance._gateBoss == 2;
        _animation.SetBool("Finish", finish);

        bool ok = GameManager.instance._gateBoss > 0;
        _animationMechanism.SetBool("Ok", ok);
    }

    public void EnabledGate()
    {
        GameManager.instance._gateBoss = 2;
        _animation.SetBool("Enabled", true);
        _animationMechanism.SetBool("Working", true);
        Invoke("FinishEnabled", 2.02f);
    }

    public void FinishEnabled()
    {
        _animation.SetBool("Finish", true);
        _animationMechanism.SetBool("Open", true);
    }
}
