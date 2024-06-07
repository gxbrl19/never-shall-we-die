using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour
{
    public BossObject _bossObject;

    [HideInInspector] public string _name;
    [HideInInspector] public int _maxHealth;
    [HideInInspector] public int _itemDropRate;
    [HideInInspector] public AudioClip _deadSound;
    [HideInInspector] public float _volume;
    [HideInInspector] public Color _damageColor;

    //Enemy Data
    public int _currentHealth;
    [HideInInspector] public bool _onHit;
    [HideInInspector] public bool _isDead;
    [HideInInspector] public Animator _animation;

    Color _defaultColor;
    SpriteRenderer _sprite;
    AudioSource _audio;

    void Awake()
    {
        _animation = GetComponent<Animator>();

        UIManager.instance._txtBossName.text = _bossObject.name;
        _maxHealth = _bossObject.maxHealth;
        _itemDropRate = _bossObject.dropRate;
        _deadSound = _bossObject.deadSound;
        _volume = _bossObject.volume;
        _damageColor = _bossObject.damageColor;
    }

    void Update()
    {

    }

    public void TakeDamage(int damage)
    {
        if (_onHit) { return; }

        _onHit = true;
        _currentHealth -= damage;
        _audio.volume = _volume;
        _sprite.color = _damageColor;
        AudioItems.instance.PlaySound(AudioItems.instance._hitSound, AudioItems.instance._hitVolume);
        Invoke("FinishHit", 0.3f);
        
        if (_currentHealth <= 0)
        {
            _isDead = true;
            _animation.SetTrigger("Dead");
        }
    }

    public void FinishHit() //chamado na animação
    {
        _onHit = false;
        _sprite.color = _defaultColor;
    }
}
