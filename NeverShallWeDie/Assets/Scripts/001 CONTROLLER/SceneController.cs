using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public static SceneController instance;

    [SerializeField] private string _nextSceneName;
    [SerializeField] private int _direction;
    [SerializeField] private int _indexPosition;
    //public Vector3 _playerPosition;
    public PlayerPosition _scriptablePosition;
    Player _player;
    PlayerHealth _health;


    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        _player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        _health = _player.GetComponent<PlayerHealth>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (SceneExists("Scenes/" + _nextSceneName))
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Player") || other.gameObject.layer == LayerMask.NameToLayer("Invencible"))
            {
                UIManager.instance.FadeIn();
                Invoke("GetNextScene", .5f);
            }
        }
        else
        {
            Debug.Log("Local liberado somente na versão final");
        }
    }

    void GetNextScene()
    {
        _scriptablePosition.SetAttributes(true, _direction, _indexPosition, _health._currentHealth, _health._currentMana);        
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

    public void GameOver()
    {
        _scriptablePosition.SetAttributes(true, GameManager.instance._direction, 0, _health._maxHealth, 0f);

        _player.gameObject.layer = LayerMask.NameToLayer("PlayerDead");
        _player.gameObject.GetComponent<Player>().DisableControls();
        _player.gameObject.GetComponent<PlayerInputs>().enabled = false;
        Invoke("SetGameOver", 2f);
    }

    public void SetGameOver() //chamado no Invoke na função GameOver()
    {
        UIManager.instance.FadeIn();
        Invoke("FinishGameOver", .5f);
    }

    void FinishGameOver()
    {
        SceneManager.LoadScene(GameManager.instance._checkpointScene);
        _player.gameObject.GetComponent<Player>().EnabledControls();
        _player.gameObject.GetComponent<PlayerInputs>().enabled = true;
    }
}