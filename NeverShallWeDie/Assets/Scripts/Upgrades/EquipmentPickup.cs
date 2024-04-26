using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentPickup : MonoBehaviour
{
    [Header("Components")]
    public EquipmentObject _equipmentObject;

    private Equipments _equipment;
    private Sprite _pickedSprite;
    private string _name;
    private bool _triggered = false;

    private Collider2D _collider;
    private AudioSource _audio;
    private AudioItems _audioItems;
    private Player _player;
    private PlayerInputs _input;

    void Start()
    {
        _collider = GetComponent<Collider2D>();
        _audio = GetComponent<AudioSource>();
        _audioItems = FindFirstObjectByType<AudioItems>();
        _player = FindFirstObjectByType<Player>();
        _input = _player.GetComponent<PlayerInputs>();

        //dados da skill
        _equipment = _equipmentObject.equipment;
        _pickedSprite = _equipmentObject.sprite;
        _name = _equipmentObject.name;
    }

    void Update()
    {
        //verifica se o player já tem a habilidade e desativa o prefab
        for (int i = 0; i < PlayerEquipment.instance.equipments.Count; i++)
        {
            if (PlayerEquipment.instance.equipments[i] == _equipment) { DisableSkill(); }
        }

        //verifica o input
        if (_input.vertical == 1 && _triggered)
        {
            DisableSkill();
            Invoke("NewSkill", 0.5f); //da um delay para a animação de nova skill      
        }
    }

    void NewSkill() //chamado no update
    {
        _audioItems.PlaySound(_audioItems._powerPickupSound, _audioItems._powerVolume);
        _player.SetPowerPickup(_pickedSprite);

        GameManager.instance._equipments.Add(_equipment);
        PlayerEquipment.instance.equipments.Add(_equipment);
    }

    void DisableSkill()
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
