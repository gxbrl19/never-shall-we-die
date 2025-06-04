using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class EnemyController : MonoBehaviour
{
    public EnemyObject _enemyObject;
    [HideInInspector] public string _name;
    [HideInInspector] public int _maxHealth;
    [HideInInspector] public int _itemDropRate;
    [HideInInspector] public Color _damageColor;

    //Enemy Data
    public int _currentHealth;
    public bool _onHit;
    public bool _isDead;
    [SerializeField] GameObject _deathEffect;
    [HideInInspector] public Animator _animation;

    [Header("FMOD Events")]
    [SerializeField] EventReference hit;
    [SerializeField] EventReference death;

    Color _defaultColor;
    SpriteRenderer _sprite;
    DropItem _dropItem;

    void Awake()
    {
        _animation = GetComponent<Animator>();
        _dropItem = GetComponent<DropItem>();
        _sprite = GetComponent<SpriteRenderer>();


        //
        _name = _enemyObject.name;
        _maxHealth = _enemyObject.maxHealth;
        _itemDropRate = _enemyObject.dropRate;
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
        if (_onHit || _name == "Beetboom" || _name == "Barrel Monkey") { return; }

        _onHit = true;
        _currentHealth -= damage;
        _sprite.color = _damageColor;
        CinemachineShake.instance.ShakeCamera(3f, 0.15f); //tremida na camera

        if (_currentHealth > 0)
        {
            RuntimeManager.PlayOneShot(hit);
        }
        else //morte do inimigo
        {
            //_dropItem.DropGold();
            _isDead = true;
            _animation.SetBool("Dead", true);
            RuntimeManager.PlayOneShot(death);

            //da um pouco de mana e XP ao player
            Instantiate(_deathEffect, transform.position, Quaternion.identity);
            PlayerHealth _playerHealth = FindFirstObjectByType<PlayerHealth>();
            _playerHealth.FillBottle(3f);
            PlayerLevel _playerLevel = FindFirstObjectByType<PlayerLevel>();
            _playerLevel.GainXP(5);
        }

        Invoke("FinishHit", 0.3f);
    }

    public void FinishHit() //chamado no TakeDamage()
    {
        _onHit = false;
        _sprite.color = _defaultColor;
    }
}
