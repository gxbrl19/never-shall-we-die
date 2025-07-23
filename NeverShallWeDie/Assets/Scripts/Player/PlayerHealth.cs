using System;
using System.Collections;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [Header("Vida e Mana")]
    [HideInInspector] public float _maxHealth;
    public float _currentHealth;
    [HideInInspector] public float _maxMana;
    public float _currentMana;

    [Header("Estados")]
    public bool _isDead;

    [Header("Partículas & Visual")]
    public GameObject _objHealing;
    public GameObject _particleHit;
    public Color _damageColor;

    SpriteRenderer spriteRenderer;
    Color startColor;

    Player player;
    PlayerAudio playerAudio;
    PlayerAnimations playerAnimations;
    PlayerInputs playerInput;

    private void Awake()
    {
        player = GetComponent<Player>();
        playerAudio = GetComponent<PlayerAudio>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerAnimations = GetComponent<PlayerAnimations>();
        playerInput = GetComponent<PlayerInputs>();
    }

    void Start()
    {
        _maxHealth = 25f;
        _maxMana = 15f;

        startColor = spriteRenderer.color;
        _currentHealth = GetHealth();
        _currentMana = GetMana();
    }

    void FixedUpdate()
    {
        Healing();
        OnHit(); // chamada constante enquanto _onHit for true
    }

    public void TakeDamage(int damage)
    {
        if (_isDead) return;

        player.gameObject.layer = LayerMask.NameToLayer("Invencible");
        player.onHit = true;
        player.canMove = false;

        _currentHealth -= damage;
        SetHealth(_currentHealth);

        playerAudio.PlayHit();
        CinemachineShake.instance.ShakeCamera(6f, 0.15f);
        spriteRenderer.color = _damageColor;

        Instantiate(_particleHit, transform.position, Quaternion.identity);
        playerAnimations.OnHit();

        if (_currentHealth < 1)
        {
            _isDead = true;
            playerAudio.PlayDeath();
            player.rb.velocity = Vector2.zero;
            player.OnDead();
            BackgroundMusic.instance.MusicControl(9);
            LostGold(0.3f);
        }
    }

    void OnHit()
    {
        if (!player.onHit || player.isDead) return;

        playerInput.OnHit(); //cancela input
        player.rb.velocity = Vector2.zero;
    }

    public void FinishHit() //chamado da animação de Hit
    {
        player.onHit = false;
        player.canMove = true;
        StartCoroutine(FinishInvincible());
    }

    IEnumerator FinishInvincible()
    {
        float hitDuration = 0.8f;
        float elapsed = 0f;
        bool visible = true;

        while (elapsed < hitDuration)
        {
            spriteRenderer.color = visible ? new Color(1, 1, 1, 0.5f) : _damageColor;
            visible = !visible;
            yield return new WaitForSeconds(0.1f);
            elapsed += 0.1f;
        }

        spriteRenderer.color = startColor;
        player.gameObject.layer = LayerMask.NameToLayer("Player");
    }

    void LostGold(float percentual)
    {
        int gold = GameManager.instance._gold;

        if (gold > 0)
        {
            int lostGold = Mathf.RoundToInt(gold * percentual);
            GameManager.instance._gold -= lostGold;
        }
    }

    public void FillBottle(float healing)
    {
        _currentMana = Mathf.Min(_currentMana + healing, _maxMana);
        SetMana(_currentMana);
    }

    public void Healing()
    {
        _objHealing.SetActive(player.isHealing);

        if (player.isHealing && _currentHealth < _maxHealth && _currentMana >= 0.1f)
        {
            _currentHealth += .06f;
            _currentMana -= .06f;

            SetHealth(_currentHealth);
            SetMana(_currentMana);
            CinemachineShake.instance.ShakeCamera(3f, 0.15f);
        }
        else
        {
            player.isHealing = false;
        }
    }

    public void ManaConsumption(float consume)
    {
        _currentMana = Mathf.Max(_currentMana - consume, 0f);
        SetMana(_currentMana);
    }


    public void ResetHealth()
    {
        _currentHealth = _maxHealth;
        _currentMana = 0f;
    }

    #region GameManager (Persistência entre cenas)

    public void SetHealth(float health)
    {
        GameManager.instance._currentHP = health;
    }

    public float GetHealth()
    {
        return GameManager.instance._currentHP != 0 ? GameManager.instance._currentHP : _maxHealth;
    }

    public void SetMana(float mana)
    {
        GameManager.instance._currentMP = mana;
    }

    public float GetMana()
    {
        return GameManager.instance._currentMP != 0 ? GameManager.instance._currentMP : 0f;
    }

    #endregion
}
