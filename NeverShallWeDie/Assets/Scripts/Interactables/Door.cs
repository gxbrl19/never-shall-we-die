using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : MonoBehaviour
{
    [SerializeField] private string _nextSceneName;
    [SerializeField] private int _direction;
    [SerializeField] private int _indexPosition;
    [SerializeField] private PlayerPosition _scriptablePosition;    
    public bool _withKey;

    private bool _locked;
    private bool _playerTriggered = false;
    PlayerInputs _input;
    PlayerHealth _health;

    private void Awake() {
        _input = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInputs>();
        _health = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>();
    }

    private void Start()
    {
        _locked = true;
        //[TO DO] verificar se a porta está aberta ou fechada no Game Manager;
    }

    private void Update() {
        if (_playerTriggered && _input.vertical > .05f)
        {
            if (_locked)
            {
                if (_withKey)
                {
                    _locked = false;
                    _withKey = false;
                }
                else
                {
                    
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

    void GetNextScene()
    {
        _scriptablePosition.SetAttributes(true, _direction, _indexPosition, _health._currentHealth, _health._currentMana);
        SceneManager.LoadScene("Scenes/" + _nextSceneName);
    }

    void OpenDoor()
    {
        _withKey = false;
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
        if(other.gameObject.layer == LayerMask.NameToLayer("Player") || other.gameObject.layer == LayerMask.NameToLayer("Invencible"))
        {
            _playerTriggered = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other) 
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("Player") || other.gameObject.layer == LayerMask.NameToLayer("Invencible"))
        {
            _playerTriggered = false;
        }
    }
}
