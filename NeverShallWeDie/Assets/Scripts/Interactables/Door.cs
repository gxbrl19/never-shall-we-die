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
    private PlayerInputs _input;

    private void Awake() {
        _input = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInputs>();
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
        _scriptablePosition._sceneTransition = true;
        _scriptablePosition._direction = _direction;
        _scriptablePosition._indexStartPosition = _indexPosition;
        //_scriptablePosition._initialValue = _playerPosition;
        //_scriptablePosition._initialValue = new Vector3(_startPoints[_indexPosition].position.x, _startPoints[_indexPosition].position.y, _startPoints[_indexPosition].position.z);
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
