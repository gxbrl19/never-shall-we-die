using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradePoints : MonoBehaviour
{
    bool _playerTriggered;
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
        if (_playerTriggered && _input.interact)
        {
            _playerTriggered = false;
            _input.interact = false;
            Debug.Log("Melhorando HP");
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
