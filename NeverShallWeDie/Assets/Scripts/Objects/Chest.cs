using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;
using FMODUnity;

public class Chest : MonoBehaviour
{
    public int _idChest;

    bool _triggered;
    Animator _animation;
    Collider2D _collider;
    Player _player;
    PlayerInputs _input;
    DropItem _dropItem;

    [Header("FMOD Events")]
    [SerializeField] EventReference openChest;

    void Start()
    {
        _animation = GetComponent<Animator>();
        _collider = GetComponent<BoxCollider2D>();
        _dropItem = GetComponent<DropItem>();
        _player = FindFirstObjectByType<Player>();
        _input = _player.GetComponent<PlayerInputs>();
    }

    void Update()
    {
        // verifica se ainda não foi aberto
        bool enabled = GameManager.instance._chests[_idChest] == 0;
        _animation.enabled = !enabled;
        _collider.enabled = enabled;

        //verifica o input
        if (_input.interact && _triggered)
        {
            RuntimeManager.PlayOneShot(openChest);
            DisableChest();
        }
    }

    void DisableChest()
    {
        _triggered = false;
        _input.interact = false;
        _dropItem.DropChest(_idChest);
        GameManager.instance._chests[_idChest] = 1;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Player _player = other.GetComponent<Player>();

        if (_player != null)
        {
            _triggered = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        Player _player = other.GetComponent<Player>();

        if (_player != null)
        {
            _triggered = false;
        }
    }
}
