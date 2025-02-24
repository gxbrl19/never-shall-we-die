using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PixelDroidIntro : MonoBehaviour
{
    public GameObject _audio;
    //AudioSource _audio;

    private void Awake()
    {
        //_audio = GetComponent<AudioSource>();
    }

    public void FinishIntro()
    {
        Invoke("LoadMenu", 2f);
    }

    void LoadMenu()
    {
        SceneManager.LoadScene("Scenes/MainMenu");
    }

    public void PlayAudio()
    {
        //_audio.PlayOneShot(audio);
        _audio.SetActive(true);
    }
}