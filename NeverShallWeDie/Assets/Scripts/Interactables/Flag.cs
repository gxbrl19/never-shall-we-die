using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using FMODUnity;

public class Flag : MonoBehaviour
{
    public int _idFlag;
    public int _direction = 1;
    public Transform healingPoint;
    public GameObject healingObject;
    [SerializeField] EventReference _checkpointSound;
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
            if (_playerTriggered && _inputs.pressInteract)
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

        if (_playerTriggered && _inputs.pressInteract)
        {
            GameManager.instance._flags[_idFlag] = 1;
            SetCheckpoint();

            //reseta a vida do player
            _health.currentHealth = _health.maxHealth;
            _health.SetHealth(_health.currentHealth);
            _health.SetHealing(_health.currentHealing);
        }
    }

    void SetCheckpoint()
    {
        Instantiate(healingObject, healingPoint.position, Quaternion.identity);
        _inputs.pressInteract = false;
        Scene _currentScene = SceneManager.GetActiveScene();
        GameManager.instance._checkpointScene = _currentScene.buildIndex;
        GameManager.instance._direction = _direction;
        RuntimeManager.PlayOneShot(_checkpointSound);
        GameManager.instance.SaveGame();
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
