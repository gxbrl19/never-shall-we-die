using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum UpItems
{
    Potentium, Orb
}

public class UpItem : MonoBehaviour
{
    public UpItems _item;
    public int _id;
    [SerializeField] private Sprite _pickedSprite;
    private bool _triggered = false;
    private Collider2D _collider;
    private AudioItems _audioItems;
    private Player _player;
    private PlayerInputs _input;

    void Start()
    {
        _collider = GetComponent<Collider2D>();
        _audioItems = FindFirstObjectByType<AudioItems>();
        _player = FindFirstObjectByType<Player>();
        _input = _player.GetComponent<PlayerInputs>();
    }

    void Update()
    {
        //verifica se o player já tem o equipamento e desativa o prefab
        if (_item == UpItems.Potentium)
        {
            if (GameManager.instance._potentiuns[_id] == 1) { DisableItem(); }
        }
        else if (_item == UpItems.Orb)
        {
            if (GameManager.instance._orbs[_id] == 1) { DisableItem(); }
        }

        //verifica o input
        if (_input.interact && _triggered)
        {
            DisableItem();
            Invoke("NewItem", 0.5f); //da um delay para a animação de novo equipamento
        }
    }

    void NewItem() //chamado no update
    {
        _audioItems.PlaySound(_audioItems._powerPickupSound, _audioItems._powerVolume);
        _player.SetPowerPickup(_pickedSprite);

        if (_item == UpItems.Potentium) { GameManager.instance._potentiuns[_id] = 1; } else if (_item == UpItems.Orb) { GameManager.instance._orbs[_id] = 1; }
    }

    void DisableItem()
    {
        _collider.enabled = false;
        gameObject.SetActive(false);
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