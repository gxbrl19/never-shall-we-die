using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public EnemyObject _enemyObject;
    [HideInInspector] public string _name;
    [HideInInspector] public int _maxHealth;
    [HideInInspector] public int _itemDropRate;
    [HideInInspector] public AudioClip _deadSound;
    [HideInInspector] public float _volume;
    [HideInInspector] public Color _damageColor;

    //Enemy Data
    public int _currentHealth;
    public bool _onHit;
    public bool _isDead;
    [HideInInspector] public Animator _animation;


    Color _defaultColor;
    SpriteRenderer _sprite;
    AudioSource _audio;
    DropItem _dropItem;

    void Awake()
    {
        _animation = GetComponent<Animator>();
        _audio = GetComponent<AudioSource>();
        _dropItem = GetComponent<DropItem>();
        _sprite = GetComponent<SpriteRenderer>();


        //
        _name = _enemyObject.name;
        _maxHealth = _enemyObject.maxHealth;
        _itemDropRate = _enemyObject.dropRate;
        _deadSound = _enemyObject.deadSound;
        _volume = _enemyObject.volume;
        _damageColor = _enemyObject.damageColor;


        //
        _currentHealth = _maxHealth;
        _isDead = false;
        _onHit = false;
        _defaultColor = _sprite.color;

        if (_dropItem != null)
        {
            _dropItem._dropRate = _itemDropRate;
        }
    }

    public void TakeDamage(int damage)
    {
        if (_onHit || _name == "Beetboom") { return; }

        _onHit = true;
        _currentHealth -= damage;
        _audio.volume = _volume;
        _sprite.color = _damageColor;
        AudioItems.instance.PlaySound(AudioItems.instance._hitSound, AudioItems.instance._hitVolume);
        CinemachineShake.instance.ShakeCamera(3f, 0.15f); //tremida na camera
        Invoke("FinishHit", 0.3f);

        if (_currentHealth <= 0)
        {
            _audio.PlayOneShot(_deadSound);
            _dropItem.DropGold();
            _isDead = true;
            _animation.SetBool("Dead", true);

            //da um pouco de mana ao player
            PlayerHealth _playerHealth = FindFirstObjectByType<PlayerHealth>();
            _playerHealth.FillBottle(1.5f);
        }

        /*if (_name == "Boar") {
            Boar _boar = GetComponent<Boar>();
            _boar.Knockback();
        }*/
    }

    public void FinishHit() //chamado no TakeDamage()
    {
        _onHit = false;
        _sprite.color = _defaultColor;
    }
}
