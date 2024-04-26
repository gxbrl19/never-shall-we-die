using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Loading : MonoBehaviour
{
    //puxar o level index para carregar depois do load

    void Start()
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
