using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitShip : MonoBehaviour
{
    [SerializeField] PlayerPosition _scriptablePosition;
    string _nextSceneName; //definido no GameManager
    bool _triggered;
    Collider2D _collider;
    Player _player;
    PlayerInputs _input;
    BackgroundMusic _music;

    private void Awake()
    {
        _collider = GetComponent<Collider2D>();
        _player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        _input = _player.GetComponent<PlayerInputs>();
        _music = FindAnyObjectByType<BackgroundMusic>();

        _triggered = false;
    }

    void Update()
    {
        //verifica se a bandeira do navio está hasteada
        if (GameManager.instance._flags[0] == 1) { _collider.enabled = true; } else { _collider.enabled = false; }
        if (_triggered && _input.pressInteract) { Exit(); }
    }

    void Exit()
    {
        //verifica se já existe o save _lastPier com o nome da cena do Pier
        _nextSceneName = GameManager.instance._lastPier != "" ? GameManager.instance._lastPier : "06/H0";
        //_nextSceneName = GameManager.instance._lastPier != "" ? GameManager.instance._lastPier : "01/H1"; //TODO: DEMO

        if (SceneExists("Scenes/" + _nextSceneName))
        {
            _input.pressInteract = false;
            UIManager.instance.FadeIn();
            Invoke("GetNextScene", .5f);
        }
        else
        {
            Debug.Log("Local liberado somente na versão final");
        }
    }

    void GetNextScene() //chamado no Invoke na função Exit()
    {
        _scriptablePosition.SetAttributes(true, 1, 1);
        SceneManager.LoadScene("Scenes/" + _nextSceneName);

        switch (_nextSceneName)
        {
            case "06/H0":
                _music.MusicControl(6);
                break;
            case "01/H1":
                _music.MusicControl(1);
                break;
            case "01/H2":
                _music.MusicControl(1);
                break;
            case "02/H3":
                _music.MusicControl(2);
                break;
            case "02/H4":
                _music.MusicControl(2);
                break;
            case "03/H5":
                _music.MusicControl(3);
                break;
            case "03/H6":
                _music.MusicControl(3);
                break;
            case "04/H7":
                _music.MusicControl(4);
                break;
        }

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
            _triggered = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player") || other.gameObject.layer == LayerMask.NameToLayer("Invencible"))
        {
            _triggered = false;
        }
    }
}
