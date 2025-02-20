using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour
{
    public BossObject _bossObject;
    bool _enabled;

    [HideInInspector] public string _name;
    [HideInInspector] public float _maxHealth;
    [HideInInspector] public int _bossID;
    [HideInInspector] public AudioClip _deadSound;
    [HideInInspector] public float _volume;
    [HideInInspector] public Color _damageColor;

    //Enemy Data
    public float _currentHealth;
    [HideInInspector] public bool _onHit;
    [HideInInspector] public bool _isDead;
    [HideInInspector] public Animator _animation;

    [SerializeField] BossDoor _bossDoor;
    [SerializeField] BossDoor _bossDoor2;
    [SerializeField] WantedBoss _wantedBoss;

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
        _deadSound = _bossObject.deadSound;
        _volume = _bossObject.volume;
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
        _audio.volume = _volume;
        _audio.PlayOneShot(_deadSound);
        _sprite.color = _damageColor;
        AudioItems.instance.PlaySound(AudioItems.instance._hitSound, AudioItems.instance._hitVolume);
        Invoke("FinishHit", 0.3f);

        if (_currentHealth <= 0)
        {
            _isDead = true;
            _animation.SetTrigger("Dead");
            GameManager.instance._bosses[_bossID] = 1;
            _bossDoor._tiggered = false;
            _bossDoor2._tiggered = false;
            UIManager.instance.BossDisabled();
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
        _wantedBoss._bossName = _bossObject.name;
        _wantedBoss._bossImage = _bossObject.sprite;
        _wantedBoss.StartWanted();
    }
}
