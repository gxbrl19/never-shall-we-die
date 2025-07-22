using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [Header("Live")]
    [HideInInspector] public float _maxHealth;
    public float _currentHealth;
    public bool _isDead;

    [Header("Elixir")]
    [HideInInspector] public float _maxMana;
    public float _currentMana;

    [Header("Particle")]
    public GameObject _objHealing;
    public GameObject _particleHit;
    public Color _damageColor;

    SpriteRenderer _spriteRenderer;
    Color _startColor;
    Player _player;
    PlayerAudio _audio;
    PlayerAnimations _animation;
    PlayerInputs _inputs;

    private void Awake()
    {
        _player = GetComponent<Player>();
        _audio = GetComponent<PlayerAudio>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _animation = GetComponent<PlayerAnimations>();
        _inputs = GetComponent<PlayerInputs>();
    }

    void Start()
    {
        _maxHealth = 25f;
        _maxMana = 15f;

        _startColor = _spriteRenderer.color;

        _currentHealth = GetHealth();
        _currentMana = GetMana();
    }

    void FixedUpdate()
    {
        Healing();
    }

    public void TakeDamage(int damage)
    {
        if (_isDead) { return; }

        _player.gameObject.layer = LayerMask.NameToLayer("Invencible");

        _player._onHit = true;
        _player._canMove = false;
        _currentHealth -= damage;
        SetHealth(_currentHealth);
        _audio.PlayHit();
        CinemachineShake.instance.ShakeCamera(6f, 0.15f); //tremida da camera
        _spriteRenderer.color = _damageColor;

        GameObject hit = Instantiate(_particleHit, transform.position, _particleHit.transform.rotation);
        Destroy(hit, 2f);

        _animation.OnHit();

        if (_currentHealth < 1)
        {
            //Time.timeScale = 0.5f;
            _audio.PlayDeath();
            _isDead = true;
            _player._body.velocity = Vector2.zero;
            _player.OnDead();
            BackgroundMusic.instance.MusicControl(9);
            LostGold(0.3f);
        }
    }

    void LostGold(float percentual)
    {
        int gold = GameManager.instance._gold;

        if (gold > 0)
        {
            float lostValue = gold * percentual;
            int lostGold = (int)Math.Round(lostValue, MidpointRounding.AwayFromZero); //Arredondar para um número inteiro
            GameManager.instance._gold -= lostGold;
        }
    }

    public void FillBottle(float healing)
    {
        _currentMana = _currentMana < _maxMana ? _currentMana += healing : _currentMana = _maxMana;
        SetMana(_currentMana);
    }

    public void Healing()
    {
        _objHealing.SetActive(_player._healing);

        if (_player._healing && _currentHealth < _maxHealth && _currentMana >= 0.1)
        {
            _currentHealth += .06f;
            _currentMana -= .06f;

            SetHealth(_currentHealth);
            SetMana(_currentMana);

            CinemachineShake.instance.ShakeCamera(3f, 0.15f);
        }
        else
        {
            _player._healing = false;
        }
    }

    public void ManaConsumption(float consume) //consumir mana ao usar as skills
    {
        if (_currentMana > consume) { _currentMana -= consume; }
        else if (_currentMana <= consume && _currentMana > 0) { _currentMana = 0f; }

        SetMana(_currentMana);
    }

    public void FinishHit()
    {
        StartCoroutine(FinishInvincible());
    }

    public void ResetHealth()
    {
        _currentHealth = _maxHealth;
        _currentMana = 0f;
    }

    IEnumerator FinishInvincible()
    {
        float hitDuration = 0.5f;
        yield return new WaitForSeconds(hitDuration);

        _spriteRenderer.color = _startColor;
        _player.gameObject.layer = LayerMask.NameToLayer("Player");
    }

    #region "GameManager"

    public void SetHealth(float health) //usado para manter o HP na troca de cena
    {
        GameManager.instance._currentHP = health;
    }

    public float GetHealth()
    {
        float health = GameManager.instance._currentHP != 0 ? GameManager.instance._currentHP : _maxHealth;
        return health;
    }

    public void SetMana(float mana) //usado para manter o MP na troca de cena
    {
        GameManager.instance._currentMP = mana;
    }

    public float GetMana()
    {
        float mana = GameManager.instance._currentMP != 0 ? GameManager.instance._currentMP : 0f;
        return mana;
    }
    #endregion
}
