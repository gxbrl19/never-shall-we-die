using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PixelDroidIntro : MonoBehaviour
{
    AudioSource _audio;

    private void Awake()
    {
        _audio = GetComponent<AudioSource>();
    }

    public void FinishIntro()
    {
        Invoke("LoadMenu", 2f);
    }

    void LoadMenu()
    {
        SceneManager.LoadScene("Scenes/MainMenu");
    }

    public void PlayAudio(AudioClip audio)
    {
        _audio.PlayOneShot(audio);
    }
}