using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnterNavigatorCell : MonoBehaviour
{
    [SerializeField] PlayerPosition _scriptablePosition;
    string _nextSceneName; //definido no GameManager
    bool _triggered;
    Player _player;
    PlayerInputs _input;
    Collider2D _collider;

    private void Awake()
    {
        _player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        _collider = GetComponent<Collider2D>();
        _input = _player.GetComponent<PlayerInputs>();

        _triggered = false;
    }

    void Update()
    {
        if (GameManager.instance._navigator == "CREW" || GameManager.instance._navigator == "COMPLETE" || GameManager.instance._navigator == "OTHER")
            _collider.enabled = true;
        else
            _collider.enabled = false;

        if (_triggered && _input.pressInteract)
        {
            _nextSceneName = "06/02(1)";
            _input.pressInteract = false;
            Enter();
        }
    }

    void Enter()
    {
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
        _scriptablePosition.SetAttributes(true, 1, 0);
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
