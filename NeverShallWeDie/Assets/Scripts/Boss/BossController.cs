using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class BossController : MonoBehaviour
{
    public BossObject _bossObject;
    bool _enabled;

    [HideInInspector] public string _name;
    [HideInInspector] public float _maxHealth;
    [HideInInspector] public int _bossID;
    [HideInInspector] public Color _damageColor;

    //Enemy Data
    public float _currentHealth;
    [HideInInspector] public bool _onHit;
    [HideInInspector] public bool _isDead;
    [HideInInspector] public Animator _animation;

    [SerializeField] BossDoor _bossDoor;
    [SerializeField] BossDoor _bossDoor2;
    [SerializeField] WantedBoss _wantedBoss;

    [Header("FMOD Events")]
    [SerializeField] EventReference hit;
    [SerializeField] EventReference dead;


    Color _defaultColor;
    SpriteRenderer _sprite;
    AudioSource _audio;

    void Awake()
    {
        _animation = GetComponent<Animator>();
        _sprite = GetComponent<SpriteRenderer>();
        _audio = GetComponent<AudioSource>();


        _maxHealth = _bossObject.maxHealth;
        _bossID = _bossObject.bossID;
        _damageColor = _bossObject.damageColor;

        _currentHealth = _maxHealth;
        _isDead = false;
        _enabled = false;
        _onHit = false;
        _defaultColor = _sprite.color;
    }

    void Update()
    {
        if (_enabled == true)
        {
            UIManager.instance._healthBoss.fillAmount = _currentHealth / _maxHealth;
        }
    }

    public void TakeDamage(int damage)
    {
        if (_onHit) { return; }

        _onHit = true;
        _currentHealth -= damage;
        PlayHit();
        _sprite.color = _damageColor;
        AudioItems.instance.PlaySound(AudioItems.instance._hitSound, AudioItems.instance._hitVolume);
        Invoke("FinishHit", 0.3f);

        if (_currentHealth <= 0)
        {
            if (_isDead) { return; }

            _isDead = true;
            _animation.SetTrigger("Dead");
            GameManager.instance._bosses[_bossID] = 1;
            _bossDoor._tiggered = false;
            _bossDoor2._tiggered = false;
            UIManager.instance.BossDisabled();
            PlayDead();
            BackgroundMusic.instance.FinishBoss();
        }
    }

    public void EnabledUI()
    {
        _enabled = true;
        UIManager.instance._txtBossName.text = _bossObject.name;
    }

    public void FinishHit() //chamado no TakeDamage()
    {
        _onHit = false;
        _sprite.color = _defaultColor;
    }

    public void SetWanted() //chamado na animação de morte
    {
        _wantedBoss.StartWanted();
    }

    public void PlayHit()
    {
        RuntimeManager.PlayOneShot(dead);
    }

    public void PlayDead()
    {
        RuntimeManager.PlayOneShot(hit);
    }
}
