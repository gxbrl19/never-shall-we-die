using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemsItens : MonoBehaviour
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
        if (_item == Items.WaterGem)
        {
            if (InventorySystem.instance.items.Contains(Items.WaterGem)) { DisableItem(); }
        }
        else if (_item == Items.FireGem)
        {
            if (InventorySystem.instance.items.Contains(Items.FireGem)) { DisableItem(); }
        }
        else if (_item == Items.AirGem)
        {
            if (InventorySystem.instance.items.Contains(Items.AirGem)) { DisableItem(); }
        }

        //verifica o input
        if (_input.pressInteract && _triggered)
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
