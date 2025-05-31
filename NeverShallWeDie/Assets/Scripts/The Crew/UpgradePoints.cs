using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradePoints : MonoBehaviour
{
    int _price = 200;
    bool _playerTriggered;
    Player _player;
    PlayerInputs _input;
    Collider2D _collider;
    Animator _animation;

    void Awake()
    {
        _collider = GetComponent<Collider2D>();
        _player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        _input = _player.GetComponent<PlayerInputs>();
        _animation = GetComponentInParent<Animator>();
    }

    void Update()
    {
        if (_playerTriggered && _input.interact)
        {
            _playerTriggered = false;
            _input.interact = false;
            UIManager.instance._qtdOrbs = GameManager.instance._qtdOrb;
            UIManager.instance._UpHpMpPrice = _price;
            UIManager.instance.ActivePanelHpMp();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player") || other.gameObject.layer == LayerMask.NameToLayer("Invencible"))
        {
            _playerTriggered = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player") || other.gameObject.layer == LayerMask.NameToLayer("Invencible"))
        {
            _playerTriggered = false;
        }
    }

    public void Action()
    {
        _animation.SetBool("Action", true);
        Invoke("FinishUpgrade", 2f);
        _player.DisableControls();
    }

    public void FinishUpgrade() //chamado na função Action
    {
        _animation.SetBool("Action", false);
        _player.EnabledControls();
    }
}
