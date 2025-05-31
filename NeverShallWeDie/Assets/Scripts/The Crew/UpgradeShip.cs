using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class UpgradeShip : MonoBehaviour
{
    bool _playerTriggered;
    Player _player;
    PlayerInputs _input;
    Collider2D _collider;
    Animator _animation;
    [SerializeField] Transform _body;
    [SerializeField] Transform _workPosition;
    [SerializeField] Transform  _initialPosition;

    bool _walk;
    bool _return;
    float _speed = 2f;

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
            VerifyProjects(); //desabilita o projeto que já foi feito
            UIManager.instance.ActivePanelShip();
        }

        if (_walk)
        {
            _body.Translate(new Vector3(_speed * -1, 0f, 0f) * Time.deltaTime);
            if (_body.position.x <= _workPosition.position.x)
            {
                BeginWork();
            }
        }

        if (_return)
        {
            _body.Translate(new Vector3(_speed * 1, 0f, 0f) * Time.deltaTime);
            if (_body.position.x >= _initialPosition.position.x)
            {
                FinishUpgrade();
            }
        }

        _animation.SetBool("Action", (_return || _walk));
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
        if (InventorySystem.instance.items.Contains(Items.Submarine)) { UIManager.instance._txtsUpgradeShip[0].enabled = true; }
        else { UIManager.instance._txtsUpgradeShip[0].enabled = false; }
        //
        if (InventorySystem.instance.items.Contains(Items.Propulsion)) { UIManager.instance._txtsUpgradeShip[1].enabled = true; }
        else { UIManager.instance._txtsUpgradeShip[1].enabled = false; }
        //
        if (InventorySystem.instance.items.Contains(Items.Cannon)) { UIManager.instance._txtsUpgradeShip[2].enabled = true; }
        else { UIManager.instance._txtsUpgradeShip[2].enabled = false; }
    }

    public void Action()
    {
        _walk = true;
        Flip();
        _player.DisableControls();
    }

    public void BeginWork()
    {
        _walk = false;
        AudioHUD.instance.PlayUpgradeShip();
        Invoke("Return", 3f);
    }

    void Return()
    {
        _return = true;
        Flip();
    }

    public void FinishUpgrade()
    {
        _return = false;
        AudioItems.instance.PlayNewSkill();
        _player.SetPowerPickup(null);
    }

    void Flip()
    {
        Vector3 _scale = _body.localScale;
        _scale.x *= -1;
        _body.localScale = _scale;
    }
}
