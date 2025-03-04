using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using FMODUnity;

public class Door : MonoBehaviour
{
    [SerializeField] private string _nextSceneName;
    [SerializeField] private int _direction;
    [SerializeField] private int _indexPosition;
    [SerializeField] private PlayerPosition _scriptablePosition;
    [SerializeField] private int _doorID;
    [SerializeField] private bool _withKey;

    [Header("FMOD Events")]
    [SerializeField] EventReference openSound;

    private bool _locked;
    private bool _playerTriggered = false;
    Animator _animation;
    PlayerInputs _input;
    SpriteRenderer _spritePadlock;

    private void Awake()
    {
        _animation = GetComponent<Animator>();
        _input = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInputs>();
    }

    private void Start()
    {
        _locked = GameManager.instance._doors[_doorID] == 0 ? true : false;
        _withKey = GameManager.instance._keys[_doorID] == 1 ? true : false;

        if (!_locked) { _animation.SetBool("Open", true); }
    }

    private void Update()
    {
        if (_playerTriggered && _input.interact)
        {
            if (_locked)
            {
                if (_withKey)
                {
                    _animation.SetBool("Opening", true);
                    RuntimeManager.PlayOneShot(openSound);
                }
                else
                {
                    //TODO: colocar som de tranca;
                    print("você precisa da chave");
                }
            }
            else
            {
                if (SceneExists("Scenes/" + _nextSceneName))
                {
                    GetNextScene();
                }
            }
        }
    }

    public void Unlocked() //chamado na animação
    {
        _locked = false;
        GameManager.instance._doors[_doorID] = 1;
    }

    void GetNextScene()
    {
        _scriptablePosition.SetAttributes(true, _direction, _indexPosition);
        SceneManager.LoadScene("Scenes/" + _nextSceneName);
    }

    private bool SceneExists(string sceneName)
    {
        for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
        {
            string scenePath = SceneUtility.GetScenePathByBuildIndex(i);
            if (scenePath.Contains(sceneName))
            {
                return true;
            }
        }
        return false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player") || other.gameObject.layer == LayerMask.NameToLayer("Invencible"))
        {
            _playerTriggered = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player") || other.gameObject.layer == LayerMask.NameToLayer("Invencible"))
        {
            _playerTriggered = false;
        }
    }
}
