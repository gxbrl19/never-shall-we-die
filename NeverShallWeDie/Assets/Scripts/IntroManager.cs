using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class IntroManager : MonoBehaviour
{
    Player _player;
    PlayerAnimations _animation;

    void Awake()
    {
        _player = FindObjectOfType<Player>();
        _animation = _player.gameObject.GetComponent<PlayerAnimations>();
    }

    void Start()
    {
        if (GameManager.instance._intro == 1) { return; }
        _player.DisableControls();
    }

    public void NextState()
    {
        GameManager.instance._intro = 1;
        _player.EnabledControls();
    }
}
