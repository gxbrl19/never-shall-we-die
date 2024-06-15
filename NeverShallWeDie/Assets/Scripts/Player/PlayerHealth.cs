using JetBrains.Annotations;
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
        CheckAttributes();
        _startColor = _spriteRenderer.color;

        _currentHealth = GetHealth();
        _currentMana = GetMana();
    }

    void Update()
    {
        Healing();
    }

    public void TakeDamage(int damage)
    {
        _player.gameObject.layer = LayerMask.NameToLayer("Invencible");

        _player._onHit = true;
        _player._canMove = false;
        _currentHealth -= damage;
        SetHealth(_currentHealth);
        _audio.PlayAudio(_audio._damage); //audio do dano
        CinemachineShake.instance.ShakeCamera(6f, 0.15f); //tremida da camera        
        _spriteRenderer.color = _damageColor;

        GameObject hit = Instantiate(_particleHit, transform.position, _particleHit.transform.rotation);
        Destroy(hit, 2f);

        _animation.OnHit();

        if (_currentHealth < 1)
        {
            Time.timeScale = 0.5f;
            _isDead = true;
            _player._body.velocity = Vector2.zero;
            _player.OnDead();
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
            _currentHealth += 0.1f;
            _currentMana -= 0.1f;

            //deixa zerado quando os valores forem negativos
            if (_currentHealth < 0) { _currentHealth = 0f; } else if (_currentHealth > _maxHealth) { _currentHealth = _maxHealth; }
            if (_currentMana < 0) { _currentMana = 0f; } else if (_currentMana > _maxMana) { _currentMana = _maxMana; }


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

    public void CheckAttributes()
    {
        if (!_player._canMove)
            return;

        //TODO: aqui será definido a vida máxima do player
        _maxHealth = 25;

        //TODO: aqui será definido a qtd máxima de mana do player
        _maxMana = 15;
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

    #region "Prefabs" 
    //PlayerPrefs para a troca de cena
    public void SetHealth(float health)
    {
        PlayerPrefs.SetFloat("Health", health);
    }

    public float GetHealth()
    {
        float health = PlayerPrefs.HasKey("Health") ? PlayerPrefs.GetFloat("Health") : _maxHealth;
        return health;
    }

    public void SetMana(float mana)
    {
        PlayerPrefs.SetFloat("Mana", mana);
    }

    public float GetMana()
    {
        float mana = PlayerPrefs.HasKey("Mana") ? PlayerPrefs.GetFloat("Mana") : 0f;
        return mana;
    }
    #endregion
}
