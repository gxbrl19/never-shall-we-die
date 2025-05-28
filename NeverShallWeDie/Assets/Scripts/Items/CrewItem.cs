using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CrewItem : MonoBehaviour
{
    public ItemObject _itemObject;

    private Items _item;
    private Sprite _sprite;
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

        _item = _itemObject.item;
        _sprite = _itemObject.sprite;
    }

    void Update()
    {
        //verifica se o player já tem o equipamento e desativa o prefab
        if (_item == Items.Hammer)
        {
            if (InventorySystem.instance.items.Contains(Items.Hammer)) { DisableItem(); }
        }
        else if (_item == Items.Grimoire)
        {
            if (InventorySystem.instance.items.Contains(Items.Grimoire)) { DisableItem(); }
        }
        else if (_item == Items.Submarine)
        {
            if (InventorySystem.instance.items.Contains(Items.Submarine)) { DisableItem(); }
        }
        else if (_item == Items.Propulsion)
        {
            if (InventorySystem.instance.items.Contains(Items.Propulsion)) { DisableItem(); }
        }
        else if (_item == Items.Cannon)
        {
            if (InventorySystem.instance.items.Contains(Items.Cannon)) { DisableItem(); }
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
        _audioItems.PlayNewSkill();
        _player.SetPowerPickup(_sprite);

        GameManager.instance._inventory.Add(_item);
        InventorySystem.instance.items.Add(_item);

        UIManager.instance.InventoryController(_itemObject);
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
