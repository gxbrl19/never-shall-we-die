﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

public class Chest : MonoBehaviour
{
    [Header("Components")]
    public SkillObject _skillObject;

    private Skills _skill;
    private Sprite _pickedSprite;
    private string _name;
    private bool _triggered = false;

    private Animator _animation;
	private BoxCollider2D _collider;
	private AudioSource _audio;
	private AudioItems _audioItems;
    private Player _player;
    private PlayerInputs _input;

	public AudioClip _openChest;

	void Start()
	{
		_animation = GetComponent<Animator>();
		_collider = GetComponent<BoxCollider2D>();
		_audio = GetComponent<AudioSource>();
        _audioItems = FindFirstObjectByType<AudioItems>();
        _player = FindFirstObjectByType<Player>();
        _input = _player.GetComponent<PlayerInputs>();

        //dados da skill
        _skill = _skillObject.skill;
        _pickedSprite = _skillObject.sprite;
        _name = _skillObject.name;
    }

    void Update()
    {
        //verifica se o player já tem a habilidade e desativa o baú
        for (int i = 0; i < PlayerSkills.instance.skills.Count; i++)
        {
            if (PlayerSkills.instance.skills[i] == _skill)
            {
                DisableChest();
            }
        }

        //verifica o input
        if(_input.vertical == 1 && _triggered)
        {
            _audio.PlayOneShot(_openChest);
            DisableChest();

            Invoke("NewSkill", 0.5f); //da um delay para a animação de nova skill      
        }
    }

    void NewSkill() //chamado no update
    {
        _audioItems.PlaySound(_audioItems._powerPickupSound, _audioItems._powerVolume);
        _player.SetPowerPickup(_pickedSprite);

        GameManager.instance._skills.Add(_skill);
        PlayerSkills.instance.skills.Add(_skill);
    }

    void DisableChest()
    {
        _collider.enabled = false;
        _animation.enabled = true;
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