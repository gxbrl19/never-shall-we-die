using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Loading : MonoBehaviour
{
    void Start()
    {
        Invoke("StartTheCoroutine", 3f); //um delay para chamar a corrotina
    }

    void StartTheCoroutine()
    {
        StartCoroutine(LoadingScene());
    }

    IEnumerator LoadingScene()
    {
        AsyncOperation _asynLoad = SceneManager.LoadSceneAsync(PlayerPrefs.GetInt("Scene"));

        while (!_asynLoad.isDone)
        {
            yield return null;
        }
    }
}
