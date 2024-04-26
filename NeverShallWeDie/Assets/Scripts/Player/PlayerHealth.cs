using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [Header("Live")]
    [HideInInspector] public int _maxHealth;
    public int _currentHealth;
    public bool _isDead;

    [Header("Elixir")]
    [HideInInspector] public float _maxMana;
    public float _currenteMana;

    [Header("Particle")]
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
        _currentHealth = _maxHealth;
        _currenteMana = _maxMana;
        _startColor = _spriteRenderer.color;
    }

    public void TakeDamage(int damage)
    {
        _player.gameObject.layer = LayerMask.NameToLayer("Invencible");

        _player._onHit = true;
        _player._canMove = false;
        _currentHealth -= damage;
        _audio.PlayAudio(_audio._damageSound); //audio do dano
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
        _currenteMana = _currenteMana < _maxMana ? _currenteMana += healing : _currenteMana = _maxMana;
    }

    public void Healing(int life)
    {
        if (_currentHealth < _maxHealth && _currenteMana >= 1)
        {
            _player._healing = true;
            _currentHealth += life;
            _currenteMana -= life;
            _animation.OnHealing();
        }
        else
        {
            _player._healing = false;
        }
    }

    public void FinishHealing() //chamado na anima��o  
    {   
        _player._healing = false;
    }

    public void ManaConsumption(float consume) //consumir mana ao usar as skills
    { 
        if (_currenteMana > consume)
        {
            _currenteMana -= consume;
        }
        else if (_currenteMana <= consume && _currenteMana > 0)
        {
            _currenteMana = 0f;
        }
    }

    public void CheckAttributes()
    {
        if (!_player._canMove)
            return;

        //TODO: aqui ser� definido a vida m�xima do player
        _maxHealth = 5;

        //TODO: aqui ser� definido a qtd m�xima de mana do player
        _maxMana = 5;
    }

    public void FinishHit()
    {
        StartCoroutine(FinishInvincible());
    }

    IEnumerator FinishInvincible()
    {
        float hitDuration = 0.5f;
        yield return new WaitForSeconds(hitDuration);

        _spriteRenderer.color = _startColor;
        _player.gameObject.layer = LayerMask.NameToLayer("Player");
    }
}
