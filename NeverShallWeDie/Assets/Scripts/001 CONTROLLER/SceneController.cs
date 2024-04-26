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


    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        _player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
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
        _scriptablePosition._sceneTransition = true;
        _scriptablePosition._direction = _direction;
        _scriptablePosition._indexStartPosition = _indexPosition;
        //_scriptablePosition._initialValue = _playerPosition;
        //_scriptablePosition._initialValue = new Vector3(_startPoints[_indexPosition].position.x, _startPoints[_indexPosition].position.y, _startPoints[_indexPosition].position.z);
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
        _scriptablePosition._sceneTransition = true;
        _scriptablePosition._direction = GameManager.instance._direction;
        _scriptablePosition._indexStartPosition = 0;

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
