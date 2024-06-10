using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Flag : MonoBehaviour
{
    public int _idFlag;
    public int _direction = 1;
    bool _playerTriggered;
    Animator _animation;
    Player _player;
    PlayerInputs _inputs;
    PlayerHealth _health;

    void Awake()
    {
        _animation = GetComponent<Animator>();
        _player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        _inputs = _player.GetComponent<PlayerInputs>();
        _health = _player.GetComponent<PlayerHealth>();
    }

    void Update()
    {
        int _enabled = GameManager.instance._flags[_idFlag];

        if (_enabled == 0)
        {
            if (_playerTriggered && _inputs.interact)
            {
                _animation.SetBool("Begin", true);
                _animation.SetBool("Enabled", true);
            }
        }
        else
        {
            _animation.SetBool("Begin", false);
            _animation.SetBool("Enabled", true);
        }

        if (_playerTriggered && _inputs.interact)
        {
            GameManager.instance._flags[_idFlag] = 1;
            SetCheckpoint();
            GameManager.instance.SaveGame();

            //reseta a vida do player
            _health._currentHealth = _health._maxHealth;
            _health.SetHealth(_health._currentHealth);
            _health.SetMana(_health._currentMana);
        }
    }

    void SetCheckpoint()
    {
        Scene _currentScene = SceneManager.GetActiveScene();
        GameManager.instance._checkpointScene = _currentScene.buildIndex;
        GameManager.instance._direction = _direction;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            _playerTriggered = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            _playerTriggered = false;
        }
    }
}
