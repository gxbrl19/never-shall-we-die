using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToNavigate : MonoBehaviour
{
    public bool _playerTriggered;
    Player _player;
    PlayerInputs _input;
    Collider2D _collider;

    void Awake()
    {
        _collider = GetComponent<Collider2D>();
        _player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        _input = _player.GetComponent<PlayerInputs>();
    }

    void Update()
    {
        //verifica se a bandeira do navio está hasteada
        if (GameManager.instance._flags[0] == 1) { _collider.enabled = true; } else { _collider.enabled = false; }

        if (_playerTriggered && _input.pressInteract)
        {
            _playerTriggered = false;
            _input.pressInteract = false;
            UIManager.instance.ActivePanelNavigate();
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
}
