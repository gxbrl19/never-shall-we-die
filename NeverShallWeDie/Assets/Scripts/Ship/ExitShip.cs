using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitShip : MonoBehaviour
{
    [SerializeField] PlayerPosition _scriptablePosition;
    string _nextSceneName; //definido no GameManager
    bool _triggered;
    Player _player;
    PlayerInputs _input;
    PlayerHealth _health;

    private void Awake()
    {
        _player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        _input = _player.GetComponent<PlayerInputs>();
        _health = _player.GetComponent<PlayerHealth>();

        _triggered = false;
    }

    void Update()
    {
        if (_triggered && _input.interact)
        {
            Exit();
        }
    }

    void Exit()
    {
        //verifica se já existe o PlayerPrefs com o nome da cena do Pier selecionado
        _nextSceneName = PlayerPrefs.HasKey("Pier") ? PlayerPrefs.GetString("Pier") : "06/H0";

        if (SceneExists("Scenes/" + _nextSceneName))
        {
            UIManager.instance.FadeIn();
            Invoke("GetNextScene", .5f);
        }
        else
        {
            Debug.Log("Local liberado somente na versão final");
        }
    }

    void GetNextScene()
    {
        _scriptablePosition.SetAttributes(true, 1, 1);
        SceneManager.LoadScene("Scenes/" + _nextSceneName);

        switch (_nextSceneName)
        {
            case "06/H0":
                BackgroundMusic.instance.ChangeMusic(BackgroundMusic.instance._kingdomTheme);
                break;
            case "01/H1":
                BackgroundMusic.instance.ChangeMusic(BackgroundMusic.instance._forestTheme);
                break;
            case "01/H2":
                BackgroundMusic.instance.ChangeMusic(BackgroundMusic.instance._minesTheme);
                break;
            case "02/H3":
                BackgroundMusic.instance.ChangeMusic(BackgroundMusic.instance._mizutonTheme);
                break;
            case "02/H4":
                BackgroundMusic.instance.ChangeMusic(BackgroundMusic.instance._mizutonTheme);
                break;
            case "03/H5":
                BackgroundMusic.instance.ChangeMusic(BackgroundMusic.instance._cemeteryTheme);
                break;
            case "03/H6":
                BackgroundMusic.instance.ChangeMusic(BackgroundMusic.instance._cemeteryTheme);
                break;
            case "04/H7":
                BackgroundMusic.instance.ChangeMusic(BackgroundMusic.instance._prisonTheme);
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
