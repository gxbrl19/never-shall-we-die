using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WantedBoss : MonoBehaviour
{
    Animator _animation;
    Player _player;

    void Awake()
    {
        _player = FindObjectOfType<Player>();
        _animation = GetComponent<Animator>();
    }

    public void StartWanted()
    {
        _animation.SetBool("Finish", true);
        _player.DisableControls();
    }

    public void FinishWanted() //chamado na animação
    {
        _player.EnabledControls();
    }
}
