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

    private void Awake()
    {
        _player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        _input = _player.GetComponent<PlayerInputs>();
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
        _nextSceneName = GameManager.instance._currentPier;

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
        _scriptablePosition._sceneTransition = true;
        _scriptablePosition._direction = 1;
        _scriptablePosition._indexStartPosition = 1;
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
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            _triggered = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            _triggered = false;
        }
    }
}
