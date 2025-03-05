using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UpgradeShip : MonoBehaviour
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
            VerifyProjects(); //desabilita o projeto que já foi feito
            UIManager.instance.ActivePanelShip();
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

    private void VerifyProjects()
    {
        if (GameManager.instance._submarine == 1) { UIManager.instance._txtsUpgradeShip[0].enabled = true; }
        else { UIManager.instance._txtsUpgradeShip[0].enabled = false; }
        //
        if (GameManager.instance._propulsion == 1) { UIManager.instance._txtsUpgradeShip[1].enabled = true; }
        else { UIManager.instance._txtsUpgradeShip[1].enabled = false; }
        //
        if (GameManager.instance._artillery == 1) { UIManager.instance._txtsUpgradeShip[2].enabled = true; }
        else { UIManager.instance._txtsUpgradeShip[2].enabled = false; }
    }
}
