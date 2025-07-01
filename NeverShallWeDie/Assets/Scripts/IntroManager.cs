using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class IntroManager : MonoBehaviour
{
    BallCannonIntro _ball;
    Player _player;

    void Awake()
    {
        _ball = GetComponentInChildren<BallCannonIntro>();
        _player = FindObjectOfType<Player>();
    }

    void Start()
    {
        if (GameManager.instance._intro == 1) { return; }
        _player.DisableControls();
    }

    public void NextState()
    {
        _ball.FireBall();
    }
}
